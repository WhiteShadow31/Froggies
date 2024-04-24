using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class LineAttribute : PropertyAttribute
    {
        public readonly string Color;

        /// <summary>
        /// Displays a line in the inspector. <br></br>
        /// Available colors : "black", "blue", "cyan", "green", "grey", "magenta", "red", "white", "yellow", "orange", "pink", "purple", "light green", "light blue", "brown", "dark blue". <br></br>
        /// Hexadecimal also works for color parameter.
        /// </summary>
        /// <param name="color">The color of the line</param>
        public LineAttribute(string color = "")
        {
            Color = color;
        }
    }
}