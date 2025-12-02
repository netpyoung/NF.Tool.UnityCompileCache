# NF.Tool.UnityCompileCache

This utility has been developed to streamline the execution of cache commands, aiming to reduce the lengthy build times associated with IL2CPP compilation on Windows.

- [repo](https://github.com/netpyoung/NF.Tool.UnityCompileCache/)
- [doc](https://netpyoung.github.io/NF.Tool.UnityCompileCache)

## Check Release Page

- [releases](https://github.com/netpyoung/NF.Tool.UnityCompileCache/releases/)

## GUI

![UnityCompileCacheGUI_Winform](./res/UnityCompileCacheGUI_Winform.webp)

⚠️ This tool is designed based on C:\Program Files\Unity\Hub and requires administrative privileges to modify Clang files.

### Require to run (GUI)

- [.NET Desktop Runtime 10.0.x](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

## Ref

- <https://discussions.unity.com/t/optimizing-ios-and-macos-build-times-using-ccache/809570>
- <https://qiita.com/mao_/items/96795c1e734129430310>
- <https://qiita.com/tani-shi/items/e1493e63a02966ef1bac>
- <https://zenn.dev/pobo380/articles/1a5d838ee857e1#エディタ拡張を追加する>
- <https://qiita.com/Ruw/items/33fe8377559a6600cb89>


## Log


``` txt
Unity: Unity6000.0.60f1
  - Template: Universal 3D
  - Target: Android
CPU: AMD Ryzen 5700G
SSD: SHGP31-2000GM

First Build
Build completed with a result of 'Succeeded' in 385 seconds (384883 ms)
UnityEditor.EditorApplication:Internal_CallDelayFunctions ()

Clean Build
Build completed with a result of 'Succeeded' in 186 seconds (185795 ms)
UnityEditor.EditorApplication:Internal_CallDelayFunctions ()

Clean Build
Build completed with a result of 'Succeeded' in 138 seconds (137736 ms)
UnityEditor.EditorApplication:Internal_CallDelayFunctions ()

Clean Build + Apply ccache
Build completed with a result of 'Succeeded' in 75 seconds (75499 ms)
UnityEditor.EditorApplication:Internal_CallDelayFunctions ()

Clean Build + Apply ccache
Build completed with a result of 'Succeeded' in 74 seconds (74274 ms)
UnityEditor.EditorApplication:Internal_CallDelayFunctions ()
```