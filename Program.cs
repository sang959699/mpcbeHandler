using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace mpcbeHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            var videoPath = args.Length == 0 ? String.Empty : args[0].Replace("mpcbe://", "");
            var process = $"\"C:\\Program Files\\MPC-BE x64\\mpc-be64.exe\" \"{videoPath}\"";

            var subtitle = GetSubtitle(videoPath);
            if (subtitle != null) process += $" /sub \"{subtitle}\"";

            Process.Start(process);
        }

        private static string GetSubtitle(string path) {
            path = HttpUtility.UrlDecode(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            string downloadPath = Path.Combine(System.Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
            var allFiles = Directory.GetFileSystemEntries(downloadPath).ToList();
            downloadPath += "\\";
            string result = null;
            for (int i = 0; i < allFiles.Count(); i++) {
                allFiles[i] = allFiles[i].Replace(downloadPath, String.Empty);
                if (allFiles[i].StartsWith(fileName)) {
                    result = Path.Combine(downloadPath, allFiles[i]);
                    break;
                }
            }
            return result;
        }
    }
}
