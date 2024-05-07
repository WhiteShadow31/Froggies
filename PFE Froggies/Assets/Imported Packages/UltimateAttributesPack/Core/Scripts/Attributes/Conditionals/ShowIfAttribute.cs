using UnityEngine;

namespace UltimateAttributesPack
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class ShowIfAttribute : PropertyAttribute
    {
        readonly public string ComparedVariableName;
        readonly public object ConditionValue;
        readonly public ComparisonType ComparisonType;
        readonly public DisablingType DisablingType;

        /// <summary>
        /// Shows variable in inspector if the condition set with ComparisonType parameter is true between compared variable value and condition value. <br></br>
        /// Condition value and compared variable works with bool, enum, float, int and string.
        /// </summary>
        /// <param name="comparedVariableName">The name of the variable that will be compared with condition value </param>
        /// <param name="conditionValue">The condition value to display variable</param>
        /// <param name="comparisonType">The comparison type</param>
        /// <param name="disablingType">The disabling type if the condition returns false</param>
        public ShowIfAttribute(string comparedVariableName, object conditionValue, ComparisonType comparisonType = ComparisonType.Equals, DisablingType disablingType = DisablingType.DontShow)
        {            
            ComparedVariableName = comparedVariableName;
            ConditionValue = conditionValue;
            ComparisonType = comparisonType;
            DisablingType = disablingType;
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