using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class AssetPreviewAttribute : PropertyAttribute
    {
        public readonly PreviewSize PreviewSize;

        /// <summary>
        /// Displays a preview of a variable if it have one. (Works with sprite, texture, texture2D, game object and mesh).
        /// </summary>
        /// <param name="previewSize">The size of the preview</param>
        public AssetPreviewAttribute(PreviewSize previewSize = PreviewSize.Medium)
        {
            PreviewSize = previewSize;
        }
    }

    public enum PreviewSize
    {
        Small,
        Medium,
        Large
    }
}