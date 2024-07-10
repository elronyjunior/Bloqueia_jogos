using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WfaAntiVini
{
    public partial class FrmSenha : Form
    {
        private readonly Timer timer = new Timer();

        // Evento para notificar a senha correta
        public event Action SenhaAcertada;
        string caminho = @"C:\Users\elron\AppData\Local\Roblox\Versions\version-7fe66aa864db490a\RobloxPlayerBeta.exe";
        public FrmSenha()
        {
            InitializeComponent();
        }

        private void btnValida_Click(object sender, EventArgs e)
        {
            string senhaCorreta = "3005"; // Substitua pela sua senha real
            string senhaDigitada = mskSenha.Text;

            if (senhaDigitada == senhaCorreta)
            {
                // Fechar o processo javaw
               Process.Start(caminho);

                // Disparar o evento SenhaAcertada
                SenhaAcertada?.Invoke();

                Close();
            }
            else
            {
                // Senha incorreta, mostrar mensagem ou realizar outra ação
                MessageBox.Show("Senha incorreta. Tente novamente.");
            }
        }

        private void FrmSenha_Load(object sender, EventArgs e)
        {
            // Configurar o temporizador para verificar a cada 5 segundos
            timer.Interval = 5000; // 5 segundos
            timer.Tick += VerificarAplicativoBloqueado;
            timer.Start();
        }

        private void VerificarAplicativoBloqueado(object sender, EventArgs e)
        {
            // Verificar se o processo javaw está sendo executado
            if (Process.GetProcessesByName("javaw").Length > 0)
            {
                // Fechar o processo javaw
                FecharProcesso("javaw");
            }
        }

        private void FecharProcesso(string nomeDoProcesso)
        {
            // Verificar se o processo está sendo executado
            if (Process.GetProcessesByName(nomeDoProcesso).Length > 0)
            {
                // Se estiver sendo executado, fechar o processo
                foreach (var processo in Process.GetProcessesByName(nomeDoProcesso))
                {
                    processo.Kill();
                }
            }
        }

        private void lblTexto_Click(object sender, EventArgs e)
        {

        }
    }
}
