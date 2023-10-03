using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class LineAttribute : PropertyAttribute
    {
        public readonly float Height = 2;
        public readonly float Spacing = 10;
        public readonly string Color = "white";

        /// <summary>
        /// <param name="color"></param>
        /// Displays a line in the inspector. 
        /// <br></br>
        /// Available colors : "black", "blue", "cyan", "green", "grey", "magenta", "red", "white", "yellow", "orange", "pink", "purple", "light green", "light blue", "brown", "dark blue".
        /// <br></br>
        /// Hexadecimal also works for color parameter.
        /// </summary>
        public LineAttribute(string color)
        {
            Color = color;
        }
    }
}