using System;
using System.Collections.Generic;
using System.Linq;

namespace IG.Num
{

    // Struct for 3D vector operations
    public struct vec3
    {
        public double x, y, z;

        public vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // Vector Addition
        public static vec3 operator +(vec3 a, vec3 b) => new vec3(a.x + b.x, a.y + b.y, a.z + b.z);

        // Vector Subtraction
        public static vec3 operator -(vec3 a, vec3 b) => new vec3(a.x - b.x, a.y - b.y, a.z - b.z);

        // Scalar Multiplication
        public static vec3 operator *(double scalar, vec3 v) => new vec3(scalar * v.x, scalar * v.y, scalar * v.z);
        public static vec3 operator *(vec3 v, double scalar) => new vec3(scalar * v.x, scalar * v.y, scalar * v.z);

        // Dot Product
        public static double Dot(vec3 a, vec3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

        // Cross Product
        public static vec3 Cross(vec3 a, vec3 b) =>
            new vec3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
            );

        public vec3 Cross(vec3 v2)
        {
            return Cross(this, v2);
        }

        // Normalize a vector
        public vec3 Normalize()
        {
            double length = Math.Sqrt(x * x + y * y + z * z);
            return length > 1e-9 ? new vec3(x / length, y / length, z / length) : new vec3(0, 0, 0);
        }
    }

}
