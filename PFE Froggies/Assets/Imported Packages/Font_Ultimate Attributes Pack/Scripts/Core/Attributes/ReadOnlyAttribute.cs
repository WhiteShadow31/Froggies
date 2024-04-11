using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = false)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        /// <summary>
        /// Draw property in readonly in the inspector.
        /// </summary>
        public ReadOnlyAttribute()
        {

        }
    }
}
