using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ExemploCriptografiaLoteEFinanceira
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Exemplo geracao lote sincrono criptografado da e-Financeira");
                Console.WriteLine(string.Empty);
                Console.WriteLine("ExemploCriptografiaLoteSincrono.exe [path_arquivo_lote_a_ser_criptografado] [thumbprint_certificado]");
                return;
            }

            string pathArquivoLote = args[0];

            XmlDocument xmlDocLote = new XmlDocument();
            xmlDocLote.Load(pathArquivoLote);

            // Encripta xml lote com chave AES randomica gerada
            byte[] chaveAES;
            byte[] vetorAES;
            string xmlLoteCriptografadoBase64 = EncriptaXmlComChaveAES(xmlDocLote, out chaveAES, out vetorAES);

            // Encripta chave AES com chave publica certificado servidor
            string thumbprintCertificado = args[1];
            string chaveLoteCriptografadoBase64 = EncriptaChaveAESComChavePublicaCertificadoServidor(chaveAES, vetorAES, thumbprintCertificado);

            // Gera arquivo Xml no formato definido para lote encriptado da e-Financeira            
            string pathArquivoSaida = GerarXml(pathArquivoLote, xmlLoteCriptografadoBase64, thumbprintCertificado, chaveLoteCriptografadoBase64);

            Console.WriteLine("Arquivo gerado : " + pathArquivoSaida);
        }


        private static string GerarXml(string pathArquivoLote, string xmlLoteCriptografadoBase64, string thumbprintCertificado, string chaveLoteCriptografadoBase64)
        {
            Schema.eFinanceira eFinanceira = new Schema.eFinanceira();
            Schema.eFinanceiraLoteCriptografado loteCriptografado = new Schema.eFinanceiraLoteCriptografado();
            loteCriptografado.id = Guid.NewGuid().ToString(); // Id a ser definido pela da instituição financeira, para utilização dela própria.
            loteCriptografado.idCertificado = thumbprintCertificado;
            loteCriptografado.chave = chaveLoteCriptografadoBase64;
            loteCriptografado.lote = xmlLoteCriptografadoBase64;
            eFinanceira.loteCriptografado = loteCriptografado;
            XmlDocument xml = Serializar(eFinanceira);

            string pathLoteCriptografado = pathArquivoLote;
            string nomeArq = Path.GetFileName(pathArquivoLote);
            pathLoteCriptografado = pathLoteCriptografado.Replace(nomeArq, Path.GetFileNameWithoutExtension(pathArquivoLote) + "-Criptografado" + Path.GetExtension(pathArquivoLote));

            xml.Save(pathLoteCriptografado);

            return pathLoteCriptografado;
        }

        private static string EncriptaChaveAESComChavePublicaCertificadoServidor(byte[] chaveAES, byte[] vetorAES, string thumbprintCertificado)
        {
            chaveAES = chaveAES.Concat(vetorAES).ToArray();
            byte[] chaveCriptografada = null;

            X509Certificate2 certificadoServidor = ObtemCertificadoPeloThumbprint(thumbprintCertificado);

            PublicKey chavePublica = certificadoServidor.PublicKey;
            using (RSACryptoServiceProvider rsa = chavePublica.Key as RSACryptoServiceProvider)
            {
                chaveCriptografada = rsa.Encrypt(chaveAES, false);
            }

            return Convert.ToBase64String(chaveCriptografada);
        }

        private static string EncriptaXmlComChaveAES(XmlDocument xmlDocLote, out byte[] chaveAES, out byte[] vetorAES)
        {
            string xmlLoteCriptografadoBase64;

            const int KEY_SIZE = 128; // AES-128
            chaveAES = GerarChaveRandomica(KEY_SIZE / 8);
            vetorAES = GerarChaveRandomica(KEY_SIZE / 8);

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = chaveAES;
                aes.IV = vetorAES;

                ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
                Stream streamXmlLoteEncriptado = new MemoryStream();

                CryptoStream cryptoStream = new CryptoStream(streamXmlLoteEncriptado, cryptoTransform, CryptoStreamMode.Write);

                byte[] xmlDocBytes = Encoding.UTF8.GetBytes(xmlDocLote.OuterXml);

                cryptoStream.Write(xmlDocBytes, 0, xmlDocBytes.Length);

                streamXmlLoteEncriptado.Flush();
                cryptoStream.FlushFinalBlock();
                cryptoStream.Flush();

                xmlLoteCriptografadoBase64 = Convert.ToBase64String(StreamToByteArray(streamXmlLoteEncriptado));

                cryptoStream.Close();
                streamXmlLoteEncriptado.Close();
                cryptoTransform.Dispose();
            }

            return xmlLoteCriptografadoBase64;
        }


        private static byte[] GerarChaveRandomica(int tamanho)
        {
            byte[] chave = new byte[tamanho];

            using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(chave);
            }

            return chave;
        }


        private static byte[] StreamToByteArray(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }


        private static X509Certificate2 ObtemCertificadoPeloThumbprint(string thumbprint)
        {
            X509Store storeMy = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            storeMy.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certColl = storeMy.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            storeMy.Close();
            return certColl[0];
        }


        private static XmlDocument Serializar(object objeto)
        {
            XmlSerializer serializer = new XmlSerializer(objeto.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                XmlTextWriter xtw = new XmlTextWriter(stream, new UTF8Encoding(false));
                serializer.Serialize(xtw, objeto);
                string xml = Encoding.UTF8.GetString(stream.ToArray());
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                return xmlDoc;
            }
        }


    }
}
