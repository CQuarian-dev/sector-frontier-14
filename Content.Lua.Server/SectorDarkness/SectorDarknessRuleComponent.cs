namespace Content.Lua.Server.SectorDarkness;

[RegisterComponent, Access(typeof(SectorDarknessRule))]
public sealed partial class SectorDarknessRuleComponent : Component
{
    [ViewVariables]
    public EntityUid? Map;
}
