using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class ChangeLabelAttribute : PropertyAttribute
    {
        public readonly string NewLabel;

        /// <summary>
        /// Changes the label of the variable
        /// </summary>
        /// <param name="newLabel">The new label of the variable</param>
        public ChangeLabelAttribute(string newLabel)
        {
            NewLabel = newLabel;
        }
    }
}