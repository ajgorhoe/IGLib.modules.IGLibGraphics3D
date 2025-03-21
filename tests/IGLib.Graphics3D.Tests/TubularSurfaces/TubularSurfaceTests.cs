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
            Console = Output;
        }

        #region Examples

        /// <summary>Extract dat form information on running processes, which could be used to initialize random
        /// generators.</summary>
        [Fact]
        protected void Example_CreateAndExportTubularSurfaceTest()
        {
            try
            {
                Console.WriteLine($"Current dir.: {Directory.GetCurrentDirectory()}");
                Stopwatch sw = Stopwatch.StartNew();


                // Define a 3D helical curve
                Func<double, vec3> helix = t => new vec3(Math.Cos(t), Math.Sin(t), t / 5.0);

                // Generate the tubular mesh
                var mesh = TubularMeshGenerator_03.Generate(helix, 0, 6 * Math.PI, 0.1, 1000, 100);

                // Export mesh and material file
                string objFile = "helix.obj";
                string mtlFile = "helix.mtl";

                mesh.ExportToObj(objFile, mtlFile);
                MeshExportExtensions_03.ExportMaterial(mtlFile, new vec3(0, 0, 1)); // Blue color

                Console.WriteLine("Helix mesh exported successfully!");


                sw.Stop();
                Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms.");
                1.Should().Be(1); // just a dummy asserrtion
            }
            catch (Exception ex)
            {
                Output.WriteLine($"\n\n======\nERROR: {ex.GetType().Name} thrown:\n  {ex.Message}");
                throw;
            }
        }





        #endregion Examples


    }

}


