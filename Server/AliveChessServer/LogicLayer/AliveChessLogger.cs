using System;
using System.IO;
using System.Windows.Forms;
using AliveChessLibrary.Interfaces;

namespace AliveChessServer.LogicLayer
{
    public delegate void LogHandler(string message);

    public class AliveChessLogger : ILogger
    {
        private readonly MainForm _form;
        private readonly LogHandler _handler;
        private const string Extension = ".txt";

        private object _logSync = new object();

        public AliveChessLogger(MainForm form)
        {
#if DEBUG
            DebugConsole.WriteLine(this, "Created");
#endif
            if (form != null)
            {
                this._form = form;
                _handler = new LogHandler(form.AddChatMessage);
            }
        }

        public void Add(string message)
        {
            if (_form != null)
                _form.Invoke(_handler, message);
        }

        public void Clear(string filename)
        {
            File.Delete(filename);
        }

        public void Log(string filename, string str)
        {
            var file = new FileStream(
                string.Concat(filename, Extension),
                FileMode.Append, FileAccess.Write);

            var writer = new StreamWriter(file);
            writer.WriteLine(str);

            writer.Close();
            file.Close();
        }

        public void Log(string filename, params string[] str)
        {
            var file = new FileStream(
                string.Concat(filename, Extension),
                FileMode.Append, FileAccess.Write);

            var writer = new StreamWriter(file);
            for (int i = 0; i < str.Length; i++)
                writer.WriteLine(str[i]);

            writer.Close();
            file.Close();
        }

        public static void LogError(Exception exception)
        {
            FileStream stream = File.Open("ErrLog.txt", FileMode.Append);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("*********************Exception*********************");
            writer.WriteLine(string.Format("Message: {0}", exception.Message));
            writer.WriteLine(string.Format("Source: {0}", exception.Source));
            writer.WriteLine(string.Format("Stack Trace: {0}", exception.StackTrace));
            writer.WriteLine(string.Format("Target: {0}", exception.TargetSite.Name));
            writer.WriteLine("***************************************************");
            writer.Close();
            stream.Close();

            MessageBox.Show("Error has been occurred. Please send ErrorLog.txt to igor.kirichenko1@gmail.com");
        }

        public int Analize(string filename)
        {
            int count = 0;
            string str = null;
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            while ((str = reader.ReadLine()) != null)
            {
                if (str == "error has occured")
                    count++;
            }
            reader.Close();
            return count;
        }
    }
}
