using System.Numerics;
using Content.Client.Parallax;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Lua.Client.SectorDarkness;

public sealed class SectorDarknessOverlay : Overlay
{
    private static readonly ProtoId<ShaderPrototype> Shader = "SectorDarkness";

    private readonly ShaderInstance _shader;

    public float Strength;
    public MapId Map = MapId.Nullspace;

    public override OverlaySpace Space => OverlaySpace.WorldSpaceBelowWorld | OverlaySpace.WorldSpace;

    public SectorDarknessOverlay(IPrototypeManager prototype)
    {
        ZIndex = ParallaxSystem.ParallaxZIndex + 2;
        _shader = prototype.Index(Shader).InstanceUnique();
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        return Strength > 0f && args.MapId == Map;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var handle = args.WorldHandle;
        handle.SetTransform(Matrix3x2.Identity);

        if (args.Space == OverlaySpace.WorldSpaceBelowWorld)
        {
            handle.DrawRect(args.WorldAABB, Color.Black.WithAlpha(Strength));
            return;
        }

        _shader.SetParameter("strength", Strength);
        handle.UseShader(_shader);
        handle.DrawRect(args.WorldAABB, Color.White);
        handle.UseShader(null);
    }
}
