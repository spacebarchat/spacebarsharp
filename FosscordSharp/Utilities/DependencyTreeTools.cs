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
            obj._client = cli;
            Type type = obj.GetType();
            var pl = type.GetProperties();
            foreach (var p in pl)
            {
                if(p.PropertyType.IsSubclassOf(typeof(FosscordObject)))
                {
                    Console.WriteLine("found property " + p.Name + ": " + p.PropertyType.Name);
                    ((FosscordObject)p.GetValue(obj)).SetClientInTree(cli);
                }
                else
                {
                    Console.WriteLine("not " + p.Name + ": " + p.PropertyType.Name);
                }
            }
        }
    }
}