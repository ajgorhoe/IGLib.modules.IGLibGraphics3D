using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IG.Num;

namespace IGLib.Gr3D
{


    public class MaterialProperties
    {
        /// <summary>
        /// Name of the material.
        /// This is used to reference the material in an .obj file.
        /// </summary>
        public string Name { get; set; } = "DefaultMaterial";

        /// <summary>
        /// Ambient color (Ka) - Defines how the material reflects ambient light.
        /// Typical values range from (0,0,0) (black) to (1,1,1) (white).
        /// </summary>
        public vec3 AmbientColor { get; set; } = new vec3(0.2f, 0.2f, 0.2f);

        /// <summary>
        /// Diffuse color (Kd) - The main color of the material under direct lighting.
        /// </summary>
        public vec3 DiffuseColor { get; set; } = new vec3(0.8f, 0.8f, 0.8f);

        /// <summary>
        /// Specular color (Ks) - Defines the shininess of the material.
        /// A higher value makes the surface appear glossier.
        /// </summary>
        public vec3 SpecularColor { get; set; } = new vec3(1.0f, 1.0f, 1.0f);

        /// <summary>
        /// Specular exponent (Ns) - Controls the sharpness of specular highlights.
        /// Typical values range from 0 (matte) to 1000 (very shiny).
        /// </summary>
        public float Shininess { get; set; } = 50.0f;

        /// <summary>
        /// Transparency (d) - Ranges from 1.0 (fully opaque) to 0.0 (fully transparent).
        /// </summary>
        public float Transparency { get; set; } = 1.0f;

        /// <summary>
        /// Emission color (Ke) - Defines how much the material glows.
        /// </summary>
        public vec3 EmissionColor { get; set; } = new vec3(0.0f, 0.0f, 0.0f);

        /// <summary>
        /// Texture map file path for the diffuse color (map_Kd).
        /// This is an image file (e.g., JPG, PNG).
        /// </summary>
        public string DiffuseTexture { get; set; } = null;

        /// <summary>
        /// Bump map file path (map_Bump or bump).
        /// This is an image file used to simulate surface roughness.
        /// </summary>
        public string BumpMap { get; set; } = null;

        /// <summary>Just calls <see cref="ToStringMtl"/>.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToStringMtl();
        }

        /// <summary>Converts material properties to a .mtl file string.</summary>
        /// <param name="materialName">Name of the material when exported to a material file (.mtl).
        /// If not specified (null or empty string) then null is taken.</param>
        public virtual string ToStringMtl(string materialName = null)
        {
            StringWriter writer = new StringWriter();
            if (string.IsNullOrEmpty(materialName))
            {
                materialName = Name;
                if (string.IsNullOrEmpty(materialName))
                {
                    materialName = "UnknownMaterial";
                }
            }
            writer.WriteLine($"newmtl {Name}");
            writer.WriteLine($"Ka {AmbientColor.x} {AmbientColor.y} {AmbientColor.z}");
            writer.WriteLine($"Kd {DiffuseColor.x} {DiffuseColor.y} {DiffuseColor.z}");
            writer.WriteLine($"Ks {SpecularColor.x} {SpecularColor.y} {SpecularColor.z}");
            writer.WriteLine($"Ns {Shininess}");
            writer.WriteLine($"d {Transparency}");
            writer.WriteLine($"Ke {EmissionColor.x} {EmissionColor.y} {EmissionColor.z}");

            if (!string.IsNullOrEmpty(DiffuseTexture))
                writer.WriteLine($"map_Kd {DiffuseTexture}");

            if (!string.IsNullOrEmpty(BumpMap))
                writer.WriteLine($"map_Bump {BumpMap}");

            return writer.ToString();
        }

        /// <summary>
        /// Saves the material properties to a .mtl file.
        /// </summary>
        public void SaveToMaterialFile(string filePath, string materialName = null)
        {
            File.WriteAllText(filePath, ToStringMtl(materialName));
        }



    }




    public class MaterialPropertiesExamples
    {

        public void Example_SaveMaterialProperrties()
        {

            // ToDo: Update example for new material creation!
            MaterialProperties material = new MaterialProperties
            {
                Name = "BlueSurface",
                AmbientColor = new vec3(0.1f, 0.1f, 0.2f),
                DiffuseColor = new vec3(0.0f, 0.0f, 1.0f),
                SpecularColor = new vec3(1.0f, 1.0f, 1.0f),
                Shininess = 100.0f,
                Transparency = 1.0f,
                //OpticalDensity = 1.5f,
                //IlluminationModel = 2,
                DiffuseTexture = "blue_texture.png"
            };

            material.SaveToMaterialFile("blue_surface.mtl");

        }

    }


}
