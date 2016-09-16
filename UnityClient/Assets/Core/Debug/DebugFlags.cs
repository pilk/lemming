
[System.Flags]
public enum DebugFlags
{
    Default = (1 << 0),
    RulesSystem = (1 << 1),
    DataCenter = (1 << 2),
    Level = (1 << 3),
    Gameplay = (1 << 4),
}