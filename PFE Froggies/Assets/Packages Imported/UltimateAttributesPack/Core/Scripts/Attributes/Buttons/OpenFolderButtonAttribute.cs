using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class OpenFolderButtonAttribute : PropertyAttribute
    {
        readonly public string Text;
        readonly public string FolderPath;

        /// <summary>
        /// Displays a button in the inspector to open a specific project folder or file. 
        /// </summary>
        /// <param name="text">The text that will be displayed on the button</param>
        /// <param name="folderPath">The path of the folder or the file</param>
        public OpenFolderButtonAttribute(string text, string folderPath)
        {
            Text = text;
            FolderPath = folderPath;
        }
    }
}