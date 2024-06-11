using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class TagAttribute : PropertyAttribute
    {
        /// <summary>
        /// Displays a popup menu in inspector to choose a tag (works with string).
        /// </summary>
        public TagAttribute()
        {

        }
    }
}