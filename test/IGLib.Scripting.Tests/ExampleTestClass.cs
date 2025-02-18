using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace IG.SandboxTests
{

    /// <summary><para>This test class just serves as example of how to create unit tests.</para>
    /// <para>Tests can be run either by right-clicking the test class or test method name and selecting
    /// "Run Tests", or opening the Visual Studio's Test Explorer and running tests from there (don't 
    /// forget to build the code beforehand).</para>
    /// <para>This test class inherits from <see cref="TestBase{ExampleTestClass}"/>, from which it inherits
    /// property  <see cref="TestBase{TestClass}.Output"/> of type <see cref="ITestOutputHelper"/>, which can 
    /// be used to write on test's output. Only the simple <see cref="ITestOutputHelper.WriteLine(string)"/>
    /// and <see cref="ITestOutputHelper.WriteLine(string, object[])"/> methods are available for writing 
    /// to test Output. In the Test Explorer in Visual Studio you can execute the tests and you can very
    /// nicely see which tests fail, but sometimes it is beneficial to write additional output before the
    /// assertions are made, such that you can efficiently trace causes of errors.</para></summary>
    public class ExampleTestClass : TestBase<ExampleTestClass>
    {
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public ExampleTestClass(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output property.
        }





    }

}

