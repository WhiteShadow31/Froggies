using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class SubTitleAttribute : PropertyAttribute
    {
        public readonly string Title;
        public readonly string BackgroundColor = "white";
        public readonly string TitleColor = "black";

        /// <summary>
        /// <param name="title"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="titleColor"></param>
        /// Displays a subtitle in the inspector. 
        /// <br></br>
        /// Available colors : "black", "blue", "cyan", "green", "grey", "magenta", "red", "white", "yellow", "orange", "pink", "purple", "light green", "light blue", "brown", "dark blue".
        /// <br></br>
        /// Hexadecimal also works for color parameter.
        /// </summary>
        public SubTitleAttribute(string title, string backgroundColor, string titleColor)
        {
            Title = title;
            BackgroundColor = backgroundColor;
            TitleColor = titleColor;
        }
    }
}