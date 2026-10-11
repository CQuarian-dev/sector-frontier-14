using Content.Lua.Client.AmbientSpaceEffects;
using Content.Lua.Shared.SectorDarkness;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Lua.Client.SectorDarkness;

public sealed class SectorDarknessSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlays = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    private const float FadeTime = 8f;

    private SectorDarknessOverlay _overlay = default!;
    private SectorDarknessBackdropOverlay _backdrop = default!;
    private int _sources;
    private float _strength;

    public override void Initialize()
    {
        base.Initialize();

        _overlay = new SectorDarknessOverlay(_prototype);
        _backdrop = new SectorDarknessBackdropOverlay();
        SubscribeLocalEvent<SectorDarknessComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<SectorDarknessComponent, ComponentShutdown>(OnShutdown);
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _strength = 0f;
        Apply();
    }

    private void OnStartup(Entity<SectorDarknessComponent> ent, ref ComponentStartup args)
    {
        _sources++;
    }

    private void OnShutdown(Entity<SectorDarknessComponent> ent, ref ComponentShutdown args)
    {
        _sources--;
    }

    public override void FrameUpdate(float frameTime)
    {
        if (_sources == 0 && _strength == 0f)
            return;

        var mapId = MapId.Nullspace;
        var dark = false;
        if (_player.LocalEntity is { } player && TryComp(player, out TransformComponent? xform))
        {
            mapId = xform.MapID;
            dark = HasComp<SectorDarknessComponent>(xform.MapUid);
        }

        if (dark)
        {
            _overlay.Map = mapId;
            _backdrop.Map = mapId;
        }

        var target = dark ? 1f : 0f;
        if (_strength == target)
            return;

        if (!dark && mapId != _overlay.Map)
            _strength = 0f;
        else if (dark)
            _strength = MathF.Min(1f, _strength + frameTime / FadeTime);
        else
            _strength = MathF.Max(0f, _strength - frameTime / FadeTime);

        Apply();
    }

    private void Apply()
    {
        _overlay.Strength = _strength;
        _backdrop.Strength = _strength;

        if (_overlays.TryGetOverlay<AmbientSpaceEffectOverlay>(out var nebula))
            nebula.Visibility = 1f - _strength;

        if (_strength <= 0f)
        {
            _overlays.RemoveOverlay(_overlay);
            _overlays.RemoveOverlay(_backdrop);
        }
        else if (!_overlays.HasOverlay<SectorDarknessOverlay>())
        {
            _overlays.AddOverlay(_overlay);
            _overlays.AddOverlay(_backdrop);
        }
    }
}
