﻿using IG.Num;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Parametric Klein bottle surface generated from the figure eight curve by moving the curve along
    /// the circle with a larger radius <see cref="aa"/> and at the same time rotating it around the circle 
    /// by linearly growing rotationn angle, which reaches the specified number of half-twists <see cref="ff"/> 
    /// (which must be odd to generate non-orientable surface of a Klein bottle) after the angle on the larger circle
    /// reaches the full angle (360 degrees or 2*π).
    /// <para>Parameters: radius <see cref="aa"/>, number of half-twists <see cref="ff"/> (the surface is
    /// non-orientable for odd values 1, 3, 5, ...; 1 for the usual Klein bottle with one half-twist)</para>
    /// <para>Basis for Implementation: <seealso href="https://virtualmathmuseum.org/docs/Moebius_Strip.pdf"/></para>
    /// <para>At wikipedia: <seealso cref="https://en.wikipedia.org/wiki/Klein_bottle#The_figure_8_immersion"/></para>
    /// <para>At MathCurve: <seealso cref=""/></para>
    /// </summary>
    /// <seealso cref="MoebiusStrip"/>
    public class KleinBottleFromFigureEight: IParametricSurfaceWithBounds
    {

        /// <summary>Constructor - Klein bottle generated from figure eight curve, with radius 
        /// <paramref name="aa"/> and number of half-twists <paramref name="ff"/>, which define 
        /// properties <see cref="aa"/> and <see cref="ff"/>.</summary>
        public KleinBottleFromFigureEight(double aa = aaDefault, double ff = ffDefault)
        {
            this.aa = aa;
            this.ff = ff;
        }

        
        /// <summary>Default value of <see cref="aa"/>.</summary>
        public const double aaDefault = 1.0;

        /// <summary>Default value of <see cref="ff"/>.</summary>
        public const double ffDefault = 1.0;


        /// <summary>Radius of the circle around which the generating line is moved.</summary>
        public double aa { get; init; }


        /// <summary>Half axis of the ellipsoid in the y direction. Default is <see cref="ffDefault"/>.</summary>
        public double ff { get; init; }


        /// <summary>Half axis of the ellipsoid in the z direction. Default is <see cref="cDefault"/>.</summary>
        public double c { get; init; }


        /// <inheritdoc/>
        public vec3 Surface(double u, double v)
        {
            return new vec3(
                aa * (Cos(v) + u * Cos(ff * v / 2.0) * Cos(v)),
                aa * (Sin(v) + u * Cos(ff * v / 2.0) * Sin(v)),
                aa * u * Sin(ff * v / 2.0));
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative1(double u, double v)
        {
            return new vec3(
                0,
                0,
                0);
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative2(double u, double v)
        {
            return new vec3(
                0,
                0,
                0 );
        }

        /// <inheritdoc/>
        public bool HasDerivative => false;

        /// <inheritdoc/>
        public double StartParameter1 { get; } = -PI;

        /// <inheritdoc/>
        public double EndParameter1 { get; } = PI;

        /// <inheritdoc/>
        public double StartParameter2 { get; init; } = - 0.5 * PI;


        /// <inheritdoc/>
        public double EndParameter2 { get; init; } = 0.5 * PI;

    }

}
