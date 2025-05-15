using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using IGLib.Tests.Base.SampleClasses;

namespace IGLib.Tests.Base.SampleCollsctions
{

    public static class SampleCollections
    {

        // Some hard-coded definitions of array values used in tests:
        #region SampleCollectionValues


        /// <summary>Sample array of int (type <see cref="int[]"/>), for use in tests.</summary>
        public static int[] IntArray { get; } = { 1, 2, 3 };

        /// <summary>Sample List of int (type <see cref="List{int}"/>), for use in tests.</summary>
        public static List<int> IntList { get; } = [1, 2, 3, 4, 5];

        /// <summary>Sample IList of int (type <see cref="IList{int}"/>, actual type <see cref="CustomList{int}"/>), 
        /// for use in tests.</summary>
        public static IList<int> IntIList { get; } = new CustomList<int>() {1, 2, 3, 4 };

        /// <summary>Sample IList of int (type <see cref="IEnumerable{int}"/>, actual type 
        /// <see cref="CustomEnumerable{int}"/>), for use in tests.</summary>
        public static IEnumerable<int> IntIEnumerable { get; } = new CustomList<int>() {1, 2, 3, 4, 5, 6 };

        /// <summary>Sample 2D rectangular array of int (type <see cref="int[,]"/>), for use in tests.
        /// Dimensions of the array are 2*3.</summary>
        public static int[,] IntArray2x3 { get; } =
        {
            { 11, 12, 13 },
            { 21, 22, 23 }
        };

        /// <summary>Sample 2D rectangular array of strings (type <see cref="string[,]"/>), for use in tests.
        /// Dimensions of the array are 2*3.
        /// <para>Elements of the array correspond to elements of <see cref="IntArray2x3"/> converted to 
        /// strings by <see cref="int.ToString()"/> method.</para></summary>
        public static string[,] StringArray2x3 { get; } =
        {
            { "11", "12", "13" },
            { "21", "22", "23" }
        };

        /// <summary>Sample 3D rectangular array of int (type <see cref="int[,,]"/>), for use in tests.
        /// Dimensions of the array are 2*3*4.</summary>
        public static int[,,] IntArray3x2x4 { get; } =
        {
            {
                { 111, 112, 113, 114 },
                { 121, 122, 123, 124 },
            },
            {
                { 211, 212, 213, 214 },
                { 221, 222, 223, 224 },
            },
            {
                { 311, 312, 313, 314 },
                { 321, 322, 323, 324 },
            }
        };

        /// <summary>Sample 3D rectangular array of strings (type <see cref="string[,,]"/>), for use in tests.
        /// Dimensions of the array are 2*3*4.
        /// <para>Elements of the array correspond to elements of <see cref="IntArray3x2x4"/> converted to 
        /// strings by <see cref="int.ToString()"/> method.</para></summary>
        public static string[,,] StringArray3x2x4 { get; } =
        {
            {
                { "111", "112", "113", "114" },
                { "121", "122", "123", "124" },
            },
            {
                { "211", "212", "213", "214" },
                { "221", "222", "223", "224" },
            },
            {
                { "311", "312", "313", "314" },
                { "321", "322", "323", "324" },
            }
        };

        /// <summary>Sample 2D jagged array of int (type <see cref="int[][]"/>), for use in tests.
        /// Array's shape corresponds to a 2D rectangular 2*3 array.</summary>
        public static int[][] IntJaggedArray2x3 { get; } =
        {
            new int[] { 11, 12, 13 },
            new int[] { 21, 22, 23 }
        };

        /// <summary>Sample 2D jagged array of strings (type <see cref="string[][]"/>), for use in tests.
        /// Array's shape corresponds to a 2D rectangular 2*3 array.
        /// <para>Elements of the array correspond to elements of <see cref="IntJaggedArray2x3"/> converted to 
        /// strings by <see cref="int.ToString()"/> method.</para></summary>
        public static string[][] StringJaggedArray2x3 { get; } =
        {
            new string[] { "11", "12", "13" },
            new string[] { "21", "22", "23" }
        };

        /// <summary>Sample 2D jagged array of int (type <see cref="int[][]"/>), for use in tests.
        /// The array does not correspond to a rectangular array (some elements are missing).
        /// The smallest rectangular array that contains it is 2*3.</summary>
        public static int[][] IntJaggedArrayNonrectangular2x3 { get; } =
        {
            new int[] { 11, 12, 13 },
            new int[] { 21, 22 }
        };

        /// <summary>Sample 2D jagged array of strings (type <see cref="string[][]"/>), for use in tests.
        /// The array does not correspond to a rectangular array (some elements are missing).
        /// The smallest rectangular array that contains it is 2*3.
        /// <para>Elements of the array correspond to elements of <see cref="IntJaggedArrayNonrectangular2x3"/> converted to 
        /// strings by <see cref="int.ToString()"/> method.</para></summary>
        public static string[][] StringJaggedArrayNonrectangular2x3 { get; } =
        {
            new string[] { "11", "12", "13" },
            new string[] { "21", "22" }
        };

        /// <summary>Sample 2D jagged array of int (type <see cref="int[][][]"/>), for use in tests.
        /// Array's shape corresponds to a 2D rectangular 3*2*4 array.</summary>
        public static int[][][] IntJaggedArray3x2x4 { get; } =
        {
            new int[][]
            {
                new int[] { 111, 112, 113, 114 },
                new int[] { 121, 122, 123, 124 }
            },
            new int[][]
            {
                new int[] { 211, 212, 213, 214 },
                new int[] { 221, 222, 223, 224 }
            },
            new int[][]
            {
                new int[] { 311, 312, 313, 314 },
                new int[] { 321, 322, 323, 324 }
            }
        };

        /// <summary>Sample 2D jagged array of strings (type <see cref="string[][][]"/>), for use in tests.
        /// Array's shape corresponds to a 2D rectangular 3*2*4 array.
        /// <para>Elements of the array correspond to elements of <see cref="IntJaggedArray3x2x4"/> converted to 
        /// strings by <see cref="int.ToString()"/> method.</para></summary>
        public static string[][][] StringJaggedArray3x2x4 { get; } =
        {
            new string[][]
            {
                new string[] { "111", "112", "113", "114" },
                new string[] { "121", "122", "123", "124" }
            },
            new string[][]
            {
                new string[] { "211", "212", "213", "214" },
                new string[] { "221", "222", "223", "224" }
            },
            new string[][]
            {
                new string[] { "311", "312", "313", "314" },
                new string[] { "321", "322", "323", "324" }
            }
        };

        /// <summary>Sample 3D jagged array of int (type <see cref="int[][][]"/>), for use in tests.
        /// The array does not correspond to a rectangular array (some elements are missing).
        /// The smallest rectangular array that contains it is 3*2*4.</summary>
        public static int[][][] IntJaggedArrayNonrectangular3x2x4 { get; } =
        {
            new int[][]
            {
                new int[] { 111, 112, 113, 114 },
                new int[] { 121, 122, 1234 }
            },
            new int[][]
            {
                new int[] { 211, 212, 213, 214 }
            },
            new int[][]
            {
                new int[] { 311, 312 },
                new int[] { 321, 322, 323, 324 }
            }
        };

        /// <summary>Sample 3D jagged array of strings (type <see cref="string[][][]"/>), for use in tests.
        /// The array does not correspond to a rectangular array (some elements are missing).
        /// The smallest rectangular array that contains it is 3*2*4.
        /// <para>Elements of the array correspond to elements of <see cref="IntJaggedArrayNonrectangular3x2x4"/> converted to 
        /// strings by <see cref="int.ToString()"/> method.</para></summary>
        public static string[][][] StringJaggedArrayNonrectangular3x2x4 { get; } =
        {
            new string[][]
            {
                new string[] { "111", "112", "113", "114" },
                new string[] { "121", "122", "1234" }
            },
            new string[][]
            {
                new string[] { "211", "212", "213", "214" }
            },
            new string[][]
            {
                new string[] { "311", "312" },
                new string[] { "321", "322", "323", "324" }
            }
        };

        #endregion SampleCollectionValues


    }


}
