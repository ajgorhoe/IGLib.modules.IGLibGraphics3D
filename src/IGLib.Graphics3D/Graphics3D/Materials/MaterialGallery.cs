using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IG.Num;

namespace IGLib.Gr3D
{

    public class MaterialGallery
    {

        /// <summary>Returns material properties for surfaces and for wireframe, with surface color
        /// LightSyBlue and wireframe color DarkRed.</summary>
        /// <returns></returns>
        public static (MaterialProperties SurfaceMaterial, MaterialProperties WireframeMaterial)
            GetMeshMaterialsLightSkyBlueWAndDarkRed()
        {
            return (
                SurfaceMaterial: new MaterialProperties
                {
                    Name = "SurfaceLightSkyBlue",
                    DiffuseColor = new vec3(135 / 255.0, 206 / 255.0, 250 / 255.0),
                    AmbientColor = new vec3(0.2, 0.2, 0.3),
                    SpecularColor = new vec3(1, 1, 1),
                    Shininess = 80,
                    Transparency = 1
                },
                WireframeMaterial: new MaterialProperties
                {
                    Name = "WireframeDarkRed",
                    DiffuseColor = new vec3(139 / 255.0, 0, 0),
                    AmbientColor = new vec3(0.1, 0.0, 0.0),
                    SpecularColor = new vec3(0.5, 0.5, 0.5),
                    Shininess = 10,
                    Transparency = 1
                } );
        }

    }

}
