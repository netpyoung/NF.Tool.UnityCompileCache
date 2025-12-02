using System;
using System.IO;
using System.IO.Enumeration;
using System.Linq;

namespace UnityCompileCacheGUI_Winform;

static class ClangFinder
{
    public static string? FindClangFromNDK(string rootDir)
    {
        return FindClang(rootDir, "NDK");
    }

    public static string? FindClangFromEmscripten(string rootDir)
    {
        return FindClang(rootDir, "Emscripten");
    }

    private static string? FindClang(string rootDir, string contain)
    {
        if (string.IsNullOrWhiteSpace(rootDir))
        {
            return null;
        }

        if (!Directory.Exists(rootDir))
        {
            return null;
        }

        EnumerationOptions options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            IgnoreInaccessible = true,
            AttributesToSkip = FileAttributes.Hidden | FileAttributes.System
        };

        // FileSystemEnumerable<string?> enumerable = new FileSystemEnumerable<string?>(ndkRoot, FindTransform, options);

        FileSystemEnumerable<string?> enumerable = new FileSystemEnumerable<string?>(
            rootDir,
            (ref FileSystemEntry entry) => FindTransform(ref entry, contain),
            options
        );

        return enumerable.FirstOrDefault(x => x != null);
    }

    private static string? FindTransform(ref FileSystemEntry entry, string contain)
    {
        if (entry.IsDirectory)
        {
            return null;
        }

        if (!entry.FileName.Equals("clang.exe", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (!entry.Directory.Contains(contain.AsSpan(), StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return entry.ToFullPath();
    }
}
