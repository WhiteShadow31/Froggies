using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class SuffixAttribute : PropertyAttribute
    {
        public readonly string SuffixText;

        /// <summary>
        /// Displays a suffix text at the right of the variable in inspector. 
        /// </summary>
        /// <param name="suffixText">The suffix text</param>
        public SuffixAttribute(string suffixText)
        {
            SuffixText = suffixText;
        }
    }
}