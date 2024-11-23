using KamiToolKit.Nodes.Parts;
using System.Numerics;

namespace KamiToolKit.Nodes;

/// <summary>
/// A simple image node that automatically creates a single <see cref="Part"/>, and exposes helpers to modify that part.
/// </summary>
/// <remarks>This node is not intended to be used with multiple <see cref="Part"/>'s.</remarks>
public class SimpleImageNode : ImageNode {
    public SimpleImageNode() {
        PartsList.Add( new Part() );
    }

    public float U {
        get => PartsList[0].U;
        set => PartsList[0].U = ( ushort )value;
    }

    public float V {
        get => PartsList[0].V;
        set => PartsList[0].V = ( ushort )value;
    }

    public Vector2 TextureCoordinates {
        get => new( U, V );
        set {
            U = value.X;
            V = value.Y;
        }
    }

    public float TextureHeight {
        get => PartsList[0].Height;
        set => PartsList[0].Height = ( ushort )value;
    }

    public float TextureWidth {
        get => PartsList[0].Width;
        set => PartsList[0].Width = ( ushort )value;
    }

    public Vector2 TextureSize {
        get => new( TextureWidth, TextureHeight );
        set {
            TextureWidth = value.X;
            TextureHeight = value.Y;
        }
    }

    public unsafe void LoadTexture( string path, bool hd )
        => InternalNode->LoadTexture(path, hd ? 2u : 1u );
}