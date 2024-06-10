using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class AddSubtractButtonsAttribute : PropertyAttribute
    {
        public readonly float SubtractValue;
        public readonly float AddValue;

        /// <summary>
        /// Displays buttons in the inspector to add or subtract the variable with determined values (works with float and int). 
        /// </summary>
        /// <param name="subtractValue">The value that will be subtracted to the variable</param>
        /// <param name="addValue">The value that will be added to the variable</param>
        public AddSubtractButtonsAttribute(float subtractValue, float addValue)
        {
            SubtractValue = subtractValue;
            AddValue = addValue;
        }
    }
}