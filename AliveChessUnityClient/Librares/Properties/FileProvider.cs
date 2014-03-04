using Properties.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Properties
{
    internal class FileProvider
    {
        public static Dictionary<String, String> ReadFile(String path)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            using (StreamReader reader = new StreamReader(path))
            {
                String buf;
                while ((buf = reader.ReadLine()) != null)
                {
                    if (buf.Length > 3)
                    {
                        String[] kv = buf.Split('=');
                        if (kv.Length != 2) throw new IllegalPropertiesFormatException();
                        result[kv[0]] = kv[1];
                    }
                }
            }
            return result;
        }

        public static void SaveFile(String path, Dictionary<String, String> data)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (var kv in data)
                {
                    writer.WriteLine(kv.Key + "=" + kv.Value);
                }
            }
        }
    }
}
