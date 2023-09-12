using System;
using System.Reflection;

namespace FosscordSharp
{
    public class RuntimeInfo
    {
        public static Version LibVersion => Assembly.GetExecutingAssembly().GetName().Version;
    }
}