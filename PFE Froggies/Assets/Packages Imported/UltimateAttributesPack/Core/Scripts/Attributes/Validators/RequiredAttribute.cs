using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredAttribute : PropertyAttribute
    {
        public readonly bool LogError;

        /// <summary>
        /// Displays an error text box in the inspector if the variable is empty (works with string, object reference, exposed reference and managed reference). 
        /// </summary>
        /// <param name="logError">Is an error logged if the variable is empty</param>
        public RequiredAttribute(bool logError = false)
        {
            LogError = logError;
        }
    }
}