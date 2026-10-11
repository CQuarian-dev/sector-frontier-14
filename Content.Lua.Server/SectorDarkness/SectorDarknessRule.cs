using Content.Lua.Shared.SectorDarkness;
using Content.Server.StationEvents.Events;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Map;

namespace Content.Lua.Server.SectorDarkness;

public sealed class SectorDarknessRule : StationEventSystem<SectorDarknessRuleComponent>
{
    protected override void Started(EntityUid uid, SectorDarknessRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        var mapId = GetRelevantMapId();
        if (mapId == MapId.Nullspace || !MapSystem.MapExists(mapId))
            return;

        var map = MapSystem.GetMap(mapId);
        component.Map = map;
        EnsureComp<SectorDarknessComponent>(map);
    }

    protected override void Ended(EntityUid uid, SectorDarknessRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.Ended(uid, component, gameRule, args);

        if (component.Map is { } map)
            RemComp<SectorDarknessComponent>(map);
    }
}
