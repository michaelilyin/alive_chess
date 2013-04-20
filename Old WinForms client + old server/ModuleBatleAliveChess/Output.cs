using System;
using System.IO;

namespace ModuleBatleAliveChess
{
    public class Output
    {
        private const string logfile = "log.txt";
        private StreamWriter myLog;
        private bool myLogging;


        public bool Logging
        {
            get { return myLogging; }

            set
            {
                if (value)
                    Open();
                else
                    Close();
            }
        }


        public Output()
        {
            myLog = null;
            myLogging = false;
        }


        public void Open()
        {
            if (!myLogging)
            {
                myLogging = true;
                myLog = new StreamWriter(logfile, true);
                myLog.AutoFlush = true;

                myLog.WriteLine();
                myLog.WriteLine();
                myLog.WriteLine("----------------------------------------------------" + DateTime.Now.ToString());
            }
        }

        public void Close()
        {
            if (myLogging)
            {
                myLogging = false;
                myLog.Close();
                myLog = null;
            }
        }



        public void Command(string s)
        {
            if (myLogging)
            {
                myLog.WriteLine("I:" + s.Replace("\n", "\xD\xAi:"));
            }
        }


        public void Log(string s)
        {
            if (myLogging)
            {
                myLog.WriteLine("L:" + s.Replace("\n", "\xD\xAo:"));
            }
        }


        public void Out(string s)
        {
            if (myLogging)
            {
                myLog.WriteLine("O:" + s.Replace("\n", "\xD\xAo:"));
            }

            Console.WriteLine(s.Replace("\n", "\xD\xA"));
        }

    }
}
