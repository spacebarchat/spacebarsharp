using System;
using System.Linq;
using System.Text;
using FosscordSharp.Core;

namespace FosscordSharp.Utilities
{
    internal static class DependencyTreeTools
    {
        public static void SetClient(this FosscordObject str, FosscordClient cli)
        {
            str._client = cli;
        }

        public static void SetClientInTree(this FosscordObject obj, FosscordClient cli)
        {
            Type type = obj.GetType();
            var pl = type.GetProperties();
            foreach (var p in pl)
            {
                
                if(p.PropertyType.IsInstanceOfType(new FosscordObject()))
                {
                    Console.WriteLine("found property " + p.Name + ": " + p.PropertyType.Name);
                }
            }
        }
    }
}