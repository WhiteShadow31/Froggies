using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class SceneAttribute : PropertyAttribute
    {
        /// <summary>
        /// Displays a popup menu in inspector to choose a scene (works string and int).
        /// </summary>
        public SceneAttribute()
        {

        }
    }
}