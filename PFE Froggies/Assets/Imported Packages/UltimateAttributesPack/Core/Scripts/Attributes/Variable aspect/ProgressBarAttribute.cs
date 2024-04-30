using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class ProgressBarAttribute : PropertyAttribute
    {
        readonly public string Text;
        readonly public float Min;
        readonly public float Max;
        readonly public bool DisplayPercent;

        /// <summary>
        /// Displays a progress bar in the inspector with min and max values, and based on variable value (works with float and int). <br></br>
        /// Works on float or int variable.
        /// </summary>
        /// <param name="text">The text displayed on the progress bar</param>
        /// <param name="min">The minimum value of the progress bar</param>
        /// <param name="max">The maximum value of the progress bar</param>
        /// <param name="displayPercent">Is the percent displayed after the text of the progress bar</param>
        public ProgressBarAttribute(string text, float min, float max, bool displayPercent = false)
        {
            Text = text;
            Min = min;
            Max = max;
            DisplayPercent = displayPercent;
        }
    }
}