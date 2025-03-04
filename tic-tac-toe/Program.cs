using System.Text;

namespace tic_tac_toe
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!File.Exists("hostname.txt"))
            {
                File.Create("hostname.txt").Dispose();
                File.WriteAllText("hostname.txt", "host.docker.internal");
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginSignup());
        }
    }
}