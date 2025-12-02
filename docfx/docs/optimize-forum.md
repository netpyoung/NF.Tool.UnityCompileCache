- https://discussions.unity.com/t/optimizing-ios-and-macos-build-times-using-ccache/809570/4

``` cs
using System.IO;
using UnityEditor;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEditor.Callbacks;
using UnityEngine;

public class PostProcessBuild
{
#if UNITY_IOS
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // Copy cache-clang script into the project as ccache-clang and ccache-clang++
        // Xcode seems to append ++ when linking, so we copy it with both names
        string ccacheSourcePath = $"{Application.dataPath}/../Scripts/ccache-clang";
        string ccacheSourceppPath = $"{Application.dataPath}/../Scripts/ccache-clang++";
        string ccacheDestPath = $"{pathToBuiltProject}/ccache-clang";
        string ccacheDestppPath = $"{pathToBuiltProject}/ccache-clang++";

        if (!File.Exists(ccacheDestPath))
            File.Copy(ccacheSourcePath, ccacheDestPath);
        if (!File.Exists(ccacheDestppPath))
            File.Copy(ccacheSourceppPath, ccacheDestppPath);

        // Load up the Xcode project
        PBXProject pbxProject = new PBXProject();
        string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        pbxProject.ReadFromFile(projectPath);

        // Set CC and CXX values to use ccache-clang script
        string frameworkGuid = pbxProject.GetUnityFrameworkTargetGuid();
        pbxProject.SetBuildProperty(frameworkGuid, "CC", "$(PROJECT_DIR)/ccache-clang");
        pbxProject.SetBuildProperty(frameworkGuid, "CXX", "$(PROJECT_DIR)/ccache-clang");

        File.WriteAllText(projectPath, pbxProject.WriteToString());
    }
#endif
}
```

``` sh
#!/bin/sh
if type -p /usr/local/bin/ccache >/dev/null 2>&1; then
  export CCACHE_MAXSIZE=10G
  export CCACHE_CPP2=true
  export CCACHE_HARDLINK=true
  export CCACHE_SLOPPINESS=file_macro,time_macros,include_file_mtime,include_file_ctime,file_stat_matches
  exec /usr/local/bin/ccache /usr/bin/clang++ "$@"
else
  exec clang "$@"
fi


#!/bin/sh
if type -p /usr/local/bin/ccache >/dev/null 2>&1; then
  export CCACHE_MAXSIZE=10G
  export CCACHE_CPP2=true
  export CCACHE_HARDLINK=true
  export CCACHE_SLOPPINESS=file_macro,time_macros,include_file_mtime,include_file_ctime,file_stat_matches
  exec /usr/local/bin/ccache /usr/bin/clang "$@"
else
  exec clang "$@"
fi
```


---

``` cs
// xcode
// // this may not necessary for incremental build
// Xcode → Preference → Locations → Derived Data → Change to “Relative”
// 
// 
// export CCACHE_SLOPPINESS=pch_defines,file_macro,time_macros,include_file_mtime,include_file_ctime

PostProcessBuild.cs
proj.SetBuildProperty(frameworkTarget, “CC”, “$(SRCROOT)/ccache-clang.sh”);
proj.SetBuildProperty(frameworkTarget, “CLANG_ENABLE_MODULES”, “NO”);
proj.SetBuildProperty(frameworkTarget, “GCC_PRECOMPILE_PREFIX_HEADER”, “NO”); // will make the first build much more slower，may not necessary for incremental build
```