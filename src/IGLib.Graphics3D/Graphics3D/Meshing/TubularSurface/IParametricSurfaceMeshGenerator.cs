using IG.Num;
using System;

namespace IGLib.Gr3D
{
    public interface IParametricSurfaceMeshGenerator
    {

        // static abstract ParametricSurfaceMeshGenerator Global { get; }

        StructuredSurfaceMesh3D GenerateMesh(Func<double, double, vec3> surface, double tStart1, double tEnd1, double tStart2, double tEnd2, int numPoints1, int numPoints2, double hrel1 = 0.01, bool restrictToInterval = false);
        StructuredSurfaceMesh3D GenerateMesh(Func<double, double, vec3> surface, Func<double, double, vec3> tangent1, Func<double, double, vec3> tangent2, double tStart1, double tEnd1, double tStart2, double tEnd2, int numPoints1, int numPoints2);
    }
}