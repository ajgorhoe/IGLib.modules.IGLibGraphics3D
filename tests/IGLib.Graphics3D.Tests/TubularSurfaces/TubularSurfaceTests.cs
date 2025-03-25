using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using IGLib.Tests.Base;
using System.Linq;
using System.IO;
using IGLib.Gr3D;
using IG.Num;


namespace IG.SandboxTests
{

    /// <summary>Testing of C# script execution with classes created for this.</summary>
    public class TubularSurfaceTests : TestBase<TubularSurfaceTests>
    {

        /// <summary>Calling base constructor initializes things like Output property to 
        /// write on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public TubularSurfaceTests(ITestOutputHelper output) : base(output)
        {
            
        }




        #region Examples



        #region BasicExamples


        /// <summary>Creates the first test of tubular surface generation and export.</summary>
        [Theory]
        [InlineData(100, 10, 0.1)]
        //[InlineData(400, 15, 0.1)]
        //[InlineData(1000, 20, 0.5)]
        protected void Example_CreateAndExportTubularSurfaceTest(int numLongitudinal, int numTransverse, double radius)
        {
            try
            {
                Output.WriteLine("Example / Semi-manual test:");
                Output.WriteLine("  Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software:");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string objFile = exportDirectory + $"00_TubularSurfaceFirstTest_{numLongitudinal}.obj";
                string mtlFile = exportDirectory + $"00_TubularSurfaceFirstTest_{numLongitudinal}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define a 3D helical curve:
                Func<double, vec3> helix = t => new vec3(Math.Cos(t), Math.Sin(t), t / 5.0);
                // Generate the tubular mesh
                try
                {
                    Directory.CreateDirectory(exportDirectory);
                }
                catch(Exception ex)
                {
                    Output.WriteLine($"\n\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}\n");
                }
                // ** Act:
                // Generate tubular mesh from curve definition:
                var mesh = TubularMeshGenerator.Generate(helix, 0.0, 6 * Math.PI, radius, numLongitudinal, numTransverse);
                // Export mesh and material to a file:
                mesh.ExportToObj(objFile, mtlFile);
                MeshExportExtensions.ExportMaterial(mtlFile, new vec3(0, 0, 1)); // Blue color
                sw.Stop();
                Console.WriteLine("\nSample mesh (Helix) exported successfully.");
                Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms.");
                // ** Assert:
                1.Should().Be(1); // just a dummy asserrtion
            }
            catch (Exception ex)
            {
                Output.WriteLine($"\n\n======\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}");
                throw;
            }
        }


        #endregion BasicExamples




        #region Examples:BasicCurves


        #endregion Examples:BasicCurves




        #region Examples:AdvancedCurves


        #endregion Examples:AdvancedCurves




        #region Examples:KnotGroups


        /// <summary>Creates the first test of tubular surface generation and export.</summary>
        [Theory]
        [InlineData(600 /* long */, 15 /* trans */, 0.05 /* r */, 3 /* n1 */, 2 /* n2 */, 7 /* n3 */,
            0.7 /* fi1 */, 0.2 /* fi2 */)]    // the three-twist knot
        [InlineData(600 /* long */, 15 /* trans */, 0.05 /* r */, 3 /* n1 */, 2 /* n2 */, 5 /* n3 */,
            1.5 /* fi1 */, 0.2 /* fi2 */)]    // the Stevedore knot
        [InlineData (600 /* long */, 15 /* trans */, 0.05 /* r */, 3 /* n1 */, 5 /* n2 */, 7 /* n3 */, 
            0.7 /* fi1 */, 1.0 /* fi2 */)]    // square knot
        [InlineData (600 /* long */, 15 /* trans */, 0.05 /* r */, 3 /* n1 */, 4 /* n2 */, 7 /* n3 */, 
            0.1 /* fi1 */, 0.7 /* fi2 */)]    // the 8_21 knot
        protected void Example_ExportLissajousKnot(int numLongitudinal, int numTransverse, double radius,
            int n1 = 3, int n2 = 4, int n3 = 7, double fi1 = 0, double fi2 = 0)
        {
            double fi3 = 0;
            try
            {
                Output.WriteLine($"Example / Semi-manual test: LISSAJOUS ({n1}, {n2}, {n3} / {fi1}, {fi2}, {fi3}) knot:");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string objFile = exportDirectory + $"01_LissajousKnot_{n1}-{n2}-{n3}_{fi1}-{fi2}-{fi3}_{numLongitudinal}.obj";
                string mtlFile = exportDirectory + $"01_LissajousKnot_n1.{n1}_n2.{n2}_n3.{n3}_fi1.{fi1}_fi2.{fi2}_{numLongitudinal}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the torus knot parameterization:
                var knot = new LissajousKnot3D(n1, n2, n3, fi1, fi2);
                // Func<double, vec3> helix = t => new vec3(Math.Cos(t), Math.Sin(t), t / 5.0);
                // Generate the tubular mesh
                try
                {
                    Directory.CreateDirectory(exportDirectory);
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"\n\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}\n");
                    throw;
                }
                // ** Act:
                // Generate tubular mesh from curve definition:
                var mesh = TubularMeshGenerator.Generate(knot.Curve, knot.StartParameter, knot.EndParameter,
                    radius, numLongitudinal, numTransverse);
                // Export mesh and material to a file:
                mesh.ExportToObj(objFile, mtlFile);
                MeshExportExtensions.ExportMaterial(mtlFile, new vec3(0, 0, 1));
                sw.Stop();
                Console.WriteLine("Mesh exported successfully.");
                Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms.");
                // ** Assert:
                1.Should().Be(1); // just a dummy asserrtion
            }
            catch (Exception ex)
            {
                Output.WriteLine($"\n\n======\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}");
                throw;
            }
        }


        /// <summary>Creates the first test of tubular surface generation and export.</summary>
        [Theory]
        [InlineData(400, 15, 0.6, 2, 3)]  // the trefoil knot
        [InlineData(400, 15, 0.6, 3, 7)]  // (3, 7) torus knot
        [InlineData(400, 15, 0.1, 2, 8)]  // (2, 8) torus link
        [InlineData(400, 15, 0.1, 3, 4)]  // (3, 4) torus knot
        [InlineData(400, 15, 0.1, 3, 5)]  // (3, 5) torus knot
        [InlineData(1000, 15, 0.1, 5, 6)]  // (3, 5) torus knot
        [InlineData(1500, 15, 0.1, 3, 11)]  // (3, 5) torus knot
        [InlineData(2000, 15, 0.1, 8, 9)]  // (7, 9) torus knot
        [InlineData(2000, 15, 0.1, 3, 17)]  // (7, 9) torus knot
        [InlineData(2000, 15, 0.1, 5, 19)]  // (7, 9) torus knot
        protected void Example_ExportTorusKnot(int numLongitudinal, int numTransverse, double radius,
            int p, int q)
        {
            try
            {
                Output.WriteLine($"Example / Semi-manual test: TORUS ({p}, {q}) knot:");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string objFile = exportDirectory + $"01_TorusKnot_{p}_{q}_{numLongitudinal}.obj";
                string mtlFile = exportDirectory + $"01_TorusKnot_{p}_{q}_{numLongitudinal}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the torus knot parameterization:
                var knot = new LissajousKnot3D(p, q);
                // Generate the tubular mesh
                try
                {
                    Directory.CreateDirectory(exportDirectory);
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"\n\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}\n");
                    throw;
                }
                // ** Act:
                // Generate tubular mesh from curve definition:
                var mesh = TubularMeshGenerator.Generate(knot.Curve, knot.StartParameter, knot.EndParameter, 
                    radius, numLongitudinal, numTransverse);
                // Export mesh and material to a file:
                mesh.ExportToObj(objFile, mtlFile);
                MeshExportExtensions.ExportMaterial(mtlFile, new vec3(0, 0, 1)); 
                sw.Stop();
                Console.WriteLine("Mesh exported successfully.");
                Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms.");
                // ** Assert:
                1.Should().Be(1); // just a dummy asserrtion
            }
            catch (Exception ex)
            {
                Output.WriteLine($"\n\n======\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}");
                throw;
            }
        }



        // ToDo: Correct the parameterization (find the correct one!)
        /// <summary>Generates and exports (as .obj) tubular surface created form smooth parameterizaion of cylindric
        /// billiard knot.
        /// <para>IMPORTANT: The parameterization used here is still wrong and it needs to be corrected.
        /// The other method with the _WrongParameterization suffix should ALSO BE KEPT because it 
        /// produces interesting curves)</summary>
        [Theory]
        [InlineData(400, 15, 0.05, 3, 8, 0.3)]  // 
        [InlineData(1000, 15, 0.05, 5, 17, 0.3)]  // 
        protected void Example_ExportCylindricalBilliardKnot(int numLongitudinal, int numTransverse, double radius,
            int N, int P, double A = 1.0)
        {
            try
            {
                Output.WriteLine($"Example / Semi-manual test: CYLINDRICAL BILLIARD ({N}, {P}) knot:");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string objFile = exportDirectory + $"01_CylindricalBilliardKnot_{N}_{P}_{numLongitudinal}.obj";
                string mtlFile = exportDirectory + $"01_CylindricalBilliardKnot_{N}_{P}_{numLongitudinal}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the cylindrical billiard knot parameterization:
                var knot = new CylindricalBilliardKnot(N, P, A);
                // Generate the tubular mesh
                try
                {
                    Directory.CreateDirectory(exportDirectory);
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"\n\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}\n");
                    throw;
                }
                // ** Act:
                // Generate tubular mesh from curve definition:
                var mesh = TubularMeshGenerator.Generate(knot.Curve, knot.StartParameter, knot.EndParameter,
                    radius, numLongitudinal, numTransverse);
                // Export mesh and material to a file:
                mesh.ExportToObj(objFile, mtlFile);
                MeshExportExtensions.ExportMaterial(mtlFile, new vec3(0, 0, 1));
                sw.Stop();
                Console.WriteLine("Mesh exported successfully.");
                Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms.");
                // ** Assert:
                1.Should().Be(1); // just a dummy asserrtion
            }
            catch (Exception ex)
            {
                Output.WriteLine($"\n\n======\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}");
                throw;
            }
        }




        // ToDo: Correct the parameterization (find the correct one!)
        /// <summary>Generates and exports (as .obj) tubular surface created form smooth parameterizaion of cylindric
        /// billiard knot (wrong, but the wrong parameterization should be kept because it  produces interesting curves).
        /// The other parameterization and its corresponding test will be kept.</summary>
        [Theory]
        [InlineData(400, 15, 0.1, 2, 3)]  // 
        [InlineData(1000, 15, 0.05, 5, 17)]  // 
        protected void Example_ExportCylindricalBilliardKnot_WrongParameterization(int numLongitudinal, int numTransverse, double radius,
            int N, int P, double A = 1.0)
        {
            try
            {
                Output.WriteLine($"Example / Semi-manual test: CYLINDRICAL BILLIARD ({N}, {P}) knot:");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string objFile = exportDirectory + $"01_CylindricalBilliardKnot_{N}_{P}_{numLongitudinal}.obj";
                string mtlFile = exportDirectory + $"01_CylindricalBilliardKnot_{N}_{P}_{numLongitudinal}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the cylindrical billiard knot parameterization:
                var knot = new CylindricalBilliardKnot_WrongParameterization(N, P, A);
                // Generate the tubular mesh
                try
                {
                    Directory.CreateDirectory(exportDirectory);
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"\n\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}\n");
                    throw;
                }
                // ** Act:
                // Generate tubular mesh from curve definition:
                var mesh = TubularMeshGenerator.Generate(knot.Curve, knot.StartParameter, knot.EndParameter,
                    radius, numLongitudinal, numTransverse);
                // Export mesh and material to a file:
                mesh.ExportToObj(objFile, mtlFile);
                MeshExportExtensions.ExportMaterial(mtlFile, new vec3(0, 0, 1));
                sw.Stop();
                Console.WriteLine("Mesh exported successfully.");
                Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms.");
                // ** Assert:
                1.Should().Be(1); // just a dummy asserrtion
            }
            catch (Exception ex)
            {
                Output.WriteLine($"\n\n======\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}");
                throw;
            }
        }




        #endregion Examples:KnotGroups



        #region Examples:SpecificKnots


        #endregion Examples:SpecificKnots





        #endregion Examples


    }

}


