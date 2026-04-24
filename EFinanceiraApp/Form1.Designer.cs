#nullable enable

namespace EFinanceiraApp;

partial class Form1
{
    private System.ComponentModel.IContainer? components = null;
    private TableLayoutPanel layoutPrincipal = null!;
    private GroupBox grpArquivo = null!;
    private TableLayoutPanel layoutArquivo = null!;
    private Label lblXmlEntrada = null!;
    private TextBox txtXmlEntrada = null!;
    private Button btnVisualizarXmlEntrada = null!;
    private Button btnSelecionarXml = null!;
    private GroupBox grpAssinatura = null!;
    private TableLayoutPanel layoutAssinatura = null!;
    private Label lblCertificadoAssinatura = null!;
    private TextBox txtCertificadoAssinatura = null!;
    private Button btnSelecionarCertificadoAssinatura = null!;
    private Label lblXmlAssinado = null!;
    private TextBox txtXmlAssinado = null!;
    private Button btnVisualizarXmlAssinado = null!;
    private Button btnAssinar = null!;
    private GroupBox grpCriptografia = null!;
    private TableLayoutPanel layoutCriptografia = null!;
    private Label lblCertificadoCriptografia = null!;
    private TextBox txtCertificadoCriptografia = null!;
    private Button btnSelecionarCertificadoCriptografia = null!;
    private Label lblXmlCriptografado = null!;
    private TextBox txtXmlCriptografado = null!;
    private Button btnVisualizarXmlCriptografado = null!;
    private Button btnCriptografar = null!;
    private TableLayoutPanel layoutLogSection = null!;
    private TableLayoutPanel layoutLogHeader = null!;
    private Label lblStatus = null!;
    private Button btnLimparLog = null!;
    private TextBox txtStatus = null!;
    private OpenFileDialog openXmlDialog = null!;
    private OpenFileDialog openCertificateDialog = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        layoutPrincipal = new TableLayoutPanel();
        grpArquivo = new GroupBox();
        layoutArquivo = new TableLayoutPanel();
        lblXmlEntrada = new Label();
        txtXmlEntrada = new TextBox();
        btnVisualizarXmlEntrada = new Button();
        btnSelecionarXml = new Button();
        grpAssinatura = new GroupBox();
        layoutAssinatura = new TableLayoutPanel();
        lblCertificadoAssinatura = new Label();
        txtCertificadoAssinatura = new TextBox();
        btnSelecionarCertificadoAssinatura = new Button();
        lblXmlAssinado = new Label();
        txtXmlAssinado = new TextBox();
        btnVisualizarXmlAssinado = new Button();
        btnAssinar = new Button();
        grpCriptografia = new GroupBox();
        layoutCriptografia = new TableLayoutPanel();
        lblCertificadoCriptografia = new Label();
        txtCertificadoCriptografia = new TextBox();
        btnSelecionarCertificadoCriptografia = new Button();
        lblXmlCriptografado = new Label();
        txtXmlCriptografado = new TextBox();
        btnVisualizarXmlCriptografado = new Button();
        btnCriptografar = new Button();
        layoutLogSection = new TableLayoutPanel();
        layoutLogHeader = new TableLayoutPanel();
        lblStatus = new Label();
        btnLimparLog = new Button();
        txtStatus = new TextBox();
        openXmlDialog = new OpenFileDialog();
        openCertificateDialog = new OpenFileDialog();
        layoutPrincipal.SuspendLayout();
        grpArquivo.SuspendLayout();
        layoutArquivo.SuspendLayout();
        grpAssinatura.SuspendLayout();
        layoutAssinatura.SuspendLayout();
        grpCriptografia.SuspendLayout();
        layoutCriptografia.SuspendLayout();
        layoutLogHeader.SuspendLayout();
        SuspendLayout();

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(980, 660);
        MinimumSize = new Size(920, 620);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Assinador e Criptografador e-Financeira";

        openXmlDialog.Filter = "Arquivos XML (*.xml)|*.xml";
        openXmlDialog.Title = "Selecione o arquivo XML de lote";

        openCertificateDialog.Filter = "Certificados (*.cer;*.crt;*.der)|*.cer;*.crt;*.der|Todos os arquivos (*.*)|*.*";
        openCertificateDialog.Title = "Selecione o certificado para criptografia";

        layoutPrincipal.ColumnCount = 1;
        layoutPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutPrincipal.Controls.Add(grpArquivo, 0, 0);
        layoutPrincipal.Controls.Add(grpAssinatura, 0, 1);
        layoutPrincipal.Controls.Add(grpCriptografia, 0, 2);
        layoutPrincipal.Controls.Add(layoutLogSection, 0, 3);
        layoutPrincipal.Dock = DockStyle.Fill;
        layoutPrincipal.Padding = new Padding(14, 12, 14, 14);
        layoutPrincipal.RowCount = 4;
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        grpArquivo.Controls.Add(layoutArquivo);
        grpArquivo.Dock = DockStyle.Top;
        grpArquivo.AutoSize = true;
        grpArquivo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpArquivo.Margin = new Padding(0, 0, 0, 8);
        grpArquivo.Padding = new Padding(12);
        grpArquivo.Text = "Arquivo";

        layoutArquivo.ColumnCount = 3;
        layoutArquivo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        layoutArquivo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutArquivo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        layoutArquivo.Controls.Add(lblXmlEntrada, 0, 0);
        layoutArquivo.Controls.Add(txtXmlEntrada, 1, 0);
        layoutArquivo.Controls.Add(btnSelecionarXml, 2, 0);
        layoutArquivo.Controls.Add(btnVisualizarXmlEntrada, 2, 1);
        layoutArquivo.AutoSize = true;
        layoutArquivo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        layoutArquivo.Dock = DockStyle.Top;
        layoutArquivo.RowCount = 2;
        layoutArquivo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layoutArquivo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        lblXmlEntrada.Anchor = AnchorStyles.Left;
        lblXmlEntrada.AutoSize = true;
        lblXmlEntrada.Text = "Arquivo XML de lote";

        txtXmlEntrada.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtXmlEntrada.ReadOnly = true;

        btnVisualizarXmlEntrada.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnVisualizarXmlEntrada.Text = "Visualizar arquivo";
        btnVisualizarXmlEntrada.Click += BtnVisualizarXmlEntrada_Click;

        btnSelecionarXml.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnSelecionarXml.Text = "Selecionar XML";
        btnSelecionarXml.Click += BtnSelecionarXml_Click;

        grpAssinatura.Controls.Add(layoutAssinatura);
        grpAssinatura.Dock = DockStyle.Top;
        grpAssinatura.AutoSize = true;
        grpAssinatura.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpAssinatura.Margin = new Padding(0, 0, 0, 8);
        grpAssinatura.Padding = new Padding(12);
        grpAssinatura.Text = "Assinatura";

        layoutAssinatura.ColumnCount = 3;
        layoutAssinatura.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        layoutAssinatura.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutAssinatura.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        layoutAssinatura.Controls.Add(lblCertificadoAssinatura, 0, 0);
        layoutAssinatura.Controls.Add(txtCertificadoAssinatura, 1, 0);
        layoutAssinatura.Controls.Add(btnSelecionarCertificadoAssinatura, 2, 0);
        layoutAssinatura.Controls.Add(btnAssinar, 2, 1);
        layoutAssinatura.Controls.Add(lblXmlAssinado, 0, 2);
        layoutAssinatura.Controls.Add(txtXmlAssinado, 1, 2);
        layoutAssinatura.Controls.Add(btnVisualizarXmlAssinado, 2, 2);
        layoutAssinatura.AutoSize = true;
        layoutAssinatura.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        layoutAssinatura.Dock = DockStyle.Top;
        layoutAssinatura.RowCount = 3;
        layoutAssinatura.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layoutAssinatura.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layoutAssinatura.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        lblCertificadoAssinatura.Anchor = AnchorStyles.Left;
        lblCertificadoAssinatura.AutoSize = true;
        lblCertificadoAssinatura.Text = "Certificado pessoal";

        txtCertificadoAssinatura.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtCertificadoAssinatura.ReadOnly = true;

        btnSelecionarCertificadoAssinatura.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnSelecionarCertificadoAssinatura.Text = "Selecionar certificado";
        btnSelecionarCertificadoAssinatura.Click += BtnSelecionarCertificadoAssinatura_Click;

        lblXmlAssinado.Anchor = AnchorStyles.Left;
        lblXmlAssinado.AutoSize = true;
        lblXmlAssinado.Text = "Saída assinada";

        txtXmlAssinado.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtXmlAssinado.ReadOnly = true;

        btnVisualizarXmlAssinado.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnVisualizarXmlAssinado.Text = "Visualizar arquivo";
        btnVisualizarXmlAssinado.Click += BtnVisualizarXmlAssinado_Click;

        btnAssinar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnAssinar.Text = "1. Assinar XML";
        btnAssinar.Click += BtnAssinar_Click;

        grpCriptografia.Controls.Add(layoutCriptografia);
        grpCriptografia.Dock = DockStyle.Top;
        grpCriptografia.AutoSize = true;
        grpCriptografia.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpCriptografia.Margin = new Padding(0, 0, 0, 8);
        grpCriptografia.Padding = new Padding(12);
        grpCriptografia.Text = "Criptografia";

        layoutCriptografia.ColumnCount = 3;
        layoutCriptografia.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        layoutCriptografia.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutCriptografia.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        layoutCriptografia.Controls.Add(lblCertificadoCriptografia, 0, 0);
        layoutCriptografia.Controls.Add(txtCertificadoCriptografia, 1, 0);
        layoutCriptografia.Controls.Add(btnSelecionarCertificadoCriptografia, 2, 0);
        layoutCriptografia.Controls.Add(btnCriptografar, 2, 1);
        layoutCriptografia.Controls.Add(lblXmlCriptografado, 0, 2);
        layoutCriptografia.Controls.Add(txtXmlCriptografado, 1, 2);
        layoutCriptografia.Controls.Add(btnVisualizarXmlCriptografado, 2, 2);
        layoutCriptografia.AutoSize = true;
        layoutCriptografia.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        layoutCriptografia.Dock = DockStyle.Top;
        layoutCriptografia.RowCount = 3;
        layoutCriptografia.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layoutCriptografia.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layoutCriptografia.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        lblCertificadoCriptografia.Anchor = AnchorStyles.Left;
        lblCertificadoCriptografia.AutoSize = true;
        lblCertificadoCriptografia.Text = "Certificado e-Financeira";

        txtCertificadoCriptografia.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtCertificadoCriptografia.ReadOnly = true;

        btnSelecionarCertificadoCriptografia.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnSelecionarCertificadoCriptografia.Text = "Selecionar .cer";
        btnSelecionarCertificadoCriptografia.Click += BtnSelecionarCertificadoCriptografia_Click;

        lblXmlCriptografado.Anchor = AnchorStyles.Left;
        lblXmlCriptografado.AutoSize = true;
        lblXmlCriptografado.Text = "Saída criptografada";

        txtXmlCriptografado.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtXmlCriptografado.ReadOnly = true;

        btnVisualizarXmlCriptografado.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnVisualizarXmlCriptografado.Text = "Visualizar arquivo";
        btnVisualizarXmlCriptografado.Click += BtnVisualizarXmlCriptografado_Click;

        btnCriptografar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        btnCriptografar.Text = "2. Criptografar XML";
        btnCriptografar.Click += BtnCriptografar_Click;

        layoutLogSection.ColumnCount = 1;
        layoutLogSection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutLogSection.Controls.Add(layoutLogHeader, 0, 0);
        layoutLogSection.Controls.Add(txtStatus, 0, 1);
        layoutLogSection.Dock = DockStyle.Fill;
        layoutLogSection.Margin = new Padding(0);
        layoutLogSection.Padding = new Padding(0);
        layoutLogSection.RowCount = 2;
        layoutLogSection.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layoutLogSection.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        layoutLogHeader.ColumnCount = 2;
        layoutLogHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutLogHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layoutLogHeader.Controls.Add(lblStatus, 0, 0);
        layoutLogHeader.Controls.Add(btnLimparLog, 1, 0);
        layoutLogHeader.AutoSize = true;
        layoutLogHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        layoutLogHeader.Dock = DockStyle.Top;
        layoutLogHeader.Margin = new Padding(0);
        layoutLogHeader.Padding = new Padding(0);
        layoutLogHeader.RowCount = 1;
        layoutLogHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        lblStatus.Anchor = AnchorStyles.Left;
        lblStatus.AutoSize = true;
        lblStatus.Margin = new Padding(0);
        lblStatus.Text = "Status";

        btnLimparLog.Anchor = AnchorStyles.Right;
        btnLimparLog.AutoSize = false;
        btnLimparLog.Margin = new Padding(0);
        btnLimparLog.Size = new Size(84, 28);
        btnLimparLog.Text = "Limpar log";
        btnLimparLog.Click += BtnLimparLog_Click;

        txtStatus.Dock = DockStyle.Fill;
        txtStatus.Margin = new Padding(0);
        txtStatus.Multiline = true;
        txtStatus.ReadOnly = true;
        txtStatus.ScrollBars = ScrollBars.Vertical;

        Controls.Add(layoutPrincipal);
        layoutPrincipal.ResumeLayout(false);
        layoutPrincipal.PerformLayout();
        grpArquivo.ResumeLayout(false);
        layoutArquivo.ResumeLayout(false);
        layoutArquivo.PerformLayout();
        grpAssinatura.ResumeLayout(false);
        layoutAssinatura.ResumeLayout(false);
        layoutAssinatura.PerformLayout();
        grpCriptografia.ResumeLayout(false);
        layoutCriptografia.ResumeLayout(false);
        layoutCriptografia.PerformLayout();
        layoutLogSection.ResumeLayout(false);
        layoutLogSection.PerformLayout();
        layoutLogHeader.ResumeLayout(false);
        layoutLogHeader.PerformLayout();
        ResumeLayout(false);
    }
}
