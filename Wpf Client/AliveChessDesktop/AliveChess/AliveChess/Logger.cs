using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AliveChessLibrary.Interfaces;

namespace AliveChess
{
    public class Logger : ILogger
    {
        private const string Extension = ".txt";

        public void Log(string filename, string str)
        {
            FileStream file = new FileStream(
                string.Concat(filename, Extension),
                FileMode.Append, FileAccess.Write);

            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine(str);

            writer.Close();
            file.Close();
        }

        public void Log(string filename, params string[] str)
        {
            throw new NotImplementedException();
        }
    }
}
