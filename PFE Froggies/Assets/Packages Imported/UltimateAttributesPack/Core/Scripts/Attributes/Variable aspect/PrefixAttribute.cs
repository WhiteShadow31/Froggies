using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class PrefixAttribute : PropertyAttribute
    {
        public readonly string PrefixText;

        /// <summary>
        /// Displays a prefix text at the left of the variable in inspector. 
        /// </summary>
        /// <param name="prefixText">The prefix text</param>
        public PrefixAttribute(string prefixText)
        {
            PrefixText = prefixText;
        }
    }
}