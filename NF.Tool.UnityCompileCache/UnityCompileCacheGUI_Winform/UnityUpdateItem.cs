namespace UnityCompileCacheGUI_Winform;

sealed class UnityUpdateItem
{
    public required string UnityVersion { get; init; }
    public required string UnityCompileCache_Mode { get; init; }
    public required string UnityCompileCache_Name { get; init; }
    public required string Dir { get; init; }
    public required bool HasUnityCompileCache { get; init; }
}
