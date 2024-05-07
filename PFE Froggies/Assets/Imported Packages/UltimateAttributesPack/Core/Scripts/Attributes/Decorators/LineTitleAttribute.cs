using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class LineTitleAttribute : PropertyAttribute
    {
        public readonly string Title;
        public readonly string TextColor;
        public readonly string LineColor;

        /// <summary>
        /// Displays a line title in the inspector. <br></br>
        /// Available colors : "black", "blue", "cyan", "green", "grey", "magenta", "red", "white", "yellow", "orange", "pink", "purple", "light green", "light blue", "brown", "dark blue". <br></br>
        /// Hexadecimal also works for color parameter.
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="textColor">The color of the title</param>
        /// <param name="lineColor">The color of the lines</param>
        public LineTitleAttribute(string title, string textColor = "", string lineColor = "")
        {
            Title = title;
            TextColor = textColor;
            LineColor = lineColor;
        }
    }
}