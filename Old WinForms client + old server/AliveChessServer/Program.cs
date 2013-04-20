using System;
using System.Windows.Forms;

namespace AliveChessServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Mutex mutex = null;
            //try
            //{
            //    mutex = Mutex.OpenExisting("ACMUTEX");
            //}
            //catch (WaitHandleCannotBeOpenedException ex)
            //{
            //    // Ok. Mutex doesn't exist, application can be started
            //}

            //if (mutex == null)
            //{
                //mutex = new Mutex(false, "ACMUTEX");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            //}
        }
    }
}
