using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class TransformInfoAttribute : PropertyAttribute
    {
        public readonly bool CanModifyValues;

        /// <summary>
        /// Displays a foldout button at the right of the variable in the inspector to fold or unfold Transform values (works with Transform).
        /// </summary>
        /// <param name="canModifyValues">Are the values modifiable in foldout</param>
        public TransformInfoAttribute(bool canModifyValues = false)
        {
            CanModifyValues = canModifyValues;
        }
    }
}