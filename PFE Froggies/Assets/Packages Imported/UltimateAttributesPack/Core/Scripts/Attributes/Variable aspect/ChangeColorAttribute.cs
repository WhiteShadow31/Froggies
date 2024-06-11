using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class ChangeColorAttribute : PropertyAttribute
    {
        readonly public string TextColor;
        readonly public string BackgroundColor;

        /// <summary>
        /// Changes the text and background colors of the variable in the inspector. <br></br>
        /// Available colors : "black", "blue", "cyan", "green", "grey", "magenta", "red", "white", "yellow", "orange", "pink", "purple", "light green", "light blue", "brown", "dark blue". <br></br>
        /// Hexadecimal also works for color parameter.
        /// </summary>
        /// <param name="textColor">The new text color of the variable</param>
        /// <param name="backgroundColor">The new background color of the variable</param>
        public ChangeColorAttribute(string textColor, string backgroundColor = "")
        {
            TextColor = textColor;
            BackgroundColor = backgroundColor;
        }
    }
}