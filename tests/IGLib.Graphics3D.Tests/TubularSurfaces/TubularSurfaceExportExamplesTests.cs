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


namespace IGLib.Graphics3D.Tests
{

    /// <summary>These tests create and export meshes for tubular surfaces created from 3D parametric
    /// curves, including various knot parameterizations, knot groups (torus knots, Lissajous knots, 
    /// cylindrical billiard knots), selected individual konts, etc. Regions are arranged by the type 
    /// of the exported curve or knot.</summary>
    public class TubularSurfaceExportExamplesTests : TestBase<TubularSurfaceExportExamplesTests>
    {

        /// <summary>Calling base constructor initializes things like Output property to 
        /// write on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public TubularSurfaceExportExamplesTests(ITestOutputHelper output) : base(output)
        {
            
        }


        #region Examples


        #region BasicExamples

        // Prefixes: 00, 01, 02, ...

        /// <summary>Creates the first test of tubular surface generation and export, using a 
        /// custom parameterization of a helix curve as example.</summary>
        [Theory]
        [InlineData(100, 10, 0.1)]
        //[InlineData(400, 15, 0.1)]
        //[InlineData(1000, 20, 0.5)]
        protected void Example00_CreateAndExportTubularSurfaceTest(int numLongitudinal, int numTransverse, double radius)
        {
            try
            {
                Output.WriteLine("Example / Semi-manual test:");
                Output.WriteLine("  Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software:");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string fileName = $"00_TubularSurfaceFirstTest_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
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
                var mesh = TubularMeshGenerator_05.Generate(helix, 0.0, 6 * Math.PI, radius, numLongitudinal, numTransverse);
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


        /// <summary>Creeates a tubular surface mesh from a helix parameterization, <see cref="HelixCurve3D"/>.</summary>
        [Theory]
        [InlineData(400, 15, 0.2, 1.0, 0.2, true)]   // right-handed 
        [InlineData(400, 15, 0.2, 1.0, 0.2, false)]  // lef-handed , similar as above
        protected void Example01_1_ExportHelixCurve3DTube(int numLongitudinal, int numTransverse, double radius,
            double a = 1.0, double b = 0.2, bool righthanded = true)
        {
            try
            {
                Output.WriteLine($"Example / Semi-manual test: helix, a = {a}, b = ({b}, right-handed: {righthanded}");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string fileName = $"01.1_Helix_{a}_{b}_{righthanded}_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the curve parameterization:
                var curveDef = new HelixCurve3D(a, b, righthanded);
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
                var mesh = TubularMeshGenerator_05.Generate(curveDef.Curve, curveDef.StartParameter, 2 * curveDef.EndParameter,
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




        #region Examples:BasicCurves

        // Prefixes: 10, 11, 12, ..., 20, 21, ...


        /// <summary>Creeates a tubular surface mesh from a helix parameterization, <see cref="HelixCurve3D"/>.</summary>
        [Theory]
        [InlineData(400, 15, 0.2, 1.0, 0.2)]
        [InlineData(400, 15, 0.2, 2.0, 0.3)]
        protected void Example11_1_ExportConicalSpiralArchimedian3DTube(int numLongitudinal, int numTransverse, double radius,
            double alpha = 1.5, double a = 1.0)
        {
            try
            {
                Output.WriteLine($"Example / Semi-manual test: conical Archimedean spiral, slope = {alpha}, a = {a}");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string fileName = $"11.1_ConicalSpiralArchimedean_{alpha}_{a}_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the curve parameterization:
                var curveDef = new ConicalSpiralArchimedean3D(alpha, a);
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
                var mesh = TubularMeshGenerator_05.Generate(curveDef.Curve, curveDef.StartParameter, curveDef.EndParameter,
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


        /// <summary>Creeates a tubular surface mesh from a helix parameterization, <see cref="HelixCurve3D"/>.</summary>
        [Theory]
        [InlineData(400, 15, 0.2, 1.0, 1.0, 0.15)]
        [InlineData(400, 15, 0.2, 2.0, 2.0, 0.06)]
        protected void Example11_2_ExportConicalSpiralLogarithmic3DTube(int numLongitudinal, int numTransverse, double radius,
            double alpha = 1.5, double a = 1.0, double k = 0.01)
        {
            try
            {
                Output.WriteLine($"Example / Semi-manual test: conical logarithmic spiral, slope = {alpha}, a = {a}, k = {k}");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string fileName = $"11.2_ConicalSpiralLogarithmic_{alpha}_{a}_{k}_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the curve parameterization:
                var curveDef = new ConicalSpiralLogarithmic3D(alpha, a, k);
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
                var mesh = TubularMeshGenerator_05.Generate(curveDef.Curve, curveDef.StartParameter, curveDef.EndParameter,
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


        /// <summary>Creeates a tubular surface mesh from a helix parameterization, <see cref="HelixCurve3D"/>.</summary>
        [Theory]
        [InlineData(400, 15, 0.2, 1.0, 1.0)]
        [InlineData(400, 15, 0.2, 2.0, 2.0)]
        protected void Example11_3_ExportConicalSpiralFermats3DTube(int numLongitudinal, int numTransverse, double radius,
            double alpha = 1.5, double a = 1.0)
        {
            try
            {
                Output.WriteLine($"Example / Semi-manual test: conical Fermat's spiral, slope = {alpha}, a = {a}");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string fileName = $"11.3_ConicalSpiralFermats_{alpha}_{a}_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the curve parameterization:
                var curveDef = new ConicalSpiralFermats3D(alpha, a);
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
                var mesh = TubularMeshGenerator_05.Generate(curveDef.Curve, curveDef.StartParameter, curveDef.EndParameter,
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


        /// <summary>Creeates a tubular surface mesh from a helix parameterization, <see cref="HelixCurve3D"/>.</summary>
        [Theory]
        [InlineData(400, 15, 0.2, 1.0, 1.0)]
        [InlineData(400, 15, 0.2, 2.0, 2.0)]
        protected void Example11_4_ExportConicalSpiralHyperbolic3DTube(int numLongitudinal, int numTransverse, double radius,
            double alpha = 1.5, double a = 1.0)
        {
            try
            {
                Output.WriteLine($"Example / Semi-manual test: conical Fermat's spiral, slope = {alpha}, a = {a}");
                Output.WriteLine($"Mesh: {numLongitudinal} x {numTransverse}, radius: {radius}");
                Output.WriteLine("Creation and export of TUBULAR SURFACE mesh\n  to view in Blender or similar software...");
                // ** Arrange: 
                // Export paths for mesh and material files
                string exportDirectory = ExportPathIGLib;
                string fileName = $"11.4_ConicalSpiralHyperbolic_{alpha}_{a}_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the curve parameterization:
                var curveDef = new ConicalSpiralHyperbolic3D(alpha, a);
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
                var mesh = TubularMeshGenerator_05.Generate(curveDef.Curve, curveDef.StartParameter, curveDef.EndParameter,
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


        #endregion Examples:BasicCurves

        // Prefixes: 30, 31, 32, ..., 40, 41, ...


        #region Examples:AdvancedCurves

        // Prefixes: 50, 51, 52, ..., 50, 51, ...

        #endregion Examples:AdvancedCurves




        #region Examples:KnotGroups

        // Prefixes: 50, 51, 52, ..., 50, 51, ...


        /// <summary>Creates the first test of tubular surface generation and export.</summary>
        [Theory]
        [InlineData(400, 15, 0.6, 2, 3)]  // the trefoil knot
        [InlineData(400, 15, 0.6, 3, 7)]  // (3, 7) torus knot
        [InlineData(400, 15, 0.1, 2, 8)]  // (2, 8) torus LINK
        [InlineData(400, 15, 0.1, 3, 4)]  // (3, 4) torus knot
        [InlineData(400, 15, 0.1, 3, 5)]  // (3, 5) torus knot
        [InlineData(1000, 15, 0.1, 5, 6)]  // (3, 5) torus knot
        [InlineData(1500, 15, 0.1, 3, 11)]  // (3, 11) torus knot
        [InlineData(2000, 15, 0.1, 8, 9)]  // (8, 9) torus knot
        [InlineData(2000, 15, 0.1, 3, 17)]  // (3, 17) torus knot
        [InlineData(2000, 15, 0.1, 5, 19)]  // (5, 19) torus knot
        protected void Example50_ExportTorusKnot(int numLongitudinal, int numTransverse, double radius,
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
                string fileName = $"50_TorusKnot_{p}_{q}_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Export dir.: {Path.GetFullPath(exportDirectory)}");
                Console.WriteLine($"Exported files: \n  {Path.GetFileName(objFile)} \n  {Path.GetFileName(mtlFile)}");
                Stopwatch sw = Stopwatch.StartNew();
                // Define the torus knot parameterization:
                var knot = new TorusKnot3D(p, q);
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
                var mesh = TubularMeshGenerator_05.Generate(knot.Curve, knot.StartParameter, knot.EndParameter,
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
        [InlineData(600 /* long */, 15 /* trans */, 0.05 /* r */, 3 /* n1 */, 2 /* n2 */, 7 /* n3 */,
            0.7 /* fi1 */, 0.2 /* fi2 */)]    // the three-twist knot
        [InlineData(600 /* long */, 15 /* trans */, 0.05 /* r */, 3 /* n1 */, 2 /* n2 */, 5 /* n3 */,
            1.5 /* fi1 */, 0.2 /* fi2 */)]    // the Stevedore knot
        [InlineData (600 /* long */, 15 /* trans */, 0.05 /* r */, 3 /* n1 */, 5 /* n2 */, 7 /* n3 */, 
            0.7 /* fi1 */, 1.0 /* fi2 */)]    // square knot
        [InlineData (600 /* long */, 15 /* trans */, 0.05 /* r */, 3 /* n1 */, 4 /* n2 */, 7 /* n3 */, 
            0.1 /* fi1 */, 0.7 /* fi2 */)]    // the 8_21 knot
        protected void Example51_ExportLissajousKnot(int numLongitudinal, int numTransverse, double radius,
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
                string fileName = $"51_LissajousKnot_{n1}_{n2}_{n3}__{fi1}_{fi2}__{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
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
                var mesh = TubularMeshGenerator_05.Generate(knot.Curve, knot.StartParameter, knot.EndParameter,
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
        protected void Example52_0_ExportCylindricalBilliardKnot(int numLongitudinal, int numTransverse, double radius,
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
                string fileName = $"52.0_CylindricalBilliardKnot_{N}_{P}_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
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
                var mesh = TubularMeshGenerator_05.Generate(knot.Curve, knot.StartParameter, knot.EndParameter,
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
        protected void Example52_1_ExportCylindricalBilliardKnot_WrongParameterization(int numLongitudinal, int numTransverse, double radius,
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
                string fileName = $"52.1_CylindricalBilliardKnot_{N}_{P}_{numLongitudinal}";
                string objFile = exportDirectory + $"{fileName}.obj";
                string mtlFile = exportDirectory + $"{fileName}.mtl";
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
                var mesh = TubularMeshGenerator_05.Generate(knot.Curve, knot.StartParameter, knot.EndParameter,
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


