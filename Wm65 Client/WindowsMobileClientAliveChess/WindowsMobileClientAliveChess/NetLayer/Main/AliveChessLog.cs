using System.IO;

namespace WindowsMobileClientAliveChess.NetLayer.Main
{
    public class AliveChessLog
    {
        private static object syncObject = new object();

        public static void Clear(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Create);
            file.Close();
        }

        public static void Write(string filename, string str)
        {
            FileStream file = new FileStream(filename, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine(str);
            writer.Close();
        }
    }
}
