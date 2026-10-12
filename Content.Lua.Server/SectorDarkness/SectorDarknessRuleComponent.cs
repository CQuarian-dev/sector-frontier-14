using Robust.Shared.Audio;
using Robust.Shared.Map;

namespace Content.Lua.Server.SectorDarkness;

[RegisterComponent, Access(typeof(SectorDarknessRule))]
public sealed partial class SectorDarknessRuleComponent : Component
{
    [DataField]
    public string Sector = "AsteroidSectorDefault";

    [DataField]
    public SoundSpecifier? Music;

    [DataField]
    public float LightBreakChancePerSecond;

    [DataField]
    public float DoorToggleChancePerSecond;

    [ViewVariables]
    public MapId MapId = MapId.Nullspace;

    [ViewVariables]
    public EntityUid? Map;
}
