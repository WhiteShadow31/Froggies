using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            ShowIfAttribute currentAttribute = attribute as ShowIfAttribute;

            // If condition is true, show property
            if (IsConditionTrue(property, currentAttribute.ComparedVariableName, currentAttribute.ComparisonType, currentAttribute.ConditionValue))
            {
                // Draw property
                EditorGUI.PropertyField(position, property, label);
            }
            // Else if readonly
            else if (currentAttribute.DisablingType == DisablingType.ReadOnly)
            {
                // Save GUI state and enable readonly
                bool savedGUIState = GUI.enabled;
                GUI.enabled = false;

                // Draw property
                EditorGUI.PropertyField(position, property, label);

                // Reset GUI state
                GUI.enabled = savedGUIState;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            ShowIfAttribute currentAttribute = attribute as ShowIfAttribute;

            if (!IsConditionTrue(property, currentAttribute.ComparedVariableName, currentAttribute.ComparisonType, currentAttribute.ConditionValue) && currentAttribute.DisablingType == DisablingType.DontShow)
                return 0;
            else
                return EditorGUI.GetPropertyHeight(property);
        }

        bool IsConditionTrue(SerializedProperty property, string comparedVariableName, ComparisonType comparisonType, object conditionValue)
        {
            // Check if compared variable is correct
            SerializedProperty comparedVariable = property.serializedObject.FindProperty(comparedVariableName);

            if (comparedVariable == null)
            {
                // Log warning if compared variable is not found
                Debug.LogWarning($"{nameof(ShowIfAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Wrong compared variable name or doesn't exist ({comparedVariableName}");
                return false;
            }

            // If it's a boolean comparison (bool or enum)
            if (comparedVariable.propertyType == SerializedPropertyType.Boolean || comparedVariable.propertyType == SerializedPropertyType.Enum)
            {
                // If comparison type is not Equals, log warning and return
                if(comparisonType != ComparisonType.Equals && comparisonType != ComparisonType.NotEqual)
                {
                    Debug.LogWarning($"{nameof(ShowIfAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Comparison type must be Equals or NotEquals to work with boolean comparison");
                    return true;
                }

                // If compared variable type and condition value type are not the same, log warning and return
                if((comparedVariable.propertyType == SerializedPropertyType.Boolean && !(conditionValue is bool)) || (comparedVariable.propertyType == SerializedPropertyType.Enum && conditionValue is bool))
                {
                    Debug.LogWarning($"{nameof(ShowIfAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Compared variable type and condition value type must be the same to work");
                    return true;
                }

                if(comparedVariable.propertyType == SerializedPropertyType.Boolean) // If it's a boolean, compare boolean value
                {
                    if (comparisonType == ComparisonType.Equals && comparedVariable.boolValue.Equals(conditionValue)) // Equal comparison
                        return true;
                    if (comparisonType == ComparisonType.NotEqual && !comparedVariable.boolValue.Equals(conditionValue)) // Not equal comparison
                        return true;

                        return false;
                }
                else if (comparedVariable.propertyType == SerializedPropertyType.Enum) // If it's an enum, compare enum value
                { 
                    if (comparisonType == ComparisonType.Equals && comparedVariable.enumValueIndex.Equals((int)conditionValue)) // Equal comparison
                        return true;
                    if (comparisonType == ComparisonType.NotEqual && !comparedVariable.enumValueIndex.Equals((int)conditionValue)) // Not equal comparison
                        return true;

                        return false;
                }
                return true;
            }
            // Else, it's a numeric comparison
            else
            {
                switch (comparedVariable.propertyType)
                {
                    case SerializedPropertyType.Float:
                        if(!(conditionValue is float || conditionValue is int)) // If values cannot be compared, log warning and return
                        {
                            Debug.LogWarning($"{nameof(ShowIfAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Condition value must be a float or an integer to work with float compared variable");
                            return true;
                        }
                        if (comparisonType == ComparisonType.GreaterThan) return comparedVariable.floatValue > (conditionValue is float ? (float)conditionValue : (int)conditionValue);
                        if (comparisonType == ComparisonType.GreaterOrEqual) return comparedVariable.floatValue >= (conditionValue is float ? (float)conditionValue : (int)conditionValue);
                        if (comparisonType == ComparisonType.Equals) return comparedVariable.floatValue == (conditionValue is float ? (float)conditionValue : (int)conditionValue);
                        if (comparisonType == ComparisonType.NotEqual) return comparedVariable.floatValue != (conditionValue is float ? (float)conditionValue : (int)conditionValue);
                        if (comparisonType == ComparisonType.SmallerOrEqual) return comparedVariable.floatValue <= (conditionValue is float ? (float)conditionValue : (int)conditionValue);
                        if (comparisonType == ComparisonType.SmallerThan) return comparedVariable.floatValue < (conditionValue is float ? (float)conditionValue : (int)conditionValue);
                        return true;
                    case SerializedPropertyType.Integer:
                        if (!(conditionValue is float || conditionValue is int)) // If values cannot be compared, log warning and return
                        {
                            Debug.LogWarning($"{nameof(ShowIfAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Condition value must be a float or an integer to work with int compared variable");
                            return true;
                        }
                        if (comparisonType == ComparisonType.GreaterThan) return comparedVariable.intValue > (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        if (comparisonType == ComparisonType.GreaterOrEqual) return comparedVariable.intValue >= (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        if (comparisonType == ComparisonType.Equals) return comparedVariable.intValue == (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        if (comparisonType == ComparisonType.NotEqual) return comparedVariable.intValue != (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        if (comparisonType == ComparisonType.SmallerOrEqual) return comparedVariable.intValue <= (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        if (comparisonType == ComparisonType.SmallerThan) return comparedVariable.intValue < (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        return true;
                    case SerializedPropertyType.String:
                        if (!(conditionValue is float || conditionValue is int || conditionValue is string)) // If values cannot be compared, log warning and return
                        {
                            Debug.LogWarning($"{nameof(ShowIfAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Condition value must be a float, an integer or a string to work with string compared variable");
                            return true;
                        }
                        if (comparisonType == ComparisonType.GreaterThan)
                        {
                            if (conditionValue is string)
                                return comparedVariable.stringValue.Length > (conditionValue as string).Length;
                            else
                                return comparedVariable.stringValue.Length > (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        }
                        if (comparisonType == ComparisonType.GreaterOrEqual)
                        {
                            if (conditionValue is string)
                                return comparedVariable.stringValue.Length >= (conditionValue as string).Length;
                            else
                                return comparedVariable.stringValue.Length >= (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        }
                        if (comparisonType == ComparisonType.Equals)
                        {
                            if (conditionValue is string) // If condition is a string, check if it's the same as compared variable
                                return comparedVariable.stringValue == conditionValue as string;
                            else // Else, check if condition value is the same as string lenght
                                return comparedVariable.stringValue.Length == (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        }
                        if (comparisonType == ComparisonType.NotEqual)
                        {
                            if (conditionValue is string) // If condition is a string, check if it's not the same as compared variable
                                return comparedVariable.stringValue != conditionValue as string;
                            else // Else, check if condition value is the same as string lenght
                                return comparedVariable.stringValue.Length != (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        }
                        if (comparisonType == ComparisonType.SmallerOrEqual)
                        {
                            if(conditionValue is string)
                                return comparedVariable.stringValue.Length <= (conditionValue as string).Length;
                            else
                                return comparedVariable.stringValue.Length <= (conditionValue is float ? Mathf.FloorToInt((float) conditionValue) : (int)conditionValue);
                        }
                        if (comparisonType == ComparisonType.SmallerThan)
                        {
                            if (conditionValue is string)
                                return comparedVariable.stringValue.Length < (conditionValue as string).Length;
                            else
                                return comparedVariable.stringValue.Length < (conditionValue is float ? Mathf.FloorToInt((float)conditionValue) : (int)conditionValue);
                        }
                        return true;
                    default:
                        // If compared variable type is not supported, log warning and return
                        Debug.LogWarning($"{nameof(ShowIfAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Compared variable type ({comparedVariable.propertyType}) is not supported (only work with float, int and string)");
                        return true;
                }             
            }
        }
    }
}