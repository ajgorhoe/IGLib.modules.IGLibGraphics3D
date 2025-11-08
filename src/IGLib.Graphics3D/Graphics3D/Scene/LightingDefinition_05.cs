
#nullable disable

using IGLib.Gr3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>Repredents different light types used in 3D rendering.</summary>
    public enum LightType_05
    {
        Ambient,
        Directional,
        Point,
        Spot,
        Area
    }

    /// <summary>Defines properties of a light source used in rendering.</summary>
    public class LightingDefinition_05
    {
        /// <summary> Type of the light source (Ambient, Directional, Point, Spot, Area). </summary>
        public LightType_05 Type { get; set; }

        /// <summary> Light color (RGB values between 0 and 1). </summary>
        public vec3 Color { get; set; }

        /// <summary> Intensity (brightness of the light). Default = 1.0 </summary>
        public double Intensity { get; set; } = 1.0;

        /// <summary> Position of the light (for Point, Spot, and Area lights). </summary>
        public vec3 Position { get; set; }

        /// <summary> Direction of the light (for Directional and Spot lights). </summary>
        public vec3 Direction { get; set; }

        /// <summary> Angle of the spotlight cone (only for Spot lights, in degrees). </summary>
        public double SpotAngle { get; set; }

        /// <summary> Size of the light source (only for Area lights). </summary>
        public vec3 AreaSize { get; set; }

        /// <summary> Creates a new light with default values (white light at intensity 1.0). </summary>
        public LightingDefinition_05(LightType_05 type)
        {
            Type = type;
            Color = new vec3(1.0, 1.0, 1.0); // Default to white light
            Intensity = 1.0;
            Position = new vec3(0, 0, 0);
            Direction = new vec3(0, 0, -1);
            SpotAngle = 45.0;
            AreaSize = new vec3(1, 1, 0);
        }

        /// <summary> Exports the light definition as a readable string (useful for debugging). </summary>
        public override string ToString()
        {
            return $"Light Type: {Type}, Color: {Color}, Intensity: {Intensity}, " +
                   $"Position: {Position}, Direction: {Direction}, SpotAngle: {SpotAngle}, AreaSize: {AreaSize}";
        }
    }


    public class LightingDefinitionExamples_05
    {

        public void Exapmle_CreateLightingDefinition()
        {

                var ambientLight = new LightingDefinition_05(LightType_05.Ambient)
                {
                    Color = new vec3(0.3, 0.3, 0.3), // Soft gray ambient light
                    Intensity = 0.5
                };

                var sunLight = new LightingDefinition_05(LightType_05.Directional)
                {
                    Color = new vec3(1, 1, 0.8), // Warm sunlight
                    Intensity = 1.5,
                    Direction = new vec3(-1, -1, -0.5) // Sunlight coming from an angle
                };

                var pointLight = new LightingDefinition_05(LightType_05.Point)
                {
                    Color = new vec3(1, 0.5, 0.5), // Reddish light
                    Intensity = 2.0,
                    Position = new vec3(2, 2, 2) // Positioned at (2,2,2)
                };

                Console.WriteLine(ambientLight);
                Console.WriteLine(sunLight);
                Console.WriteLine(pointLight);

            }

    }

}

