
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace IG.Num
{

    /// <summary>Struct representing a 3D vector.</summary>
    public struct vec2
    {

        /// <summary>The first component of the vector.</summary>
        public double x;

        /// <summary>The second component of the vector.</summary>
        public double y;


        /// <summary>Constructor, initializes vecto components with provided values.</summary>
        /// <param name="x">First component of the vector.</param>
        /// <param name="y">Second component of the vector.</param>
        public vec2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>Vector addition.</summary>
        /// <returns></returns>
        public static vec2 operator +(vec2 a, vec2 b) => new vec2(a.x + b.x, a.y + b.y);

        /// <summary>Vector Subtraction.</summary>
        public static vec2 operator -(vec2 a, vec2 b) => new vec2(a.x - b.x, a.y - b.y);

        /// <summary>Multiplication of vector by scalar.</summary>
        public static vec2 operator *(double scalar, vec2 v) => new vec2(scalar * v.x, scalar * v.y);

        /// <summary>Multiplication of vector by scalar where scalar is the first parameter.</summary>
        public static vec2 operator *(vec2 v, double scalar) => new vec2(scalar * v.x, scalar * v.y);

        /// <summary>Dot Product (scalar product).</summary>
        public static double Dot(vec2 a, vec2 b) => a.x * b.x + a.y * b.y;

        /// <summary>Cross Product of 2D vectors (<see cref="vec2"/>), results in a 3D vector in the 
        /// direction of the 3rd coordinate axis (Z).</summary>
        public static vec3 Cross(vec2 a, vec2 b) =>
            new vec3(0.0, 0.0,
                a.x * b.y - a.y * b.x
            );

        /// <summary>Cross product of the current vector with another vector (in this order);
        /// Calls <see cref="Cross(vec2, vec2)"/> to do the job.</summary>
        public vec3 Cross(vec2 v2)
        {
            return Cross(this, v2);
        }

        const double DefaulltEpsilon = 1e-15;

        /// <summary>Returns the normalized current vector.</summary>
        /// <param name="epsilon">If calculated length of the vector is below this value, vector [0, 0]
        /// is returned in order to avoid division with very small numbers. Default: </param>
        /// <returns></returns>
        public vec2 Normalize(double epsilon = 1e-15)
        {
            double length = Math.Sqrt(x * x + y * y);
            return length > DefaulltEpsilon ? new vec2(x / length, y / length) : new vec2(0, 0);
        }
    }

}
