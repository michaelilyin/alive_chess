using System;
using System.IO;
using System.Windows.Forms;
using AliveChessClient.GameLayer.Forms;

namespace AliveChessClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.Run(new ParentForm());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            FileStream stream = new FileStream("Error.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(e.Exception.Message);
            writer.WriteLine();
            writer.WriteLine(e.Exception.Source);
            writer.WriteLine();
            writer.WriteLine(e.Exception.StackTrace);
            writer.WriteLine();
            writer.WriteLine(e.Exception.TargetSite);
            writer.Close();
            stream.Close();
        }
    }
}