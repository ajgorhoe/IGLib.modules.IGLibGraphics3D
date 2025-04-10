using System;

namespace IGLib.Gr3D
{

    /// <summary>Represents RGBA color with components between 0 and 1.</summary>
    public struct ColorRGBA
    {
        public float R, G, B, A;

        public ColorRGBA(float r, float g, float b, float a = 1.0f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static readonly ColorRGBA White = new ColorRGBA(1, 1, 1, 1);
        public static readonly ColorRGBA Black = new ColorRGBA(0, 0, 0, 1);

        public override string ToString()
        {
            return $"[{R}, {G}, {B}, {A}]";
        }
    }

}