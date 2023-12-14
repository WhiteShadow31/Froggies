using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TextColorAttribute : PropertyAttribute
    {
        readonly public string Color;

        /// <summary>
        /// <param name="color"></param>
        /// Change property color.
        /// </summary>
        public TextColorAttribute(string color)
        {
            Color = color;
        }
    }
}