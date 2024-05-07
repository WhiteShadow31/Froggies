using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class LineTitleAttribute : PropertyAttribute
    {
        readonly public string Title;
        readonly public string LineColor = "white";
        readonly public string TitleColor = "white";

        /// <summary>
        /// <param name="title"></param>
        /// <param name="lineColor"></param>
        /// <param name="titleColor"></para>
        /// Displays a line title in the inspector. 
        /// <br></br>
        /// Available colors : "black", "blue", "cyan", "green", "grey", "magenta", "red", "white", "yellow", "orange", "pink", "purple", "light green", "light blue", "brown", "dark blue".
        /// <br></br>
        /// Hexadecimal also works for color parameter.
        /// </summary>
        public LineTitleAttribute(string title, string lineColor, string titleColor)
        {
            Title = title;
            LineColor = lineColor;
            TitleColor = titleColor;
        }
    }
}