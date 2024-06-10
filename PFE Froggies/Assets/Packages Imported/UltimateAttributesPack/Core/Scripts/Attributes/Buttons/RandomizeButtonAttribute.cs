using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class RandomizeButtonAttribute : PropertyAttribute
    {
        readonly public float MinValue;
        readonly public float MaxValue;
        readonly public int Digits;

        /// <summary>
        /// Shows a button in inspector that randomize the variable between min and max values, and with a digits count (works with float and int). <br></br>
        /// Digits count can be between 0 and 5.
        /// </summary>
        /// <param name="minValue">The minimum value of the random</param>
        /// <param name="maxValue">The maximum value of the random</param>
        /// <param name="digits">The digits count of the random</param>
        public RandomizeButtonAttribute(float minValue, float maxValue, int digits)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Digits = digits;
        }
    }
}