using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace WfaAntiVini
{
    static class Program
    {
        private static readonly string mutexName = "SeuNomeUnicoParaOMutex";
        private static bool fecharThread = false;
        private static bool acertou = false;

        [STAThread]
        static void Main()
        {
            bool fecho = false;

            using (Mutex mutex = new Mutex(true, mutexName, out bool isMutexCreated))
            {
                if (isMutexCreated)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Console.WriteLine("Aplicativo principal iniciado.");

                    Thread thread = new Thread(() =>
                    {
                        Console.WriteLine("Thread iniciada.");

                        while (!fecharThread)
                        {
                            if (JavaEstaRodando() && !acertou)
                            {
                                Console.WriteLine("Java está em execução. Aguardando fechamento.");
                                while (JavaEstaRodando())
                                {
                                    FecharProcesso("RobloxPlayerBeta");
                                    Thread.Sleep(1000);
                                    fecho = true;
                                }
                            }
                            else if (fecho)
                            {
                                Console.WriteLine("Java não está em execução. Abrindo FrmSenha.");
                                // Java não está em execução, então abra o seu aplicativo
                                AbrirFormSenha();
                                fecho = false;
                            }

                            Thread.Sleep(1000);
                        }

                        Console.WriteLine("Thread encerrada.");
                    });

                    thread.IsBackground = true;
                    thread.Start();

                    Console.WriteLine("Aguardando thread.");

                    Application.Run();

                    fecharThread = true;

                    Console.WriteLine("Aplicativo principal encerrado.");
                }
                else
                {
                    Console.WriteLine("O aplicativo já está em execução.");
                    MessageBox.Show("O aplicativo já está em execução.");
                }
            }
        }

        private static bool JavaEstaRodando()
        {
            return Process.GetProcessesByName("RobloxPlayerBeta").Length > 0;
        }

        private static void AbrirFormSenha()
        {
            FrmSenha frmSenha = new FrmSenha();

            // Assinar o evento SenhaAcertada
            frmSenha.SenhaAcertada += () => { acertou = true; };

            Console.WriteLine("Abrindo FrmSenha.");
            Application.Run(frmSenha);
        }

        private static void FecharProcesso(string nomeDoProcesso)
        {
            if (Process.GetProcessesByName(nomeDoProcesso).Length > 0)
            {
                foreach (var processo in Process.GetProcessesByName(nomeDoProcesso))
                {
                    processo.Kill();
                }
            }
        }
    }
}
