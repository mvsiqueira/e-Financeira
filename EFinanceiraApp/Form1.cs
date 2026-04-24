using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace EFinanceiraApp;

public partial class Form1 : Form
{
    private X509Certificate2? _signingCertificate;
    private readonly Color _surfaceColor = Color.FromArgb(245, 247, 250);
    private readonly Color _panelColor = Color.White;
    private readonly Color _borderColor = Color.FromArgb(220, 226, 232);
    private readonly Color _accentColor = Color.FromArgb(0, 102, 204);
    private readonly Color _primaryButtonColor = Color.FromArgb(19, 91, 194);
    private readonly Color _secondaryButtonColor = Color.FromArgb(236, 240, 245);

    public Form1()
    {
        InitializeComponent();
        ApplyWindowIcon();
        ConfigureVisualStyle();
    }

    private void BtnSelecionarXml_Click(object? sender, EventArgs e)
    {
        if (openXmlDialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        txtXmlEntrada.Text = openXmlDialog.FileName;
        LimparArquivosGerados();
        Log($"Arquivo XML selecionado: {openXmlDialog.FileName}");
    }

    private void BtnSelecionarCertificadoAssinatura_Click(object? sender, EventArgs e)
    {
        using X509Store currentUserStore = new(StoreName.My, StoreLocation.CurrentUser);
        currentUserStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        using X509Store localMachineStore = new(StoreName.My, StoreLocation.LocalMachine);
        localMachineStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        X509Certificate2Collection certificados = new();
        certificados.AddRange(currentUserStore.Certificates);
        certificados.AddRange(localMachineStore.Certificates);

        X509Certificate2Collection certificadosAssinatura = certificados.Find(
            X509FindType.FindByKeyUsage,
            X509KeyUsageFlags.DigitalSignature,
            validOnly: false);

        X509Certificate2Collection certificadosSelecionados = X509Certificate2UI.SelectFromCollection(
            certificadosAssinatura,
            "Certificados de assinatura",
            "Selecione o certificado digital para assinar o lote da e-Financeira.",
            X509SelectionFlag.SingleSelection);

        if (certificadosSelecionados.Count == 0)
        {
            return;
        }

        _signingCertificate = certificadosSelecionados[0];
        txtCertificadoAssinatura.Text = $"{_signingCertificate.Subject} | Thumbprint: {_signingCertificate.Thumbprint}";
        Log($"Certificado de assinatura selecionado: {_signingCertificate.Subject}");
    }

    private void BtnSelecionarCertificadoCriptografia_Click(object? sender, EventArgs e)
    {
        if (openCertificateDialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        txtCertificadoCriptografia.Text = openCertificateDialog.FileName;
        Log($"Certificado para criptografia selecionado: {openCertificateDialog.FileName}");
    }

    private void BtnAssinar_Click(object? sender, EventArgs e)
    {
        try
        {
            string xmlEntrada = ValidarArquivoEntrada();
            X509Certificate2 certificado = ValidarCertificadoAssinatura();

            string caminhoAssinado = EFinanceiraProcessor.SignXmlBatch(xmlEntrada, certificado);
            txtXmlAssinado.Text = caminhoAssinado;
            txtXmlCriptografado.Clear();

            Log($"Arquivo assinado gerado em: {caminhoAssinado}");
            MessageBox.Show(
                this,
                $"Arquivo assinado gerado com sucesso:\n{caminhoAssinado}",
                "Assinatura concluida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowError("Falha ao assinar o XML.", ex);
        }
    }

    private void BtnCriptografar_Click(object? sender, EventArgs e)
    {
        try
        {
            string certificadoCriptografia = ValidarArquivoCertificadoCriptografia();
            string xmlParaCriptografar = ObterArquivoParaCriptografia();

            string caminhoCriptografado = EFinanceiraProcessor.EncryptSignedBatch(xmlParaCriptografar, certificadoCriptografia);
            txtXmlCriptografado.Text = caminhoCriptografado;

            Log($"Arquivo criptografado gerado em: {caminhoCriptografado}");
            MessageBox.Show(
                this,
                $"Arquivo criptografado gerado com sucesso:\n{caminhoCriptografado}",
                "Criptografia concluida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowError("Falha ao criptografar o XML assinado.", ex);
        }
    }

    private string ValidarArquivoEntrada()
    {
        string caminho = txtXmlEntrada.Text.Trim();
        if (string.IsNullOrWhiteSpace(caminho))
        {
            throw new InvalidOperationException("Selecione o arquivo XML de lote.");
        }

        if (!File.Exists(caminho))
        {
            throw new FileNotFoundException("O arquivo XML informado nao foi encontrado.", caminho);
        }

        return caminho;
    }

    private X509Certificate2 ValidarCertificadoAssinatura()
    {
        if (_signingCertificate is null)
        {
            throw new InvalidOperationException("Selecione um certificado instalado para assinatura.");
        }

        if (!_signingCertificate.HasPrivateKey)
        {
            throw new InvalidOperationException("O certificado selecionado nao possui chave privada.");
        }

        return _signingCertificate;
    }

    private string ValidarArquivoCertificadoCriptografia()
    {
        string caminho = txtCertificadoCriptografia.Text.Trim();
        if (string.IsNullOrWhiteSpace(caminho))
        {
            throw new InvalidOperationException("Selecione o arquivo de certificado para criptografia.");
        }

        if (!File.Exists(caminho))
        {
            throw new FileNotFoundException("O arquivo de certificado informado nao foi encontrado.", caminho);
        }

        return caminho;
    }

    private string ObterArquivoParaCriptografia()
    {
        string xmlAssinado = txtXmlAssinado.Text.Trim();
        if (!string.IsNullOrWhiteSpace(xmlAssinado) && File.Exists(xmlAssinado))
        {
            return xmlAssinado;
        }

        throw new InvalidOperationException("Nenhum arquivo assinado foi gerado nesta sessão. Assine o lote antes de criptografar.");
    }

    private void LimparArquivosGerados()
    {
        txtXmlAssinado.Clear();
        txtXmlCriptografado.Clear();
    }

    private void Log(string message)
    {
        txtStatus.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
    }

    private void ShowError(string title, Exception ex)
    {
        Log($"{title} {ex.Message}");
        MessageBox.Show(
            this,
            $"{title}\n{ex.Message}",
            "Erro",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private void BtnLimparLog_Click(object? sender, EventArgs e)
    {
        txtStatus.Clear();
    }

    private void BtnVisualizarXmlEntrada_Click(object? sender, EventArgs e)
    {
        TryOpenFile(txtXmlEntrada.Text, "arquivo XML de lote");
    }

    private void BtnVisualizarXmlAssinado_Click(object? sender, EventArgs e)
    {
        TryOpenFile(txtXmlAssinado.Text, "arquivo assinado");
    }

    private void BtnVisualizarXmlCriptografado_Click(object? sender, EventArgs e)
    {
        TryOpenFile(txtXmlCriptografado.Text, "arquivo criptografado");
    }

    private void ConfigureVisualStyle()
    {
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        BackColor = _surfaceColor;

        ConfigureFieldLabel(lblXmlEntrada);
        ConfigureFieldLabel(lblCertificadoAssinatura);
        ConfigureFieldLabel(lblXmlAssinado);
        ConfigureFieldLabel(lblCertificadoCriptografia);
        ConfigureFieldLabel(lblXmlCriptografado);
        ConfigureArquivoPanel();
        ConfigureGroupBox(grpAssinatura);
        ConfigureGroupBox(grpCriptografia);

        ConfigureReadOnlyTextBox(txtXmlEntrada);
        ConfigureReadOnlyTextBox(txtCertificadoAssinatura);
        ConfigureReadOnlyTextBox(txtXmlAssinado);
        ConfigureReadOnlyTextBox(txtCertificadoCriptografia);
        ConfigureReadOnlyTextBox(txtXmlCriptografado);

        txtStatus.BackColor = Color.FromArgb(251, 252, 253);
        txtStatus.BorderStyle = BorderStyle.FixedSingle;
        txtStatus.Font = new Font("Consolas", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

        lblStatus.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        lblStatus.ForeColor = Color.FromArgb(50, 58, 69);
        layoutLogHeader.BackColor = Color.Transparent;

        ConfigureButton(btnSelecionarXml, _secondaryButtonColor, Color.FromArgb(35, 45, 58));
        ConfigureButton(btnSelecionarCertificadoAssinatura, _secondaryButtonColor, Color.FromArgb(35, 45, 58));
        ConfigureButton(btnSelecionarCertificadoCriptografia, _secondaryButtonColor, Color.FromArgb(35, 45, 58));
        ConfigureButton(btnVisualizarXmlEntrada, _secondaryButtonColor, Color.FromArgb(35, 45, 58));
        ConfigureButton(btnVisualizarXmlAssinado, _secondaryButtonColor, Color.FromArgb(35, 45, 58));
        ConfigureButton(btnVisualizarXmlCriptografado, _secondaryButtonColor, Color.FromArgb(35, 45, 58));
        ConfigureButton(btnAssinar, _primaryButtonColor, Color.White);
        ConfigureButton(btnCriptografar, _primaryButtonColor, Color.White);
        ConfigureButton(btnLimparLog, Color.FromArgb(255, 244, 228), Color.FromArgb(132, 76, 0));

        btnAssinar.Text = "1. Assinar XML";
        btnCriptografar.Text = "2. Criptografar XML";
    }

    private void ConfigureGroupBox(GroupBox groupBox)
    {
        groupBox.BackColor = _surfaceColor;
        groupBox.ForeColor = Color.FromArgb(37, 45, 56);
        groupBox.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        groupBox.Padding = new Padding(12, 14, 12, 10);
    }

    private void ConfigureArquivoPanel()
    {
        grpArquivo.BackColor = _surfaceColor;
        grpArquivo.ForeColor = Color.FromArgb(37, 45, 56);
        grpArquivo.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        grpArquivo.Padding = new Padding(12, 14, 12, 10);
    }

    private void ConfigureFieldLabel(Label label)
    {
        label.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        label.ForeColor = Color.FromArgb(42, 49, 58);
        label.AutoSize = true;
    }

    private void ConfigureReadOnlyTextBox(TextBox textBox)
    {
        textBox.BackColor = Color.White;
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.ForeColor = Color.FromArgb(42, 49, 58);
        textBox.Margin = new Padding(3, 4, 3, 4);
    }

    private void ConfigureButton(Button button, Color backColor, Color foreColor)
    {
        button.BackColor = backColor;
        button.ForeColor = foreColor;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = _borderColor;
        button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.04F);
        button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.03F);
        button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
        button.Height = 28;
        button.Margin = new Padding(8, 3, 0, 3);
        button.UseVisualStyleBackColor = false;
    }

    private void TryOpenFile(string path, string description)
    {
        string filePath = path.Trim();
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            MessageBox.Show(
                this,
                $"Nenhum {description} disponível para visualização.",
                "Arquivo não encontrado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }

    private void ApplyWindowIcon()
    {
        string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "app-icon.ico");
        if (!File.Exists(iconPath))
        {
            return;
        }

        using Icon icon = new(iconPath);
        Icon = (Icon)icon.Clone();
    }
}
