using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using IGLib.Tests.Base;
using System.Linq;
using System.Collections;
using IGLib.Core;
using IGLib.Core.CollectionExtensions;
using static IGLib.Tests.Base.SampleCollsctions.SampleCollections;

namespace IGLib.Core.Tests
{

    /// <summary>Tests utilities from the <see cref="CollectionExtensions"/> class.</summary>
    public class CollectionExtensionsTests : TestBase<CollectionExtensionsTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public CollectionExtensionsTests(ITestOutputHelper output) : base(output) 
        { }

        /// <summary>Open bracket used in the string representation of array-like objects.</summary>
        protected string ArrayIndentation { get; } = CollectionExtensions.CollectionExtensions.ArrayIndentation;

        /// <summary>Open bracket used in the string representation of array-like objects.</summary>
        protected string ArrayBracketOpen { get; } = CollectionExtensions.CollectionExtensions.ArrayBracketOpen;

        /// <summary>Closed bracket used in the string representation of array-like objects.</summary>
        protected string ArrayBracketClosed { get; } = CollectionExtensions.CollectionExtensions.ArrayBracketClosed;

        /// <summary>Array element separator used in the string representation of array-like objects.</summary>
        protected string ArraySeparator { get; } = CollectionExtensions.CollectionExtensions.ArraySeparator;

        /// <summary>Representation of null reference.</summary>
        protected string NullString { get; } = CollectionExtensions.CollectionExtensions.NullString;

        #region ToReadableString_ForCollections_BasicTests
        // Tests in this region perform basic verification of correctness of the extension methogs
        // ToreadableString(...) for array-like types of method parameter. This involves checking that brackets
        // and separators are present in generated strings, and that all array elementts are also present.

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_DoubleNumber_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            double collection = 2.43;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
2.43
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_IntNumber_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            int collection = 256;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
256
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_NullableDoubleNumber_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            double? collection = 4.22;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
4.22
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_NullableDoubleNumberNull_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            double? collection = null;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(NullString);
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
null
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_NullableIntNumber_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            int? collection = 6225;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
6225
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_NullableIntNumberNull_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            int? collection = null;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(NullString);
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
null
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }




        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_String_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            string collection = "The number is: 256";
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            stringRepresentation.Should().StartWith("\"");
            stringRepresentation.Should().EndWith("\"");
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
"The number is: 256"
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_StringWithDoubleQuotes_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            string collection = "The number is: 256; The string is: \"ABcd\"";
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().StartWith("\"");
            stringRepresentation.Should().EndWith("\"");
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
"The number is: 256; The string is: \"ABcd\""
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_StringNull_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            string collection = null;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Be(NullString);
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
null
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_Char_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char collection = 'x';
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
'x'
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_CharSingleQuote_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char collection = '\'';
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
'\''
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_NullableChar_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char? collection = 'x';
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
'x'
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_NullableCharSingleQuote_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char? collection = '\'';
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
'\''
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_NullableCharNull_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char? collection = null;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(NullString);
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
null
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }



        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_ArrayOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an integer array to a readable string...");
            int[] collection = IntArray;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{1, 2, 3}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_ListOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a List of integers to a readable string...");
            List<int> collection = IntList;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{1, 2, 3, 4, 5}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_IListOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an IList of integers to a readable string...");
            IList<int> collection = IntIList;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{1, 2, 3, 4}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_IEnumerableOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an IEnumerable of integers to a readable string...");
            IEnumerable<int> collection = IntIEnumerable;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{1, 2, 3, 4, 5, 6}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Should().Be(expectedOutput);
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_RectangularArray2dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 2D rectangular array of ntegers to a readable string...");
            int[,] collection = IntArray2x3;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            stringRepresentation.Should().Contain(ArrayIndentation);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{
    {11, 12, 13},
    {21, 22, 23}
}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n","\n").Should().Be(expectedOutput.Replace("\r\n","\n"));
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_RectangularArray3dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D rectangular array of integers to a readable string...");
            int[,,] collection = IntArray3x2x4;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            stringRepresentation.Should().Contain(ArrayIndentation);
            stringRepresentation.Should().Contain(ArrayIndentation + ArrayIndentation);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{
    {
        {111, 112, 113, 114},
        {121, 122, 123, 124}
    },
    {
        {211, 212, 213, 214},
        {221, 222, 223, 224}
    },
    {
        {311, 312, 313, 314},
        {321, 322, 323, 324}
    }
}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n", "\n").Should().Be(expectedOutput.Replace("\r\n", "\n"));
                ;
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_JaggedArray2dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 2D jagged array of integers to a readable string...");
            int[][] collection = IntJaggedArray2x3;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            stringRepresentation.Should().Contain(ArrayIndentation);
            foreach (int[] subCollection in collection)
            {
                foreach (int element in subCollection)
                    stringRepresentation.Should().Contain(element.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{
    {11, 12, 13},
    {21, 22, 23}
}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n", "\n").Should().Be(expectedOutput.Replace("\r\n", "\n"));
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_JaggedArray3dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D jagged array of integers to a readable string...");
            int[][][] collection = IntJaggedArray3x2x4;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int[][] subCollection in collection)
            {
                foreach (int[] subSubCollection in subCollection)
                {
                    foreach (int element in subSubCollection)
                        stringRepresentation.Should().Contain(element.ToString());
                }
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{
    {
        {111, 112, 113, 114},
        {121, 122, 123, 124}
    },
    {
        {211, 212, 213, 214},
        {221, 222, 223, 224}
    },
    {
        {311, 312, 313, 314},
        {321, 322, 323, 324}
    }
}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n", "\n").Should().Be(expectedOutput.Replace("\r\n", "\n"));
                ;
            }
        }



        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_JaggedArray2dNonrectangularOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 2D jagged array of integers to a readable string...");
            int[][] collection = IntJaggedArrayNonrectangular2x3;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int[] subCollection in collection)
            {
                foreach (int element in subCollection)
                    stringRepresentation.Should().Contain(element.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{
    {11, 12, 13},
    {21, 22}
}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n", "\n").Should().Be(expectedOutput.Replace("\r\n", "\n"));
                ;
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableString_JaggedArray3dNonrectangularOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D jagged array of integers to a readable string...");
            int[][][] collection = IntJaggedArrayNonrectangular3x2x4;
            string stringRepresentation = collection.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int[][] subCollection in collection)
            {
                foreach (int[] subSubCollection in subCollection)
                {
                    foreach (int element in subSubCollection)
                        stringRepresentation.Should().Contain(element.ToString());
                }
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
{
    {
        {111, 112, 113, 114},
        {121, 122, 1234}
    },
    {
        {211, 212, 213, 214}
    },
    {
        {311, 312},
        {321, 322, 323, 324}
    }
}
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n", "\n").Should().Be(expectedOutput.Replace("\r\n", "\n"));
                ;
            }
        }


        #endregion ToReadableString_ForCollections_BasicTests



        #region ToReadableString_ForCollectionsDynamic_BasicTests
        // Tests in this region perform verification of correctness of the extension methogs
        // ToreadableString(...) for object parameters whose actual types are array-like.


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_DoubleNumber_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value  to a readable string...");
            double collection = 2.43;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_NullableDoubleNumber_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            double? collection = 4.22;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_NullableDoubleNumberNull_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            double? collection = null;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_IntNumber_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            int collection = 256;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_NullableIntNumber_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            int? collection = 6225;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_NullableIntNumberNull_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            int? collection = null;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_String_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an integer array to a readable string...");
            string collection = "This is a test string";
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection);
            stringRepresentation.Should().StartWith("\"");
            stringRepresentation.Should().EndWith("\"");
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_StringNull_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an integer array to a readable string...");
            string collection = null;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_StringWithDoubleQuotes_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            string collection = "The number is: 256; The string is: \"ABcd\"";
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().StartWith("\"");
            stringRepresentation.Should().EndWith("\"");
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_Char_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char collection = 'x';
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            stringRepresentation.Should().StartWith("'");
            stringRepresentation.Should().EndWith("'");
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_CharSingleQuote_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char collection = '\'';
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            stringRepresentation.Should().StartWith("'");
            stringRepresentation.Should().EndWith("'");
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_NullableChar_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char? collection = 'x';
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            stringRepresentation.Should().StartWith("'");
            stringRepresentation.Should().EndWith("'");
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_NullableCharSingleQuote_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char? collection = '\'';
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(collection.ToString());
            stringRepresentation.Should().StartWith("'");
            stringRepresentation.Should().EndWith("'");
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_NullableCharNull_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a string value to a readable string...");
            char? collection = null;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().NotContain(ArrayBracketOpen);
            stringRepresentation.Should().NotContain(ArrayBracketClosed);
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_ArrayOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an integer array to a readable string...");
            int[] collection = IntArray;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_ListOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a List of integers to a readable string...");
            List<int> collection = IntList;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_IListOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an IList of integers to a readable string...");
            IList<int> collection = IntIList;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_IEnumerableOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an IEnumerable of integers to a readable string...");
            IEnumerable<int> collection = IntIEnumerable;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_RectangularArray2dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 2D rectangular array of ntegers to a readable string...");
            int[,] collection = IntArray2x3;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_RectangularArray3dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D rectangular array of integers to a readable string...");
            int[,,] collection = IntArray3x2x4;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_JaggedArray2dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 2D jagged array of integers to a readable string...");
            int[][] collection = IntJaggedArray2x3;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int[] subCollection in collection)
            {
                foreach (int element in subCollection)
                    stringRepresentation.Should().Contain(element.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_JaggedArray3dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D jagged array of integers to a readable string...");
            int[][][] collection = IntJaggedArray3x2x4;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int[][] subCollection in collection)
            {
                foreach (int[] subSubCollection in subCollection)
                {
                    foreach (int element in subSubCollection)
                        stringRepresentation.Should().Contain(element.ToString());
                }
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }



        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_JaggedArray2dNonrectangularOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 2D jagged array of integers to a readable string...");
            int[][] collection = IntJaggedArrayNonrectangular2x3;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int[] subCollection in collection)
            {
                foreach (int element in subCollection)
                    stringRepresentation.Should().Contain(element.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringDynamic_JaggedArray3dNonrectangularOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D jagged array of integers to a readable string...");
            int[][][] collection = IntJaggedArrayNonrectangular3x2x4;
            object obj = collection;
            string expectedOutput = collection.ToReadableString();
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString();
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(ArrayBracketOpen);
            stringRepresentation.Should().Contain(ArrayBracketClosed);
            stringRepresentation.Should().Contain(ArraySeparator);
            foreach (int[][] subCollection in collection)
            {
                foreach (int[] subSubCollection in subCollection)
                {
                    foreach (int element in subSubCollection)
                        stringRepresentation.Should().Contain(element.ToString());
                }
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        #endregion ToReadableString_ForCollections_Dynamic



        #region CustomFormatting

        // Custom formatting parameters used in tests:
        protected string CustomIndentation { get; } = "··";
        protected string CustomOpenBracket { get; } = "[";
        protected string CustomClosedBracket { get; } = "]";
        protected string CustomSeparator { get; } = ";";


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringCustomFormat_RectangularArray3dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D rectangular array of integers to a readable string...");
            int[,,] collection = IntArray3x2x4;
            string stringRepresentation = collection.ToReadableString(indentation: CustomIndentation, 
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(CustomOpenBracket);
            stringRepresentation.Should().Contain(CustomClosedBracket);
            stringRepresentation.Should().Contain(CustomSeparator);
            stringRepresentation.Should().Contain(CustomIndentation);
            stringRepresentation.Should().Contain(CustomIndentation + CustomIndentation);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
[
··[
····[111; 112; 113; 114];
····[121; 122; 123; 124]
··];
··[
····[211; 212; 213; 214];
····[221; 222; 223; 224]
··];
··[
····[311; 312; 313; 314];
····[321; 322; 323; 324]
··]
]
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n", "\n").Should().Be(expectedOutput.Replace("\r\n", "\n"));
                ;
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringCustomFormat_JaggedArray2dNonrectangularOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 2D jagged array of integers to a readable string...");
            int[][] collection = IntJaggedArrayNonrectangular2x3;
            string stringRepresentation = collection.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(CustomOpenBracket);
            stringRepresentation.Should().Contain(CustomClosedBracket);
            stringRepresentation.Should().Contain(CustomSeparator);
            stringRepresentation.Should().Contain(CustomIndentation);
            foreach (int[] subCollection in collection)
            {
                foreach (int element in subCollection)
                    stringRepresentation.Should().Contain(element.ToString());
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
[
··[11; 12; 13];
··[21; 22]
]
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n", "\n").Should().Be(expectedOutput.Replace("\r\n", "\n"));
                ;
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringCustomFormat_JaggedArray3dNonrectangularOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D jagged array of integers to a readable string...");
            int[][][] collection = IntJaggedArrayNonrectangular3x2x4;
            string stringRepresentation = collection.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(CustomOpenBracket);
            stringRepresentation.Should().Contain(CustomClosedBracket);
            stringRepresentation.Should().Contain(CustomSeparator);
            stringRepresentation.Should().Contain(CustomIndentation);
            stringRepresentation.Should().Contain(CustomIndentation + CustomIndentation);
            foreach (int[][] subCollection in collection)
            {
                foreach (int[] subSubCollection in subCollection)
                {
                    foreach (int element in subSubCollection)
                        stringRepresentation.Should().Contain(element.ToString());
                }
            }
            if (checkPreciseOutput)
            {
                Console.WriteLine("Verifying exact match with expected output...");
                string expectedOutput =
"""
[
··[
····[111; 112; 113; 114];
····[121; 122; 1234]
··];
··[
····[211; 212; 213; 214]
··];
··[
····[311; 312];
····[321; 322; 323; 324]
··]
]
""";
                Console.WriteLine($"Expected output:\n<<\n{expectedOutput}\n>>");
                stringRepresentation.Replace("\r\n", "\n").Should().Be(expectedOutput.Replace("\r\n", "\n"));
                ;
            }
        }


        #region CustomFormatting.Dynamic
        // Dynamic type detection:

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringCustomFormatDynamic_JaggedArray3dNonrectangularOfInt_WorksCorrectly(
            bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 3D jagged array of integers to a readable string...");
            int[][][] collection = IntJaggedArrayNonrectangular3x2x4;
            object obj = collection;
            string expectedOutput = collection.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(CustomOpenBracket);
            stringRepresentation.Should().Contain(CustomClosedBracket);
            stringRepresentation.Should().Contain(CustomSeparator);
            stringRepresentation.Should().Contain(CustomIndentation);
            stringRepresentation.Should().Contain(CustomIndentation + CustomIndentation);
            foreach (int[][] subCollection in collection)
            {
                foreach (int[] subSubCollection in subCollection)
                {
                    foreach (int element in subSubCollection)
                        stringRepresentation.Should().Contain(element.ToString());
                }
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringCustomFormatDynamic_ArrayOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an integer array to a readable string...");
            int[] collection = IntArray;
            object obj = collection;
            string expectedOutput = collection.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(CustomOpenBracket);
            stringRepresentation.Should().Contain(CustomClosedBracket);
            stringRepresentation.Should().Contain(CustomSeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringCustomParametersDynamic_RectangularArray2dOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting a 2D rectangular array of ntegers to a readable string...");
            int[,] collection = IntArray2x3;
            object obj = collection;
            string expectedOutput = collection.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(CustomOpenBracket);
            stringRepresentation.Should().Contain(CustomClosedBracket);
            stringRepresentation.Should().Contain(CustomSeparator);
            stringRepresentation.Should().Contain(CustomIndentation);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringCustomParametersDynamic_IListOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an IList of integers to a readable string...");
            IList<int> collection = IntIList;
            object obj = collection;
            string expectedOutput = collection.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(CustomOpenBracket);
            stringRepresentation.Should().Contain(CustomClosedBracket);
            stringRepresentation.Should().Contain(CustomSeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        protected void CollectionExtensoins_ToReadableStringCustomParametersDynamic_IEnumerableOfInt_WorksCorrectly(bool checkPreciseOutput)
        {
            Console.WriteLine("Converting an IEnumerable of integers to a readable string...");
            IEnumerable<int> collection = IntIEnumerable;
            object obj = collection;
            string expectedOutput = collection.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Expected string (from typed object):\n<<\n{expectedOutput}\n>>");
            string stringRepresentation = obj.ToReadableString(indentation: CustomIndentation,
                openBracket: CustomOpenBracket, closedBracket: CustomClosedBracket, separator: CustomSeparator);
            Console.WriteLine($"Produced string (angular brackets don't belong to the string):\n<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrWhiteSpace();
            stringRepresentation.Should().Contain(CustomOpenBracket);
            stringRepresentation.Should().Contain(CustomClosedBracket);
            stringRepresentation.Should().Contain(CustomSeparator);
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
            if (checkPreciseOutput)
            {
                stringRepresentation.Should().Be(expectedOutput);
            }
        }


        #endregion CustomFormatting.Dynamic

        #endregion CustomFormatting



    }

}


