using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ProgressBarAttribute : PropertyAttribute
    {
        readonly public string Name;
        readonly public float Min;
        readonly public float Max;
        readonly public float Value;

        /// <summary>
        /// <param name="name"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
        /// Shows a progress bar in the inspector with min, max and value.
        /// <br></br>
        /// It's just visual, it's can't be refreshed in edit mode.
        /// </summary>
        public ProgressBarAttribute(string name, float min, float max, float value)
        {
            Name = name;
            Min = min;
            Max = max;
            Value = value;
        }
    }
}