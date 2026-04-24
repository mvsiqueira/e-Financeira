using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Linq;
using System.Security;

namespace EFinanceiraApp;

internal static class EFinanceiraProcessor
{
    private const string SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
    private const string DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
    private const string EncryptedBatchNamespace = "http://www.eFinanceira.gov.br/schemas/envioLoteCriptografado/v1_2_0";
    private const string IdAttributeName = "id";

    private static readonly string[] SupportedEventTags =
    [
        "evtCadDeclarante",
        "evtAberturaeFinanceira",
        "evtCadIntermediario",
        "evtCadPatrocinado",
        "evtExclusaoeFinanceira",
        "evtExclusao",
        "evtFechamentoeFinanceira",
        "evtMovOpFin",
        "evtMovPP"
    ];

    public static string BuildSignedOutputPath(string xmlPath)
    {
        string directory = Path.GetDirectoryName(xmlPath) ?? string.Empty;
        string fileName = Path.GetFileNameWithoutExtension(xmlPath);
        string extension = Path.GetExtension(xmlPath);
        return Path.Combine(directory, $"{fileName}-ASSINADO{extension}");
    }

    public static string BuildEncryptedOutputPath(string signedXmlPath)
    {
        string directory = Path.GetDirectoryName(signedXmlPath) ?? string.Empty;
        string fileName = Path.GetFileNameWithoutExtension(signedXmlPath);
        string extension = Path.GetExtension(signedXmlPath);
        return Path.Combine(directory, $"{fileName}-Criptografado{extension}");
    }

    public static string SignXmlBatch(string xmlPath, X509Certificate2 certificate)
    {
        XmlDocument xmlDocument = LoadXml(xmlPath);
        XmlNodeList events = SelectBatchEvents(xmlDocument);

        if (events.Count == 0)
        {
            throw new InvalidOperationException(
                "Nao foram encontrados eventos do lote para assinatura. O XML precisa conter loteEventosAssincrono/eventos/evento ou loteEventos/evento.");
        }

        foreach (XmlNode eventNode in events)
        {
            XmlDocument eventDocument = new() { PreserveWhitespace = true };
            eventDocument.LoadXml(eventNode.InnerXml);

            string eventTag = ResolveEventTag(eventDocument);
            if (string.IsNullOrWhiteSpace(eventTag))
            {
                throw new InvalidOperationException("Foi encontrado um evento de tipo nao suportado para assinatura.");
            }

            XmlDocument signedEvent = SignEventXml(eventDocument, certificate, eventTag);
            eventNode.InnerXml = signedEvent.InnerXml;
        }

        string outputPath = BuildSignedOutputPath(xmlPath);
        xmlDocument.Save(outputPath);
        return outputPath;
    }

    public static string EncryptSignedBatch(string signedXmlPath, string certificatePath)
    {
        XmlDocument xmlDocument = LoadXml(signedXmlPath);
        X509Certificate2 certificate = X509CertificateLoader.LoadCertificateFromFile(certificatePath);
        string thumbprint = NormalizeThumbprint(certificate.Thumbprint);

        byte[] aesKey = RandomNumberGenerator.GetBytes(16);
        byte[] aesIv = RandomNumberGenerator.GetBytes(16);

        string encryptedBatch = EncryptXmlWithAes(xmlDocument, aesKey, aesIv);
        string encryptedKey = EncryptAesMaterial(certificate, aesKey, aesIv);

        XmlDocument encryptedXml = BuildEncryptedXml(thumbprint, encryptedKey, encryptedBatch);
        string outputPath = BuildEncryptedOutputPath(signedXmlPath);
        encryptedXml.Save(outputPath);
        return outputPath;
    }

    private static XmlDocument LoadXml(string path)
    {
        XmlDocument xmlDocument = new() { PreserveWhitespace = true };
        xmlDocument.Load(path);
        return xmlDocument;
    }

    private static XmlNodeList SelectBatchEvents(XmlDocument xmlDocument)
    {
        XmlNamespaceManager namespaceManager = new(xmlDocument.NameTable);
        namespaceManager.AddNamespace("ef", xmlDocument.DocumentElement?.NamespaceURI ?? string.Empty);

        XmlNodeList? asyncEvents = xmlDocument.SelectNodes("//ef:loteEventosAssincrono/ef:eventos/ef:evento", namespaceManager);
        if (asyncEvents is not null && asyncEvents.Count > 0)
        {
            return asyncEvents;
        }

        XmlNodeList? syncEvents = xmlDocument.SelectNodes("//ef:loteEventos/ef:evento", namespaceManager);
        if (syncEvents is not null)
        {
            return syncEvents;
        }

        XmlDocument emptyDocument = new();
        emptyDocument.LoadXml("<root />");
        return emptyDocument.SelectNodes("/root/nonexistent")!;
    }

    private static string ResolveEventTag(XmlDocument eventDocument)
    {
        foreach (string eventTag in SupportedEventTags)
        {
            if (eventDocument.GetElementsByTagName(eventTag).Count > 0)
            {
                return eventTag;
            }
        }

        return string.Empty;
    }

    private static XmlDocument SignEventXml(XmlDocument eventDocument, X509Certificate2 certificate, string eventTag)
    {
        XmlNodeList eventNodes = eventDocument.GetElementsByTagName(eventTag);
        if (eventNodes.Count == 0 || eventNodes[0] is not XmlElement eventElement)
        {
            throw new InvalidOperationException($"A tag do evento {eventTag} nao foi encontrada no XML.");
        }

        string? eventId = eventElement.GetAttribute(IdAttributeName);
        if (string.IsNullOrWhiteSpace(eventId))
        {
            throw new InvalidOperationException($"A tag {eventTag} precisa possuir o atributo '{IdAttributeName}'.");
        }

        RSA? privateKey = certificate.GetRSAPrivateKey();
        if (privateKey is null)
        {
            throw new InvalidOperationException("O certificado selecionado nao possui uma chave privada RSA acessivel.");
        }

        SignedXml signedXml = new(eventElement)
        {
            SigningKey = privateKey
        };
        ArgumentNullException.ThrowIfNull(signedXml.SignedInfo);
        signedXml.SignedInfo.SignatureMethod = SignatureMethod;

        Reference reference = new($"#{eventId}")
        {
            DigestMethod = DigestMethod
        };
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(false));
        reference.AddTransform(new XmlDsigC14NTransform(false));
        signedXml.AddReference(reference);

        KeyInfo keyInfo = new();
        keyInfo.AddClause(new KeyInfoX509Data(certificate));
        signedXml.KeyInfo = keyInfo;

        signedXml.ComputeSignature();
        XmlElement signatureElement = signedXml.GetXml();
        eventElement.ParentNode!.AppendChild(eventDocument.ImportNode(signatureElement, true));

        XmlDocument signedDocument = new() { PreserveWhitespace = true };
        signedDocument.LoadXml(eventDocument.OuterXml);
        return signedDocument;
    }

    private static string EncryptXmlWithAes(XmlDocument xmlDocument, byte[] key, byte[] iv)
    {
        using Aes aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;
        aes.Key = key;
        aes.IV = iv;

        using MemoryStream memory = new();
        using ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
        using CryptoStream cryptoStream = new(memory, transform, CryptoStreamMode.Write);

        byte[] xmlBytes = Encoding.UTF8.GetBytes(xmlDocument.OuterXml);
        cryptoStream.Write(xmlBytes, 0, xmlBytes.Length);
        cryptoStream.FlushFinalBlock();

        return Convert.ToBase64String(memory.ToArray());
    }

    private static string EncryptAesMaterial(X509Certificate2 certificate, byte[] key, byte[] iv)
    {
        using RSA? rsa = certificate.GetRSAPublicKey();
        if (rsa is null)
        {
            throw new InvalidOperationException("O certificado informado para criptografia nao possui chave publica RSA.");
        }

        byte[] payload = key.Concat(iv).ToArray();
        byte[] encryptedPayload = rsa.Encrypt(payload, RSAEncryptionPadding.Pkcs1);
        return Convert.ToBase64String(encryptedPayload);
    }

    private static XmlDocument BuildEncryptedXml(string thumbprint, string encryptedKey, string encryptedBatch)
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <eFinanceira xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="{EncryptedBatchNamespace}">
              <loteCriptografado>
                <id>{EscapeXml(Guid.NewGuid().ToString())}</id>
                <idCertificado>{EscapeXml(thumbprint)}</idCertificado>
                <chave>{EscapeXml(encryptedKey)}</chave>
                <lote>{EscapeXml(encryptedBatch)}</lote>
              </loteCriptografado>
            </eFinanceira>
            """;

        XmlDocument xmlDocument = new();
        xmlDocument.LoadXml(xml);
        return xmlDocument;
    }

    private static string NormalizeThumbprint(string? thumbprint)
    {
        return (thumbprint ?? string.Empty).Replace(" ", string.Empty).Trim().ToLowerInvariant();
    }

    private static string EscapeXml(string value)
    {
        return SecurityElement.Escape(value) ?? string.Empty;
    }
}
