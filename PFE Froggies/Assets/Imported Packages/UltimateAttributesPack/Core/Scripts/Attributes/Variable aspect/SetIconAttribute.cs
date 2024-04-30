using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class SetIconAttribute : PropertyAttribute
    {
        public readonly string IconPath;
        public readonly IconSize IconSize;

        /// <summary>
        /// Displays an icon at the left of the variable in the inspector.
        /// </summary>
        /// <param name="iconPath">The icon path in the project</param>
        /// <param name="iconSize">The size of the image</param>
        public SetIconAttribute(string iconPath, IconSize iconSize = IconSize.Small)
        {
            IconPath = iconPath;
            IconSize = iconSize;
        }
    }

    public enum IconSize
    {
        Small,
        Medium,
        Large
    }
}