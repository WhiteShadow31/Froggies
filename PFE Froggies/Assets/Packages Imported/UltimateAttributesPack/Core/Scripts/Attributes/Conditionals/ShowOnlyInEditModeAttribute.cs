using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class ShowOnlyInEditModeAttribute : PropertyAttribute
    {
        public readonly DisablingType DisablingType;

        /// <summary>
        /// Shows only the variable in edit mode. 
        /// </summary>
        /// <param name="disablingType">The disabling type of the variable</param>
        public ShowOnlyInEditModeAttribute(DisablingType disablingType = DisablingType.DontShow)
        {
            DisablingType = disablingType;
        }
    }
}