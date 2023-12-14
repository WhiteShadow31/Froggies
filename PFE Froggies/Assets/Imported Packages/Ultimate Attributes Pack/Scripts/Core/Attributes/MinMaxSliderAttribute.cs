using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public readonly float Min;
        public readonly float Max;

        /// <summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// Displays a MinMaxSlider for Vector2 in the inspector. 
        /// </summary>
        public MinMaxSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}