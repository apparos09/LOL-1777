using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Calculations for custom math.
    public class CustomMath
    {
        // ROUND //
        // Rounds to the provided number of decimal places.
        public static float Round(float value, int decimalPlaces)
        {
            // Calculates the factor.
            float factor = Mathf.Pow(10, decimalPlaces);

            // If the factor is less than or equal to 0, make it 1 (no effect).
            if (factor <= 0)
                factor = 1;

            // Calculates the result.
            float result = (Mathf.Round(value * factor)) / factor;
            return result;
        }

        // Ceiling rounds to the provided number of decimal places.
        public static float Ceil(float value, int decimalPlaces)
        {
            // Calculates the factor.
            float factor = Mathf.Pow(10, decimalPlaces);

            // If the factor is less than or equal to 0, make it 1 (no effect).
            if (factor <= 0)
                factor = 1;

            // Calculates the result.
            float result = (Mathf.Ceil(value * factor)) / factor;
            return result;
        }

        // Floor rounds to the provided number of decimal places.
        public static float Floor(float value, int decimalPlaces)
        {
            // Calculates the factor.
            float factor = Mathf.Pow(10, decimalPlaces);

            // If the factor is less than or equal to 0, make it 1 (no effect).
            if (factor <= 0)
                factor = 1;

            // Calculates the result.
            float result = (Mathf.Floor(value * factor)) / factor;
            return result;
        }

        // Truncates to the provided number of decimal places.
        public static float Truncate(float value, int decimalPlaces)
        {
            // Floors the value to get the whole number, and uses it to calculate the decimal portion.
            float wholePart = Mathf.Floor(value);
            float decimalPart = value - wholePart;

            // The result to be returned.
            float result;

            // If there should be no decimal places, return the whole number.
            // Also return the whole number if there is no decimal portion.
            // This is checked by seeing if the original value is approximately equal to the whole part.
            if(decimalPlaces <= 0 || Mathf.Approximately(value, wholePart))
            {
                result = wholePart;
            }
            //  Round the decimal part and add it to the result.
            else
            {
                // Round the decimal part to the provided number of digits.
                float decimalRounded = Round(decimalPart, decimalPlaces);

                // Add the rounded decimal to the whole portion.
                result = wholePart + decimalRounded;
            }

            return result;
        }

        // ROTATE //

        // Rotates the 2D vector (around its z-axis).
        public static Vector2 Rotate(Vector2 v, float angle, bool inDegrees)
        {
            // Converts the angle to radians if provided in degrees.
            angle = (inDegrees) ? Mathf.Deg2Rad * angle : angle;

            // Calculates the rotation using matrix math...
            // Except it manually puts in what the resulting calculation would be).
            Vector2 result;
            result.x = (v.x * Mathf.Cos(angle)) - (v.y * Mathf.Sin(angle));
            result.y = (v.x * Mathf.Sin(angle)) + (v.y * Mathf.Cos(angle));
            
            return result;
        }

        // The rotation matrix.
        private static Vector3 Rotate(Vector3 v, float angle, char axis, bool inDegrees)
        {
            // Converts the angle to radians if provided in degrees.
            angle = (inDegrees) ? Mathf.Deg2Rad * angle : angle;

            // The rotation matrix.
            Matrix4x4 rMatrix = new Matrix4x4();

            // Checks what axis to rotate the vector3 on.
            switch(axis)
            {
                case 'x': // X-Axis
                case 'X':
                    // Rotation X Matrix
                    /*
                     * [1, 0, 0, 0]
                     * [0, cos a, -sin a, 0]
                     * [0, sin a, cos a, 0]
                     * [0, 0, 0, 1]
                     */

                    rMatrix.SetRow(0, new Vector4(1, 0, 0, 0));
                    rMatrix.SetRow(1, new Vector4(0, Mathf.Cos(angle), -Mathf.Sin(angle), 0));
                    rMatrix.SetRow(2, new Vector4(0, Mathf.Sin(angle), Mathf.Cos(angle), 0));
                    rMatrix.SetRow(3, new Vector4(0, 0, 0, 1));
                    break;

                case 'y': // Y-Axis
                case 'Y':
                    // Rotation Y Matrix
                    /*
                     * [cos a, 0, sin a, 0]
                     * [0, 1, 0, 0]
                     * [-sin a, 0, cos a, 0]
                     * [0, 0, 0, 1]
                     */

                    rMatrix.SetRow(0, new Vector4(Mathf.Cos(angle), 0, Mathf.Sin(angle), 0));
                    rMatrix.SetRow(1, new Vector4(0, 1, 0, 0));
                    rMatrix.SetRow(2, new Vector4(-Mathf.Sin(angle), 0, Mathf.Cos(angle), 0));
                    rMatrix.SetRow(3, new Vector4(0, 0, 0, 1));
                    break;

                case 'z': // Z-Axis
                case 'Z':
                    // Rotation Z Matrix
                    /*
                     * [cos a, -sin a, 0, 0]
                     * [sin a, cos a, 0, 0]
                     * [0, 0, 1, 0]
                     * [0, 0, 0, 1]
                     */

                    rMatrix.SetRow(0, new Vector4(Mathf.Cos(angle), -Mathf.Sin(angle), 0, 0));
                    rMatrix.SetRow(1, new Vector4(Mathf.Sin(angle), Mathf.Cos(angle), 0, 0));
                    rMatrix.SetRow(2, new Vector4(0, 0, 1, 0));
                    rMatrix.SetRow(3, new Vector4(0, 0, 0, 1));
                    break;

                default: // Unknown
                    return v;
            }
            


            // The vector matrix.
            Matrix4x4 vMatrix = new Matrix4x4();
            vMatrix[0, 0] = v.x;
            vMatrix[1, 0] = v.y;
            vMatrix[2, 0] = v.z;
            vMatrix[3, 0] = 1;


            // The resulting matrix.
            Matrix4x4 resultMatrix = rMatrix * vMatrix;

            // Gets the vector3 from the result matrix.
            Vector3 resultVector = new Vector3(
                resultMatrix[0, 0],
                resultMatrix[1, 0],
                resultMatrix[2, 0]
                );

            // Returns the result.
            return resultVector;
        }

        // Rotate around the x-axis.
        public static Vector3 RotateX(Vector3 v, float angle, bool inDegrees)
        {
            return Rotate(v, angle, 'X', inDegrees);
        }

        // Rotate around the y-axis.
        public static Vector3 RotateY(Vector3 v, float angle, bool inDegrees)
        {
            return Rotate(v, angle, 'Y', inDegrees);
        }

        // Rotate around the z-axis.
        public static Vector3 RotateZ(Vector3 v, float angle, bool inDegrees)
        {
            return Rotate(v, angle, 'Z', inDegrees);
        }

        // MATRIX //
        // Multiplies a 4 x 4 matrix by a value.
        public static Matrix4x4 Matrix4x4Multiply(Matrix4x4 m, float value)
        {
            Matrix4x4 mx = new Matrix4x4();

            // calculation
            mx.SetRow(0, new Vector4(value * m.m00, value * m.m01, value * m.m02, value * m.m03));
            mx.SetRow(1, new Vector4(value * m.m10, value * m.m11, value * m.m12, value * m.m13));
            mx.SetRow(2, new Vector4(value * m.m20, value * m.m21, value * m.m22, value * m.m23));
            mx.SetRow(3, new Vector4(value * m.m30, value * m.m31, value * m.m32, value * m.m33));

            return mx;
        }
    }
}