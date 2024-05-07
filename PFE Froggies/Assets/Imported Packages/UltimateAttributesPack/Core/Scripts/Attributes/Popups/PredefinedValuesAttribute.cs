using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class PredefinedValuesAttribute : PropertyAttribute
    {
        public readonly object[] Values;

        /// <summary>
        /// Displays a button at the right of the variable to chose a predefined value (works with float, int and string).
        /// </summary>
        /// <param name="values">The list of predefined values that can be chosen to set variable</param>
        public PredefinedValuesAttribute(params object[] values)
        {
            Values = values;
        }
    }
}