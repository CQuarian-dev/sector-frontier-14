using Robust.Shared.Serialization;

namespace Content.Shared._Arcane.ErpPanel;

[Serializable, NetSerializable]
public enum ErpPanelKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class ErpPanelBuiState(NetEntity user, NetEntity target) : BoundUserInterfaceState
{
    public NetEntity User { get; } = user;
    public NetEntity Target { get; } = target;
}

[Serializable, NetSerializable]
public sealed partial class ErpPanelSendMessage(string interaction, bool privateMode) : BoundUserInterfaceMessage // Lua
{
    public readonly string Interaction = interaction;
    public readonly bool PrivateMode = privateMode; // Lua
}

// Lua start
[Serializable, NetSerializable]
public sealed class ErpPanelCustomEmoteMessage(ErpCustomEmote emote, bool privateMode) : BoundUserInterfaceMessage
{
    public readonly ErpCustomEmote Emote = emote;
    public readonly bool PrivateMode = privateMode;
}
// Lua end

[ByRefEvent]
public readonly record struct ErpPanelOpenEvent(EntityUid Target);
