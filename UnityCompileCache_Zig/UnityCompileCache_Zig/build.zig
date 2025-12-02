const std = @import("std");

pub fn build(b: *std.Build) void {
    const target = b.standardTargetOptions(.{});
    const optimize = b.standardOptimizeOption(.{});

    const root_module = b.createModule(.{
        .root_source_file = b.path("src/main.zig"),
        .target = target,
        .optimize = optimize,
    });

    const wrapper_filename_without_ext = b.option([]const u8, "wrapper", "wrapper filename (wihtout extension)") orelse "default";
    const build_exe_name = if (std.mem.eql(u8, wrapper_filename_without_ext, "default"))
        "UnityCompileCache_Zig"
    else
        std.fmt.allocPrint(b.allocator, "UnityCompileCache_{s}", .{wrapper_filename_without_ext}) catch unreachable;

    const exe = b.addExecutable(.{
        .name = build_exe_name,
        .root_module = root_module,
    });

    const wrapper_filename = std.fmt.allocPrint(b.allocator, "{s}.exe", .{wrapper_filename_without_ext}) catch unreachable;
    const options = b.addOptions();
    options.addOption([]const u8, "WRAPPER_FILENAME", wrapper_filename);
    exe.root_module.addOptions("build_options", options);

    const productName = build_exe_name;
    const productVersion = "0.0.1";
    const rc_content = std.fmt.allocPrint(b.allocator, @embedFile("resources/app.rc.template"), .{
        productName,
        productVersion,
    }) catch @panic("OOM");

    const rc_step = b.addWriteFiles();
    const rc_file = rc_step.add("app.rc", rc_content);
    exe.addWin32ResourceFile(.{ .file = rc_file });

    b.installArtifact(exe);

    const run_cmd = b.addRunArtifact(exe);
    run_cmd.step.dependOn(b.getInstallStep());
    if (b.args) |args| {
        run_cmd.addArgs(args);
    }

    const run_step = b.step("run", "Run the app");
    run_step.dependOn(&run_cmd.step);

    const exe_unit_tests = b.addTest(.{
        .root_module = root_module,
    });

    const run_exe_unit_tests = b.addRunArtifact(exe_unit_tests);
    const test_step = b.step("test", "Run unit tests");
    test_step.dependOn(&run_exe_unit_tests.step);
}
