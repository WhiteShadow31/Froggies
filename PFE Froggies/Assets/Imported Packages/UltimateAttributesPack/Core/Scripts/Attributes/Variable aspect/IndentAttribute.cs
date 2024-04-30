using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class IndentAttribute : PropertyAttribute
    {
        public readonly int IndentLevel;

        /// <summary>
        /// Indent the variable in the inspector to the target indent level
        /// </summary>
        /// <param name="indentLevel">The target indent level</param>
        public IndentAttribute(int indentLevel)
        {
            IndentLevel = indentLevel;
        }
    }
}