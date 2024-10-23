using System.Diagnostics;
using System.Web;

var videoPath = args[0].Replace("mpcbe://", "");
var splitedVideoPath = videoPath.Split(':');
var isPotPlayer = splitedVideoPath.Last() == "PotPlayer";
var playerName = splitedVideoPath.Last();
videoPath = splitedVideoPath[0] + ":" + splitedVideoPath[1];
videoPath = videoPath.Replace("https//", "https://");

var process = $"\"C:\\Program Files\\MPC-BE x64\\mpc-be64.exe\" \"{videoPath}\"";
switch (playerName) {
    case "":
    case "MPCBE":
        process = $"\"C:\\Program Files\\MPC-BE x64\\mpc-be64.exe\" \"{videoPath}\"";
        break;
    case "PotPlayer":
        process = $"\"C:\\Program Files\\DAUM\\PotPlayer\\PotPlayerMini64.exe\" \"{videoPath}\"";
        break;
    case "MPV":
        process = $"\"E:\\MPV\\mpvnet.exe\" \"{videoPath}\"";
        break;
}

var subtitle = GetSubtitle(videoPath);
if (subtitle != null) {
    if (playerName != "MPV") process += $" /sub \"{subtitle}\"";
}

var proc = new Process();
proc.StartInfo = new ProcessStartInfo(process);
proc.Start();

//Process.Start(process);

if (playerName == "MPV" && subtitle != null) {
    proc.WaitForInputIdle();

    var procSubRemove = new Process();
    procSubRemove.StartInfo = new ProcessStartInfo($"\"E:\\MPV\\mpvnet.exe\" --command=\"sub-remove\"");
    procSubRemove.Start();
    procSubRemove.WaitForInputIdle();

    var procSubAdd = new Process();
    procSubAdd.StartInfo = new ProcessStartInfo($"\"E:\\MPV\\mpvnet.exe\" --command=\"sub-add \\\"{subtitle.Replace("\\", "\\\\")}\\\"\"");
    procSubAdd.Start();

    //Process.Start($"\"E:\\MPV\\mpvnet.exe\" --command=\"sub-remove\"");
    //Process.Start($"\"E:\\MPV\\mpvnet.exe\" --command=\"sub-add \\\"{subtitle.Replace("\\", "\\\\")}\\\"\"");
}

static string GetSubtitle(string path) {
    path = HttpUtility.UrlDecode(path);
    var fileName = Path.GetFileNameWithoutExtension(path);
    string downloadPath = Path.Combine(System.Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
    var allFiles = Directory.GetFileSystemEntries(downloadPath).ToList();
    downloadPath += "\\";
    string result = null;
    string[] extension = new string[] { ".ass", ".srt", ".ssa" };
    for (int i = 0; i < allFiles.Count(); i++) {
        if (!extension.Contains(Path.GetExtension(allFiles[i]))) continue;
        allFiles[i] = allFiles[i].Replace(downloadPath, String.Empty);
        if (allFiles[i].StartsWith(fileName)) {
            result = Path.Combine(downloadPath, allFiles[i]);
            break;
        }
    }
    return result;
}