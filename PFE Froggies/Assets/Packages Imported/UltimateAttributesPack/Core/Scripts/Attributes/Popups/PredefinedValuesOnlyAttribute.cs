using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class PredefinedValuesOnlyAttribute : PropertyAttribute
    {
        public readonly object[] Values;

        /// <summary>
        /// Displays a popup menu to chose between predefined values (work with float, int and string).
        /// </summary>
        /// <param name="values">The list of predefined values that can be chosen to set variable</param>
        public PredefinedValuesOnlyAttribute(params object[] values)
        {
            Values = values;
        }
    }
}