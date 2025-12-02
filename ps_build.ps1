## Build Zig

Push-Location UnityCompileCache_Zig/UnityCompileCache_Zig
zig build -Doptimize=ReleaseFast -Dwrapper=ccache
zig build -Doptimize=ReleaseFast -Dwrapper=sccache
Pop-Location # UnityCompileCache_Zig/UnityCompileCache_Zig

## Build Winform

Push-Location NF.Tool.UnityCompileCache
dotnet publish UnityCompileCacheGUI_Winform -p:PublishProfile=FolderProfile.pubxml
Pop-Location # NF.Tool.UnityCompileCache
