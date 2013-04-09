using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chat
{
    public static class Censorship
    {
        private static List<Regex> patterns=new List<Regex>();
        public static bool CheckMessage(string message)
        {
            for (int i = 0; i < patterns.Count; i++)
                if (patterns[i].IsMatch(message))
                    return false;
            return true;
        }
        public static void AddExp(string exp)
        {
            patterns.Add(new Regex(exp));
        }
        
    }
}
