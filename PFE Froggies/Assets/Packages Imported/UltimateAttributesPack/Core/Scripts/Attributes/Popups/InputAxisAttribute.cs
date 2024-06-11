using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class InputAxisAttribute : PropertyAttribute
    {
        /// <summary>
        /// Displays a popup menu in the inspector to select an input axis of the input manager (works with string and int).
        /// </summary>
        public InputAxisAttribute()
        {

        }
    }

}