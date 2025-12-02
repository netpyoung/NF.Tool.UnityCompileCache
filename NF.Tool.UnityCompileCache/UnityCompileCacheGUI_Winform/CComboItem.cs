namespace UnityCompileCacheGUI_Winform;

internal sealed class CComboItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required E_UNITYCOMPILECACHE_TYPE WrapperType { get; init; }

    public override string ToString()
    {
        return Name;
    }
}
