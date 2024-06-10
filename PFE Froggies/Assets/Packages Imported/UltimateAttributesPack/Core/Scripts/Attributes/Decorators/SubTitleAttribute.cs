using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class SubTitleAttribute : PropertyAttribute
    {
        public readonly string Title;
        public readonly string TextColor;
        public readonly string BackgroundColor;
        public readonly string IconPath;

        /// <summary>
        /// Displays a subtitle in the inspector. <br></br>
        /// Available colors : "black", "blue", "cyan", "green", "grey", "magenta", "red", "white", "yellow", "orange", "pink", "purple", "light green", "light blue", "brown", "dark blue". <br></br>
        /// Hexadecimal also works for color parameter.
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="textColor">The color of the title</param>
        /// <param name="backgroundColor">The color of the background</param>
        /// <param name="iconPath">The path of the icon that will be display at the left of the title</param>
        public SubTitleAttribute(string title, string textColor = "", string backgroundColor = "", string iconPath = "")
        {
            Title = title;
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            IconPath = iconPath;
        }
    }
}