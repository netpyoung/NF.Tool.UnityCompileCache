using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;
using static Vanara.PInvoke.Kernel32;

namespace NF.Tool.UnityCompileCache.UnityCompileCache_NativeAOT;

sealed class Program_UnityCompileCache_NativeAOT
{

#if USE_CCACHE
    const string CACHE_PROGRAM_FILENAME = "ccache.exe";
#elif USE_SCCACHE
    const string CACHE_PROGRAM_FILENAME = "sccache.exe";
#else
    const string CACHE_PROGRAM_FILENAME = "ccache.exe";
#endif

    static async Task<int> Main(string[] args)
    {
        bool isCompilation = args.Any(arg => arg == "-c");
        if (isCompilation)
        {
            return await CacllSccache(args);
        }
        else
        {
#pragma warning disable CA1416 // Validate platform compatibility
            return await CallClang(args);
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }

    [SupportedOSPlatform("windows")]
    static async Task<int> CallClang(string[] args)
    {
        string callerFileFullPath = Path.GetFullPath(Environment.GetCommandLineArgs().First());
        string callerDir = Path.GetDirectoryName(callerFileFullPath)!;
        string callerFileName = Path.GetFileNameWithoutExtension(callerFileFullPath)!;
        string realFileFullPath = Path.Combine(callerDir, $"{callerFileName}.exe.backup");

        string wrappedArgs = ArgumentsBuilder(args);
        string cmd0 = callerFileFullPath;
        if (cmd0.Contains(' '))
        {
            cmd0 = $"\"{cmd0}\"";
        }
        string commandLine = $"{cmd0} {wrappedArgs}";

#if DEBUG
        Console.WriteLine("AAAAAAAAAAAAAAAAAA=============================================");
        Console.WriteLine($"workingDir: {Directory.GetCurrentDirectory()}");
        Console.WriteLine($"callerDir: {callerDir}");
        Console.WriteLine($"lpApplicationName: {realFileFullPath}");
        Console.WriteLine($"lpCommandLine: {commandLine}");
        Console.WriteLine("AAAAAAAAAAAAAAAAAA=============================================");
#endif // DEBUG

        bool isOk = CreateProcess(
            lpApplicationName: realFileFullPath,
            lpCommandLine: new StringBuilder(commandLine),
            lpProcessAttributes: null,
            lpThreadAttributes: null,
            bInheritHandles: false,
            dwCreationFlags: CREATE_PROCESS.CREATE_UNICODE_ENVIRONMENT | CREATE_PROCESS.CREATE_NO_WINDOW | CREATE_PROCESS.CREATE_NEW_PROCESS_GROUP,
            lpEnvironment: null,
            lpCurrentDirectory: null,
            lpStartupInfo: STARTUPINFOEX.Default,
            lpProcessInformation: out SafePROCESS_INFORMATION pi
        );


        using (pi)
        {
            if (!isOk)
            {

                Win32Error err = GetLastError();
                Console.WriteLine(err.ToString());
                return 9000;
            }

            WAIT_STATUS result = WaitForSingleObject(pi.hProcess, INFINITE);
            if (result == WAIT_STATUS.WAIT_FAILED)
            {
                Win32Error err = GetLastError();
                Console.WriteLine(err.ToString());
                return 9000;
            }

            if (result != WAIT_STATUS.WAIT_OBJECT_0)
            {
                Win32Error err = GetLastError();
                Console.WriteLine(err.ToString());
                return 9000;
            }

            if (!GetExitCodeProcess(pi.hProcess, out uint lpExitCode))
            {
                Win32Error err = GetLastError();
                Console.WriteLine(err.ToString());
                return 9000;
            }

            return (int)lpExitCode;
        }
    }

    static async Task<int> CacllSccache(string[] args)
    {
        string callerFileFullPath = Path.GetFullPath(Environment.GetCommandLineArgs().First());
        string callerDir = Path.GetDirectoryName(callerFileFullPath)!;
        string callerFileName = Path.GetFileNameWithoutExtension(callerFileFullPath)!;
        string realFileFullPath = Path.Combine(callerDir, $"{callerFileName}.exe.backup");

        string wrappedArgs = ArgumentsBuilder(args);
        string cmd0 = realFileFullPath;
        if (cmd0.Contains(' '))
        {
            cmd0 = $"\"{cmd0}\"";
        }
        string commandLine = $"{cmd0} {wrappedArgs}";

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = CACHE_PROGRAM_FILENAME,
            Arguments = commandLine,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            WorkingDirectory = Directory.GetCurrentDirectory(),
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
        };

#if DEBUG
        Console.WriteLine("HHHHHHHHHHHHHHHHHHHHHH=======================================");
        Console.WriteLine($"WorkingDirectory: {psi.WorkingDirectory}");
        Console.WriteLine($"psi: {psi.FileName} {psi.Arguments}");
        Console.WriteLine($"Environment.CommandLine: {Environment.CommandLine}");
#endif //DEBUG

        Process p = Process.Start(psi)!;
        p.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                Console.Out.WriteLine(e.Data);
            }
        };
        p.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                Console.Error.WriteLine(e.Data);
            }
        };

        p.BeginOutputReadLine();
        p.BeginErrorReadLine();

        await p.WaitForExitAsync();
        int result = p.ExitCode;
        return result;
    }

    static string ArgumentsBuilder(string[] args)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string arg in args)
        {
            sb.Append(' ').Append(QuoteIf(arg));
        }
        return sb.ToString();
    }

    static string QuoteIf(string arg)
    {
        if (arg.StartsWith("C:/Program Files"))
        {
            return $"\"{arg}\"";
        }

        string[] prefixArr = new string[] { "-I", "-Wl,--version-script=", "--sysroot=" };
        foreach (string prefix in prefixArr)
        {
            if (!arg.StartsWith(prefix))
            {
                continue;
            }

            string path = arg.Substring(prefix.Length);
            if (path.StartsWith('\"') && path.EndsWith('\"'))
            {
                return arg;
            }

            return $"{prefix}\"{path}\"";
        }

        return arg;
    }
}
