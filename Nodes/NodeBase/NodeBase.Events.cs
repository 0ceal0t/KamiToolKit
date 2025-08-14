using Dalamud.Game.Addon.Events;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;

namespace KamiToolKit.Nodes;

public abstract unsafe partial class NodeBase {
    public Action? MouseOver { get; set; }
    private IAddonEventHandle? MouseOverHandle { get; set; }

    public Action? MouseOut { get; set; }
    private IAddonEventHandle? MouseOutHandle { get; set; }

    public Action? MouseClick { get; set; }
    private IAddonEventHandle? MouseClickHandle { get; set; }

    public SeString? Tooltip { get; set; }

    private IAddonEventManager? EventManager { get; set; }

    public virtual void EnableEvents( IAddonEventManager eventManager, AtkUnitBase* addon ) {
        if( MouseOver is not null || MouseOut is not null || MouseClick is not null || Tooltip is not null ) {
            AddFlags( NodeFlags.EmitsEvents | NodeFlags.HasCollision | NodeFlags.RespondToMouse );
        }

        EventManager = eventManager;
    }

    public virtual void DisableEvents( IAddonEventManager eventManager ) {
        RemoveFlags( NodeFlags.EmitsEvents | NodeFlags.HasCollision | NodeFlags.RespondToMouse );

        if( MouseOverHandle is not null ) {
            eventManager.RemoveEvent( MouseOverHandle );
            MouseOverHandle = null;
        }

        if( MouseOutHandle is not null ) {
            eventManager.RemoveEvent( MouseOutHandle );
            MouseOutHandle = null;
        }

        if( MouseClickHandle is not null ) {
            eventManager.RemoveEvent( MouseClickHandle );
            MouseClickHandle = null;
        }

        EventManager = eventManager;
    }

    public virtual void UpdateEvents( IAddonEventManager eventManager, AtkUnitBase* addon ) {
        DisableEvents( eventManager );
        EnableEvents( eventManager, addon );
    }
}