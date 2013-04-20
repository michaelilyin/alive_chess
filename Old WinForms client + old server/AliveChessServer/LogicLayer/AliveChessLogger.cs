using System.Collections.Generic;
using System.IO;

namespace AliveChessServer.LogicLayer
{
    public delegate void LogHandler(string message);

    public class AliveChessLogger
    {
        private MainForm _form;
        private LogHandler _handler;
        private List<string> _logMessages;

        private object _logSync = new object();

        public AliveChessLogger(MainForm form)
        {
            this._form = form;
            _handler = new LogHandler(form.AddChatMessage);
            _logMessages = new List<string>();
        }

        public void Add(string message)
        {
            lock (_logSync)
                _logMessages.Add(message);
            _form.Invoke(_handler, message);
        }

        public void Clear(string filename)
        {
            File.Delete(filename);
        }

        public void Write(string filename, string str)
        {
            FileStream file = new FileStream(filename, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine(str);
            writer.Close();
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
