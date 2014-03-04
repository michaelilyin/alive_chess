using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logger
{
    public enum LogLevel 
    {
        ALL = 0, 
        DEBUG = 1,
        ERRORS = 2,
        NOPE = 3
    }

    public static class Log
    {
        private static LogLevel level;
        private static String userProfilePath;
        private static String logsPath;
        private static String currentFile;
        private static TextWriter logFile;

        static Log()
        {
            level = LogLevel.ERRORS;
            userProfilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            userProfilePath = Path.Combine(userProfilePath, Properties.Properties.USER_DATA);
            if (!Directory.Exists(userProfilePath)) Directory.CreateDirectory(userProfilePath);
            logsPath = Path.Combine(userProfilePath, Properties.Properties.LOG);
            if (!Directory.Exists(logsPath)) Directory.CreateDirectory(logsPath);
            currentFile = Path.Combine(logsPath, DateTime.Now.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-') + ".txt");
            logFile = File.CreateText(currentFile);
        }

        public static void SetLevel(LogLevel level)
        {
            Log.level = level;
        }

        public static void Message(String message)
        {
            if (level <= LogLevel.ALL)
            {
                lock (logFile)
                {
                    logFile.WriteLine(String.Format("[MESSAGE {0}] {1}", DateTime.Now, message));
                    logFile.Flush();
                }
            }
        }

        public static void Debug(String message)
        {
            if (level <= LogLevel.DEBUG)
            {
                lock (logFile)
                {
                    logFile.WriteLine(String.Format("[DEBUG {0}] {1}", DateTime.Now, message));
                    logFile.Flush();
                }
            }
        }

        public static void Error(String message)
        {
            if (level <= LogLevel.ERRORS)
            {
                lock (logFile)
                {
                    logFile.WriteLine(String.Format("[ERROR {0}] {1}", DateTime.Now, message));
                    logFile.Flush();
                }
            }
        }

        public static void Error(Exception excepton, String message = "")
        {
            if (level <= LogLevel.ERRORS)
            {
                lock (logFile)
                {
                    logFile.WriteLine(String.Format("[EXCEPTION {0}] {1} \n    Message: {2} \n    Object: {4}\n    Stack trace: {3}", DateTime.Now, message, excepton.Message, excepton.StackTrace, excepton.Source));
                    logFile.Flush();
                }
            }
        }

        public static void Close()
        {
            lock (logFile)
                logFile.Close();
        }
    }
}
