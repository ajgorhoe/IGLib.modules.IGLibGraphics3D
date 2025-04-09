using IGLib.Gr3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using IG.Num;
using System.Text;

namespace IGLib.Gr3D
{


    /// <summary>Enum representing different light types used in 3D rendering.</summary>
    public enum LightType
    {
        Ambient,
        Directional,
        Point,
        Spot,
        Area
    }


    /// <summary>Defines properties of a light source used in rendering.</summary>
    public class LightingDefinition
    {

        /// <summary> Creates a new light with default values (white light at intensity 1.0). </summary>
        public LightingDefinition(LightType type)
        {
            Type = type;
        }

        /// <summary> Type of the light source (Ambient, Directional, Point, Spot, Area). </summary>
        public LightType Type { get; set; }

        /// <summary> Light color (RGB values between 0 and 1). </summary>
        public vec3 Color { get; set; } = new vec3(1, 1, 1);

        /// <summary> Intensity (brightness of the light). Default = 1.0 </summary>
        public double Intensity { get; set; } = 1.0;

        /// <summary> Position of the light (for Point, Spot, and Area lights). </summary>
        public vec3 Position { get; set; } = new vec3(0, 0, 0);

        /// <summary> Direction of the light (for Directional and Spot lights). </summary>
        public vec3 Direction { get; set; } = new vec3(0, 0, -1);

        /// <summary> Angle of the spotlight cone (only for Spot lights, in degrees). </summary>
        public double SpotAngle { get; set; } = 45;

        /// <summary> Size of the light source (only for Area lights). </summary>
        public vec3 AreaSize { get; set; } = new vec3(1, 1, 0);

        /// <summary>Generates a GLTF-compatible JSON snippet for the light.</summary>
        public string ToGltfJson(int index)
        {
            string type = Type switch
            {
                LightType.Directional => "directional",
                LightType.Point => "point",
                LightType.Spot => "spot",
                _ => "ambient" // fallback
            };

            string colorStr = $"[{Color.x}, {Color.y}, {Color.z}]";
            string lightJson = $@"
        {{
            ""name"": ""Light_{index}"",
            ""type"": ""{type}"",
            ""color"": {colorStr},
            ""intensity"": {Intensity}
        }}";

            return lightJson;
        }

        /// <summary> Exports the light definition as a readable string (useful for debugging). </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Light source:");
            sb.AppendLine($"  Type: {Type}");
            sb.AppendLine($"  Color: {Color}");
            sb.AppendLine($"  Intensity: {Intensity}");
            sb.AppendLine($"  Position: {Position}");
            sb.AppendLine($"  Direction: {Direction}");
            sb.AppendLine($"  SpotAngle: {SpotAngle}");
            sb.AppendLine($"  AreaSize: {AreaSize}");
            return sb.ToString();
        }
    }

    public class LightingDefinitionExamples
    {

        public void Exapmle_CreateLightingDefinition()
        {

                var ambientLight = new LightingDefinition(LightType.Ambient)
                {
                    Color = new vec3(0.3, 0.3, 0.3), // Soft gray ambient light
                    Intensity = 0.5
                };

                var sunLight = new LightingDefinition(LightType.Directional)
                {
                    Color = new vec3(1, 1, 0.8), // Warm sunlight
                    Intensity = 1.5,
                    Direction = new vec3(-1, -1, -0.5) // Sunlight coming from an angle
                };

                var pointLight = new LightingDefinition(LightType.Point)
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

