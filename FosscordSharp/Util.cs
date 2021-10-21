using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace FosscordSharp
{
    public class Util
    {
        public static void Log(string message,
            bool LogAlways = false,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)          
        {
            if(Debugger.IsAttached) Console.WriteLine("{0}:{1} {2}", Path.GetFileName(file), line, message);
            else if(LogAlways) Console.WriteLine(message);
        }
        public static void LogDebug(string message,
            bool LogAlways = false,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)          
        {
            if(Debugger.IsAttached) Debug.WriteLine("{0}:{1} {2}", Path.GetFileName(file), line, message);
            else if(LogAlways) Debug.WriteLine(message);
        }
        public static void LogDebugStdout(string message,
            bool LogAlways = false,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)          
        {
#if DEBUG
            if(Debugger.IsAttached) Console.WriteLine("{0}:{1} {2}", Path.GetFileName(file), line, message);
            else if(LogAlways) Console.WriteLine(message);
#endif
        }

    }
}