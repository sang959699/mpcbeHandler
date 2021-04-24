using System;
using System.Diagnostics;

namespace mpcbeHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            var param = args.Length == 0 ? String.Empty : args[0].Replace("mpcbe://", "");
            var process = $"\"C:\\Program Files\\MPC-BE x64\\mpc-be64.exe\" \"{param}\"";
            Process.Start(process);
        }
    }
}
