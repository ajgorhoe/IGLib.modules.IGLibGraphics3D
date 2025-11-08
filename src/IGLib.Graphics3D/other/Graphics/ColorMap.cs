
#nullable disable

using System;

namespace IGLib.Gr3D
{

    public enum ColorMapType
    {
        Rainbow,
        Heat,
        CoolWarm,
        Grayscale,
        Jet
    }

    /// <summary>
    /// Maps scalar values [0, 1] to RGBA colors using predefined color maps.
    /// </summary>
    public static class ColorMap
    {

        /// <summary>
        /// Gets a color for a normalized scalar input u ∈ [0, 1].
        /// </summary>
        public static ColorRGBA Map(double u, ColorMapType type)
        {
            u = Clamp(u, 0.0, 1.0);

            return type switch
            {
                ColorMapType.Rainbow => Rainbow(u),
                ColorMapType.Heat => Heat(u),
                ColorMapType.CoolWarm => CoolWarm(u),
                ColorMapType.Grayscale => Grayscale(u),
                ColorMapType.Jet => Jet(u),
                _ => new ColorRGBA(1, 1, 1, 1)
            };
        }

        private static ColorRGBA Rainbow(double u)
        {
            float r = 0, g = 0, b = 0;
            if (u < 0.5)
            {
                double v = u * 2;
                g = (float)v;
                b = (float)(1 - v);
            }
            else
            {
                double v = (u - 0.5) * 2;
                r = (float)v;
                g = (float)(1 - v);
            }
            return new ColorRGBA(r, g, b, 1);
        }

        public static ColorRGBA Heat(double u)
        {
            float r = (float)Math.Min(1.0, 2 * u);
            float g = (float)Math.Min(1.0, 2 * (u - 0.5));
            float b = 0;
            return new ColorRGBA(r, g, b, 1);
        }

        public static ColorRGBA CoolWarm(double u)
        {
            float r = (float)u;
            float g = (float)(1.0 - Math.Abs(u - 0.5) * 2);
            float b = (float)(1.0 - u);
            return new ColorRGBA(r, g, b, 1);
        }

        public static ColorRGBA Grayscale(double u)
        {
            float v = (float)u;
            return new ColorRGBA(v, v, v, 1);
        }

        public static ColorRGBA Jet(double u)
        {
            float r = (float)Clamp(1.5f - Math.Abs(4 * (float)u - 3), 0, 1);
            float g = (float)Clamp(1.5f - Math.Abs(4 * (float)u - 2), 0, 1);
            float b = (float)Clamp(1.5f - Math.Abs(4 * (float)u - 1), 0, 1);
            return new ColorRGBA(r, g, b, 1);
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value >= min && value <= max)
            {
                return value;
            }
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            throw new ArgumentException($"Clamp({value}, {min}, {max}) could not return anything. Error in implementation?");
        }

    }


}