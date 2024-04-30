using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class RectTransformInfoAttribute : PropertyAttribute
    {
        public readonly bool CanModifyValues;

        /// <summary>
        /// Displays a foldout button at the right of the variable in the inspector to fold or unfold RectTransform values (works with RectTransform).
        /// </summary>
        /// <param name="canModifyValues">Are the values modifiable in foldout</param>
        public RectTransformInfoAttribute(bool canModifyValues = false)
        {
            CanModifyValues = canModifyValues;
        }
    }
}