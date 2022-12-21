using System.Diagnostics;
using System.Web;

var videoPath = args[0].Replace("mpcbe://", "");
var splitedVideoPath = videoPath.Split(':');
var isPotPlayer = splitedVideoPath.Last() == "PotPlayer";
videoPath = splitedVideoPath[0] + ":" + splitedVideoPath[1];
var process = $"\"C:\\Program Files\\MPC-BE x64\\mpc-be64.exe\" \"{videoPath}\"";
if (isPotPlayer) process = $"\"C:\\Program Files\\DAUM\\PotPlayer\\PotPlayerMini64.exe\" \"{videoPath}\"";

var subtitle = GetSubtitle(videoPath);
if (subtitle != null) process += $" /sub \"{subtitle}\"";

Process.Start(process);

static string GetSubtitle(string path) {
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