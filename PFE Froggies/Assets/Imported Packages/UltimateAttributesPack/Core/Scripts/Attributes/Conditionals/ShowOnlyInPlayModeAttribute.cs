using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class ShowOnlyInPlayModeAttribute : PropertyAttribute
    {
        public readonly DisablingType DisablingType;

        /// <summary>
        /// Shows only the variable in play mode. 
        /// </summary>
        /// <param name="disablingType">The disabling type of the variable</param>
        public ShowOnlyInPlayModeAttribute(DisablingType disablingType = DisablingType.DontShow)
        {
            DisablingType = disablingType;
        }
    }
}