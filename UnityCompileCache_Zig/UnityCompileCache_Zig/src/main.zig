const std = @import("std");
const builtin = @import("builtin");
const windows = std.os.windows;
const build_options = @import("build_options");

extern "kernel32" fn GetLongPathNameA(
    lpszShortPath: windows.LPCSTR,
    lpszLongPath: ?windows.LPSTR,
    cchBuffer: windows.DWORD,
) callconv(.winapi) windows.DWORD;

var gpa_instance = std.heap.GeneralPurposeAllocator(.{
    .thread_safe = true,
    .never_unmap = true,
    .retain_metadata = true,
    .stack_trace_frames = 16,
}){};

const WRAPPER_FILENAME = build_options.WRAPPER_FILENAME;

pub fn main() !u8 {
    const allocator: std.mem.Allocator = if (builtin.mode == .Debug or builtin.mode == .ReleaseSafe)
        gpa_instance.allocator()
    else
        std.heap.page_allocator;
    defer deinit();

    const fullArgs = try std.process.argsAlloc(allocator);
    defer std.process.argsFree(allocator, fullArgs);

    if (HasCompileArgs(fullArgs)) {
        return try Call_Cache(allocator, fullArgs);
    }

    return try Call_Normal(allocator, fullArgs);
}

fn deinit() void {
    if (builtin.mode == .Debug or builtin.mode == .ReleaseSafe) {
        const leaked = gpa_instance.deinit();
        if (leaked == .leak) {
            std.debug.print("\nMemory leak detected!\n", .{});
        }
    }
}

fn Call_Normal(allocator: std.mem.Allocator, fullArgs: [][:0]u8) !u8 {
    const exeArgs0 = try ExeIf(allocator, fullArgs[0]);
    defer allocator.free(exeArgs0);

    const realFileFullPath = try std.fmt.allocPrintSentinel(allocator, "{s}.backup", .{exeArgs0}, 0);
    defer allocator.free(realFileFullPath);

    const callerFileFullPath = exeArgs0;
    const lpCommandLine = try BuildCommandLine(allocator, callerFileFullPath, "", fullArgs[1..]);
    defer allocator.free(lpCommandLine);

    const lpApplicationName = realFileFullPath;

    const lpApplicationNameW = try std.unicode.utf8ToUtf16LeAllocZ(allocator, lpApplicationName);
    defer allocator.free(lpApplicationNameW);
    const lpCommandLineW = try std.unicode.utf8ToUtf16LeAllocZ(allocator, lpCommandLine);
    defer allocator.free(lpCommandLineW);

    var si: windows.STARTUPINFOW = std.mem.zeroes(windows.STARTUPINFOW);
    si.cb = @sizeOf(windows.STARTUPINFOW);

    var pi: windows.PROCESS_INFORMATION = undefined;
    const flags = windows.CreateProcessFlags{
        .create_no_window = true,
        .high_priority_class = true,
    };
    const isOk = windows.kernel32.CreateProcessW(
        lpApplicationNameW,
        lpCommandLineW,
        null,
        null,
        0,
        flags,
        null,
        null,
        &si,
        &pi,
    );

    if (isOk == 0) {
        const err = std.os.windows.GetLastError();
        std.debug.print("Call_Normal================================================\n", .{});
        std.debug.print("GetLastError: {}\n", .{err});
        std.debug.print("exeArgs0: {s}\n", .{exeArgs0});
        std.debug.print("realFileFullPath: {s}\n", .{realFileFullPath});
        std.debug.print("lpCommandLine: {s}\n", .{lpCommandLine});

        return 1;
    }
    _ = windows.kernel32.WaitForSingleObject(pi.hProcess, windows.INFINITE);
    var exit_code: u32 = undefined;
    _ = windows.kernel32.GetExitCodeProcess(pi.hProcess, &exit_code);
    _ = windows.CloseHandle(pi.hProcess);
    _ = windows.CloseHandle(pi.hThread);

    return @intCast(exit_code);
}

fn Call_Cache(allocator: std.mem.Allocator, fullArgs: [][:0]u8) !u8 {
    const exeArgs0 = try ExeIf(allocator, fullArgs[0]);
    defer allocator.free(exeArgs0);

    const realFileFullPath = try std.fmt.allocPrint(allocator, "{s}.backup", .{exeArgs0});
    defer allocator.free(realFileFullPath);

    const lpCommandLine = try BuildCommandLine(allocator, WRAPPER_FILENAME, realFileFullPath, fullArgs[1..]);
    defer allocator.free(lpCommandLine);

    var si: windows.STARTUPINFOW = std.mem.zeroes(windows.STARTUPINFOW);
    si.cb = @sizeOf(windows.STARTUPINFOW);

    var pi: windows.PROCESS_INFORMATION = undefined;
    const flags = windows.CreateProcessFlags{
        .create_no_window = true,
        .high_priority_class = true,
    };

    const lpCommandLineW = try std.unicode.utf8ToUtf16LeAllocZ(allocator, lpCommandLine);
    defer allocator.free(lpCommandLineW);

    const isOk = windows.kernel32.CreateProcessW(
        null,
        lpCommandLineW,
        null,
        null,
        0,
        flags,
        null,
        null,
        &si,
        &pi,
    );

    if (isOk == 0) {
        const err = std.os.windows.GetLastError();
        std.debug.print("Call_Cache================================================\n", .{});
        std.debug.print("GetLastError: {}\n", .{err});
        std.debug.print("exeArgs0: {s}\n", .{exeArgs0});
        std.debug.print("realFileFullPath: {s}\n", .{realFileFullPath});
        std.debug.print("lpCommandLine: {s}\n", .{lpCommandLine});
        return 1;
    }

    _ = windows.kernel32.WaitForSingleObject(pi.hProcess, windows.INFINITE);
    var exit_code: u32 = undefined;
    _ = windows.kernel32.GetExitCodeProcess(pi.hProcess, &exit_code);
    _ = windows.CloseHandle(pi.hProcess);
    _ = windows.CloseHandle(pi.hThread);

    return @intCast(exit_code);
}

fn Contains(haystack: []const u8, needle: u8) bool {
    return std.mem.indexOfScalar(u8, haystack, needle) != null;
}

fn HasCompileArgs(args: [][:0]u8) bool {
    for (args) |arg| {
        if (std.mem.eql(u8, arg, "-c")) {
            return true;
        }
    }
    return false;
}

fn ExeIf(allocator: std.mem.Allocator, cmd: [:0]const u8) ![]const u8 {
    var buffer: [512]u8 = undefined;
    const longPath = try ShortToLongPath(&buffer, cmd);

    if (std.ascii.endsWithIgnoreCase(longPath, ".exe")) {
        return allocator.dupe(u8, longPath);
    }

    return std.fmt.allocPrint(allocator, "{s}.exe", .{longPath});
}

fn ShortToLongPath(buffer: []u8, shortPath: [:0]const u8) ![]u8 {
    if (!Contains(shortPath, '~')) {
        @memcpy(buffer[0..shortPath.len], shortPath[0..shortPath.len]);
        return buffer[0..shortPath.len];
    }

    const needed = GetLongPathNameA(shortPath.ptr, null, 0);
    if (needed == 0) {
        @memcpy(buffer[0..shortPath.len], shortPath[0..shortPath.len]);
        return buffer[0..shortPath.len];
    }

    const written = GetLongPathNameA(shortPath.ptr, @ptrCast(buffer.ptr), @intCast(buffer.len));
    if (written == 0 or written >= needed) {
        @memcpy(buffer[0..shortPath.len], shortPath[0..shortPath.len]);
        return buffer[0..shortPath.len];
    }

    return buffer[0..written];
}

fn QuoteIf(buf: []u8, arg: []const u8) ![]u8 {
    if (std.ascii.startsWithIgnoreCase(arg, "C:/Program Files") or
        std.ascii.startsWithIgnoreCase(arg, "C:\\Program Files"))
    {
        const need = arg.len + 2;
        if (buf.len < need) return error.BufferTooSmall;

        buf[0] = '"';
        @memcpy(buf[1 .. 1 + arg.len], arg);
        buf[arg.len + 1] = '"';
        return buf[0 .. arg.len + 2];
    }

    const prefixes = [_][]const u8{
        "-I",
        "-Wl,--version-script=",
        "--sysroot=",
    };

    for (prefixes) |p| {
        if (!std.mem.startsWith(u8, arg, p)) {
            continue;
        }

        const path = arg[p.len..];
        const isQuoted = path.len >= 2 and path[0] == '"' and path[path.len - 1] == '"';
        if (isQuoted) {
            if (buf.len < arg.len) return error.BufferTooSmall;
            @memcpy(buf[0..arg.len], arg);
            return buf[0..arg.len];
        }

        const need = p.len + path.len + 2;
        if (buf.len < need) return error.BufferTooSmall;

        @memcpy(buf[0..p.len], p);
        buf[p.len] = '"';
        @memcpy(buf[p.len + 1 .. p.len + 1 + path.len], path);
        buf[p.len + path.len + 1] = '"';
        return buf[0..need];
    }

    if (buf.len < arg.len) return error.BufferTooSmall;
    @memcpy(buf[0..arg.len], arg);
    return buf[0..arg.len];
}

fn BuildCommandLine(allocator: std.mem.Allocator, cmd0: []const u8, cmd1: []const u8, args: []const [:0]const u8) ![:0]u8 {
    var sb = try std.ArrayListUnmanaged(u8).initCapacity(allocator, 1024);
    defer sb.deinit(allocator);

    if (Contains(cmd0, ' ')) {
        try sb.append(allocator, '"');
        try sb.appendSlice(allocator, cmd0);
        try sb.append(allocator, '"');
    } else {
        try sb.appendSlice(allocator, cmd0);
    }

    try sb.append(allocator, ' ');

    if (Contains(cmd1, ' ')) {
        try sb.append(allocator, '"');
        try sb.appendSlice(allocator, cmd1);
        try sb.append(allocator, '"');
    } else {
        try sb.appendSlice(allocator, cmd1);
    }

    var buffer: [512]u8 = undefined;

    for (args) |arg| {
        try sb.append(allocator, ' ');

        if (Contains(arg, ' ')) {
            const quoted = try QuoteIf(&buffer, arg);
            try sb.appendSlice(allocator, quoted);
        } else {
            try sb.appendSlice(allocator, arg);
        }
    }

    const ret = try sb.toOwnedSliceSentinel(allocator, 0);
    return ret;
}

test "ShortToLongPath" {
    var buffer: [512]u8 = undefined;
    const shortPath = "C:\\PROGRA~1\\Unity\\Hub\\Editor\\60000~1.60F\\Editor\\Data\\PLAYBA~1\\ANDROI~1\\NDK\\TOOLCH~1\\llvm\\prebuilt\\WINDOW~1\\bin\\clang.exe";
    const longPath = try ShortToLongPath(&buffer, shortPath);

    try std.testing.expectEqualStrings("C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.60f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\NDK\\toolchains\\llvm\\prebuilt\\windows-x86_64\\bin\\clang.exe", longPath);
}

test "QuoteIf" {
    var buffer: [512]u8 = undefined;

    const quoted_1 = try QuoteIf(&buffer, "-IC:/Program Files/Unity/Hub/Editor/6000.0.62f1/Editor/Data/il2cpp/external/baselib/Include");
    try std.testing.expectEqualStrings("-I\"C:/Program Files/Unity/Hub/Editor/6000.0.62f1/Editor/Data/il2cpp/external/baselib/Include\"", quoted_1);

    const quoted_2 = try QuoteIf(&buffer, "-Wl,--version-script=C:/Program Files/Unity/Hub/Editor/6000.0.62f1/Editor/Data/PlaybackEngines/AndroidPlayer/Variations/il2cpp/Common/StaticLibs/libunity.map");
    try std.testing.expectEqualStrings("-Wl,--version-script=\"C:/Program Files/Unity/Hub/Editor/6000.0.62f1/Editor/Data/PlaybackEngines/AndroidPlayer/Variations/il2cpp/Common/StaticLibs/libunity.map\"", quoted_2);

    const quoted_3 = try QuoteIf(&buffer, "--sysroot=C:/Program Files/Unity/Hub/Editor/6000.0.60f1/Editor/Data/PlaybackEngines/AndroidPlayer/NDK/toolchains/llvm/prebuilt/windows-x86_64/sysroot");
    try std.testing.expectEqualStrings("--sysroot=\"C:/Program Files/Unity/Hub/Editor/6000.0.60f1/Editor/Data/PlaybackEngines/AndroidPlayer/NDK/toolchains/llvm/prebuilt/windows-x86_64/sysroot\"", quoted_3);

    const quoted_4 = try QuoteIf(&buffer, "C:\\Program Files\\Unity");
    try std.testing.expectEqualStrings("\"C:\\Program Files\\Unity\"", quoted_4);
}
