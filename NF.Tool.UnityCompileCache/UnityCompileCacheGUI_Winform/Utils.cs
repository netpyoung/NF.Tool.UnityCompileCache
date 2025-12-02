using NuGet.Versioning;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;

namespace UnityCompileCacheGUI_Winform;

internal static class Utils
{
    public static string ExtractResourceToTempFilePath(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string tempFilePath = Path.GetTempFileName();
        using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName)!)
        {
            Debug.Assert(resourceStream != null);
            using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                resourceStream.CopyTo(fileStream);
            }
        }
        return tempFilePath;
    }

    public static bool IsRunningAsAdministrator()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    public static void BackupAndUpdate(string tempFullpath, string targetFullpath)
    {
        if (!File.Exists(targetFullpath))
        {
            return;
        }

        string currDir = Path.GetDirectoryName(targetFullpath)!;
        string currFileName = Path.GetFileName(targetFullpath);

        FileVersionInfo currFvi = FileVersionInfo.GetVersionInfo(targetFullpath);
        if (currFvi.ProductName == null || !currFvi.ProductName.StartsWith("UnityCompileCache_"))
        {
            string backup = $"{currFileName}.backup";
            string fullpath_backup = Path.Combine(currDir, backup);
            if (!File.Exists(fullpath_backup))
            {
                File.Copy(targetFullpath, fullpath_backup);
                Trace.WriteLine($"Backup - {currFileName} => {backup}");
            }
        }

        File.Copy(tempFullpath, targetFullpath, overwrite: true);
        Trace.WriteLine($"Updated - {currFileName}");
    }

    public static void Revert(string dir, string x)
    {
        string backup = $"{x}.backup";
        string fullpath_backup = Path.Combine(dir, backup);
        if (!File.Exists(fullpath_backup))
        {
            Debugger.Break();
            return;
        }

        string fullpath = Path.Combine(dir, x);
        File.Move(fullpath_backup, fullpath, overwrite: true);
        Trace.WriteLine($"revert - {backup} => {x}");
    }
}
