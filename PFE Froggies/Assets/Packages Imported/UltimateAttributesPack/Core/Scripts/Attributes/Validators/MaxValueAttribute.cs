using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class MaxValueAttribute : PropertyAttribute
    {
        public readonly float MaxValue;

        /// <summary>
        /// Set a maximum value to the variable (works with float and int). 
        /// </summary>
        /// <param name="maxValue">The maximum value of the variable</param>
        public MaxValueAttribute(float maxValue)
        {
            MaxValue = maxValue;
        }
    }
}