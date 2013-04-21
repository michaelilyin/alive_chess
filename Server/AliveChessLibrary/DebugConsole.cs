﻿#if DEBUG
using System;
using System.Runtime.InteropServices;

namespace AliveChessLibrary
{
    public class DebugConsole
    {
            /// <summary>
            /// Allocates a new console for current process.
            /// </summary>
            [DllImport("kernel32.dll")]
            public static extern Boolean AllocConsole();

            /// <summary>
            /// Frees the console.
            /// </summary>
            [DllImport("kernel32.dll")]
            public static extern Boolean FreeConsole();

            public static void WriteLine(Object sender, string message)
            {
                //AllocConsole();
                Console.WriteLine("[" + sender.GetType().Name + "]: " + message);
            }
            public static void WriteLine(string sender, string message)
            {
                //AllocConsole();
                Console.WriteLine("[" + sender + "]: " + message);
            }
    }
}
#endif
