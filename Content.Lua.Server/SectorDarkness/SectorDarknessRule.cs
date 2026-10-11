using Content.Lua.Server.Sectors;
using Content.Lua.Shared.SectorDarkness;
using Content.Server.StationEvents.Events;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Map;

namespace Content.Lua.Server.SectorDarkness;

public sealed class SectorDarknessRule : StationEventSystem<SectorDarknessRuleComponent>
{
    [Dependency] private readonly SectorSystem _sectors = default!;

    private MapId _relevantMapId = MapId.Nullspace;

    protected override MapId GetRelevantMapId()
    {
        return _relevantMapId;
    }

    protected override void Added(EntityUid uid, SectorDarknessRuleComponent component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        if (!_sectors.TryGetMapId(component.Sector, out var mapId))
            mapId = _sectors.ResolveHubMapId();

        component.MapId = mapId;
        _relevantMapId = mapId;
        base.Added(uid, component, gameRule, args);
    }

    protected override void Started(EntityUid uid, SectorDarknessRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        _relevantMapId = component.MapId;
        base.Started(uid, component, gameRule, args);

        if (component.MapId == MapId.Nullspace || !MapSystem.MapExists(component.MapId))
            return;

        var map = MapSystem.GetMap(component.MapId);
        component.Map = map;
        EnsureComp<SectorDarknessComponent>(map);
    }

    protected override void Ended(EntityUid uid, SectorDarknessRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        _relevantMapId = component.MapId;
        base.Ended(uid, component, gameRule, args);

        if (component.Map is { } map)
            RemComp<SectorDarknessComponent>(map);
    }
}
