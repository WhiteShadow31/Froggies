using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class MinValueAttribute : PropertyAttribute
    {
        public readonly float MinValue;

        /// <summary>
        /// Set a minimum value to the variable (works with float and int). 
        /// </summary>
        /// <param name="minValue">The minumum value of the variable</param>
        public MinValueAttribute(float minValue)
        {
            MinValue = minValue;
        }
    }
}