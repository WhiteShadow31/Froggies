using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class OpenInternetPageButtonAttribute : PropertyAttribute
    {
        readonly public string Name;
        readonly public string Link;

        /// <summary>
        /// <param name="name"></param>
        /// <param name="link"></param>
        /// Displays a button in the inspector to open a specific internet page. 
        /// </summary>
        public OpenInternetPageButtonAttribute(string name, string link)
        {
            Name = name;
            Link = link;
        }
    }
}