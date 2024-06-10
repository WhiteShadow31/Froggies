using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        /// <summary>
        /// Displays the variable in readonly in the inspector.
        /// </summary>
        public ReadOnlyAttribute()
        {

        }
    }
}
