using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        // STRING MATH CALCULATION
        // Performs a BEDMAS calculation on a string. If an empty string is returned, the calculation failed.
        public static string CalculateMathString(string equation)
        {
            /*
             * Rules/Checks:
             * Decimal Point: 
             *  - A number can only have one decimal point. If there's a decimal point at the end of a number...
             *  - Treat it as the number ending it as a zero.
             * Brackets (B):
             *  - A quick check can be done with the number of left brackets and right brackets to see if an equation might be valid.
             *  - You can check the bracket count to catch brackets that are placed within brackets.
             *  - A bracket next to a number counts as multiplication (e.g., (4)3 is 4 x 3).
             * Exponents (E):
             *  - Represented by "^".
             * Division (D):
             *  - Represented by the "/" symbol.
             *  - If the user attempts to divide by 0 (x / 0), then return an empty string by default.
             * Multiplication (M):
             *  - Represented by the "*" symbol.
             * Addition (A):
             *  - A plus sign (+) may be used to indicate that a number is positive.
             * Subtraction (S):
             *  - A minus sign (-) may be used to indicate that a number is negative.
             * Other:
             *  - Return an empty string if the operation is invalid.
             */

            // First, remove all the spaces.
            string equationAdjusted = equation.Replace(" ", "");

            // Second, check that the number of left and right brackets are equal.
            {
                int leftBrackets = StringHelper.GetSubstringCount(equation, "(");
                int rightBrackets = StringHelper.GetSubstringCount(equation, ")");

                // The bracket counts don't match, meaning the equation is invalid.
                // Return an empty string.
                if(leftBrackets != rightBrackets)
                {
                    return "";
                }
            }

            // TODO: add in multiplication symbols in cases where a bracket is next to a number.
            // Also check for decimals with no numbers after the decimal point.

            // equation.Count()

            // Run the recursive caculation.
            string result = RunStringMathCalculationRecursive(equation);

            // Return the result.
            return result;
        }

        // A recursion string.
        private static string RunStringMathCalculationRecursive(string equation)
        {
            // If the string is empty, return an empty string.
            if (equation == "")
                return string.Empty;

            // Checks if the equation contains a math operation.
            if(ContainsMathOperationSymbol(equation)) // Has operations.
            {
                // The operation that will be performed.
                string operation = "";

                // TODO: account for brackets.

                // Checks what operation to use.
                if(equation.Contains("^")) // Exponent
                {
                    operation = "^";
                }
                else if(equation.Contains("/")) // Division
                {
                    operation = "/";
                }
                else if(equation.Contains("*")) // Multiplication
                {
                    operation = "*";
                }
                else if(equation.Contains("+")) // Addition
                {
                    operation = "+";
                }
                else if(equation.Contains("-")) // Subtraction
                {
                    operation = "-";
                }

                // Gets the index of the operation.
                int opIndex = equation.IndexOf(operation);

                // This shouldn't happen, but if it does, just return a blank string.
                if(opIndex == -1)
                {
                    return "";
                }

                // TODO: does not handle multiple operations in one equation correctly.

                // LEFT SIDE
                // Gets the left side of the equation.
                string leftSide;

                // If the operation is at the start of the string.
                if(opIndex > 0) // Valid operation.
                {
                    // Gets the left side.
                    leftSide = equation.Substring(0, opIndex);
                }
                else
                {
                    // If the operation is positive or negative, then it's valid (e.g., +12, -3).
                    // Just put a 0 on the left side.
                    if(operation == "+" || operation == "-")
                    {
                        leftSide = "0";
                    }
                    else // Invalid, so leave left side blank.
                    {
                        leftSide = "";
                    }
                }

                // RIGHT SIDE
                // Gets the right side of the equation.
                string rightSide;

                // If the operation is at the end of the string.
                if (opIndex < equation.Length) // Valid operation.
                {
                    // Gets the right side after the operation.
                    rightSide = equation.Substring(opIndex + 1);
                }
                else
                {
                    // If the operation is at the end of the string, it is invalid.
                    rightSide = "";
                }

                // If either of the strings are empty, then the equation is invalid.
                if(leftSide == string.Empty || rightSide == string.Empty)
                {
                    // Return an empty string.
                    return string.Empty;
                }
                else // Equation may be valid.
                {
                    // Recursively call the function to see if the numbers are valid.
                    leftSide = RunStringMathCalculationRecursive(leftSide);
                    rightSide = RunStringMathCalculationRecursive(rightSide);

                    // If both are valid, perform the calculation and return the result.
                    if(leftSide != string.Empty && rightSide != string.Empty)
                    {
                        // The values.
                        float value1;
                        float value2;

                        // Tries to parse both values.
                        if(float.TryParse(leftSide, out value1) && float.TryParse(rightSide, out value2))
                        {
                            // Converts the operation to a char for ease of checking.
                            char opChar = operation[0];

                            // The result to be saved.
                            float result = 0;

                            // Checks the operation character.
                            switch(opChar)
                            {
                                case '^': // Exponent
                                    result = Mathf.Pow(value1, value2);
                                    break;

                                case '/': // Division
                                    // If this would result in division by 0, return an empty string.
                                    if(value2 == 0)
                                    {
                                        result = 0;
                                        return string.Empty;
                                    }
                                    else
                                    {
                                        // Safe division.
                                        result = value1 / value2;
                                    }

                                    break;

                                case '*': // Multiplication
                                    result = value1 * value2;
                                    break;

                                case '+': // Addition
                                    result = value1 + value2;
                                    break;

                                case '-': // Subtraction
                                    result = value1 - value2;
                                    break;
                            }

                            // Returns the result as a string.
                            string resultStr = result.ToString();
                            return resultStr;
                        }
                        else // Parsing failed.
                        {
                            return string.Empty;
                        }
                    }
                    else // At least one of them are invalid, so return the empty string.
                    {
                        return string.Empty;
                    }
                }
            }
            else // No operations.
            {
                // The value that's gotten by parsing.
                float value;

                // The value string.
                string valueStr;

                // Tries to turn the equation into a float.
                if(float.TryParse(equation, out value)) // Success
                {
                    valueStr = value.ToString();
                }
                else // Failure
                {
                    valueStr = string.Empty; // Empty string.
                }

                // Returns the string.
                return valueStr;
            }
        }

        // Checks if the provided string contains one of the operation symbols.
        private static bool ContainsMathOperationSymbol(string str)
        {
            // The result.
            bool result;

            // Checks for BEDMAS symbols.
            if (str.Contains("(") || str.Contains(")") || str.Contains("^") || 
                str.Contains("/") || str.Contains("*") || str.Contains("+") || str.Contains("-"))

            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}