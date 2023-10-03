using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = false)]
    public class ShowIfAttribute : PropertyAttribute
    {
        readonly public string comparedVariableName;
        readonly public object comparedValue;
        readonly public ComparisonType comparisonType;
        readonly public DisablingType disablingType;

        /// <summary>
        /// <param name="comparedVariableName"></param>
        /// <param name="comparedValue"></param>
        /// <param name="comparisonType"></param>
        /// <param name="disablingType"></param>
        /// Shows property in inspector if condition set with ComparisonType parameter is true between compared variable value and compared value.
        /// <br></br>
        /// Conditions works with bool, enum, float, int and string.
        /// <br></br>
        /// /!\ Do not use on lists or arrays. /!\
        /// </summary>
        public ShowIfAttribute(string comparedVariableName, object comparedValue, ComparisonType comparisonType = ComparisonType.Equals, DisablingType disablingType = DisablingType.DontShow)
        {
            this.comparedVariableName = comparedVariableName;
            this.comparedValue = comparedValue;
            this.comparisonType = comparisonType;
            this.disablingType = disablingType;
        }
    }

    public enum ComparisonType
    {
        Equals,
        NotEqual,
        GreaterThan,
        SmallerThan,
        SmallerOrEqual,
        GreaterOrEqual
    }

    public enum DisablingType
    {
        ReadOnly,
        DontShow
    }
}