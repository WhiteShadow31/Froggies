using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class OpenProjectFileButtonAttribute : PropertyAttribute
    {
        readonly public string Name;
        readonly public string Path;

        /// <summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// Displays a button in the inspector to open a specific project file. 
        /// </summary>
        public OpenProjectFileButtonAttribute(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}