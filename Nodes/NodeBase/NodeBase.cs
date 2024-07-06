using FFXIVClientStructs.FFXIV.Client.System.Memory;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using System;
using System.Collections.Generic;

namespace KamiToolKit.Nodes;

public abstract unsafe partial class NodeBase : IDisposable {
    protected static readonly List<IDisposable> CreatedNodes = [];

    private bool isDisposed;

    public abstract AtkResNode* InternalResNode { get; }

    /// <summary>
    /// Warning, this is only to ensure there are no memory leaks.
    /// Ensure you have detached nodes safely from native ui before disposing.
    /// </summary>
    internal static void DisposeAllNodes() {
        foreach( var node in CreatedNodes.ToArray() ) {
            node.Dispose();
        }
    }

    ~NodeBase() => Dispose( false );

    protected abstract void Dispose( bool disposing );

    public void Dispose() {
        if( !isDisposed ) {
            Dispose( true );
            GC.SuppressFinalize( this );

            CreatedNodes.Remove( this );
        }

        isDisposed = true;
    }
}

public abstract unsafe class NodeBase<T> : NodeBase where T : unmanaged, ICreatable {
    protected T* InternalNode { get; }

    public sealed override AtkResNode* InternalResNode => ( AtkResNode* )InternalNode;

    protected NodeBase( NodeType nodeType ) {
        InternalNode = NativeMemoryHelper.Create<T>();
        InternalResNode->Type = nodeType;

        if( InternalNode is null ) {
            throw new Exception( $"Unable to allocate memory for {typeof( T )}" );
        }

        CreatedNodes.Add( this );
    }

    protected override void Dispose( bool disposing ) {
        if( disposing ) {
            InternalResNode->Destroy( false );
            NativeMemoryHelper.UiFree( InternalNode );
        }
    }
}

