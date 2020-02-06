using System.Reflection;
using System;

namespace AzeLib
{
    class LogOnLoad
    {
        public static void OnLoad()
        {
            Debug.Log("    - version: " + Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}


