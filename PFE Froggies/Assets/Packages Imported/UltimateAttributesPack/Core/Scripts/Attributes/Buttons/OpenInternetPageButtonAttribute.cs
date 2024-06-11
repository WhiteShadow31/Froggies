using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class OpenInternetPageButtonAttribute : PropertyAttribute
    {
        readonly public string Text;
        readonly public string Link;

        /// <summary>
        /// Displays a button in the inspector to open a specific internet page. 
        /// </summary>
        /// <param name="text">The text that will be displayed on the button</param>
        /// <param name="link">The link of the internet page</param>
        public OpenInternetPageButtonAttribute(string text, string link)
        {
            Text = text;
            Link = link;
        }
    }
}