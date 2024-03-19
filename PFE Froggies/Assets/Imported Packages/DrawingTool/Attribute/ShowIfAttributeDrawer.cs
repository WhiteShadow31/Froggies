using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DrawingTool.Attribute
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        ShowIfAttribute showIfAttribute;

        // Set total spacing
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShowMe(property) && showIfAttribute.disablingType == DisablingType.DontShow)
                return 0f;

            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShowMe(property)) // If condition is true, show property
            {
                Rect propertyRect = new Rect(position.xMin, position.yMin, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(propertyRect, property, label);
            }
            else if (showIfAttribute.disablingType == DisablingType.ReadOnly) // If readonly
            {
                GUI.enabled = false;
                Rect propertyRect = new Rect(position.xMin, position.yMin, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(propertyRect, property, label);
                GUI.enabled = true;
            }
        }

        private bool ShowMe(SerializedProperty property)
        {
            showIfAttribute = attribute as ShowIfAttribute;

            string path = property.propertyPath.Contains(".") ? System.IO.Path.ChangeExtension(property.propertyPath, showIfAttribute.comparedVariableName) : showIfAttribute.comparedVariableName;
            SerializedProperty comparedField = property.serializedObject.FindProperty(path);

            if (comparedField == null)
            {
                Debug.LogWarning("ShowIfAttribute : Wrong compared variable name or doesn't exist (" + showIfAttribute.comparedVariableName + ") in " + property.serializedObject.targetObject.name + " ,or attribute set on invalid property (lists, arrays).");
                return true;
            }

            switch (comparedField.type)
            {
                case "bool":
                    if (showIfAttribute.comparedValue is bool)
                    {
                        switch (showIfAttribute.comparisonType)
                        {
                            case ComparisonType.Equals:
                                return comparedField.boolValue.Equals(showIfAttribute.comparedValue);
                            case ComparisonType.NotEqual:
                                return !comparedField.boolValue.Equals(showIfAttribute.comparedValue);
                            default:
                                return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                case "Enum":
                    if (showIfAttribute.comparedValue is Enum)
                    {
                        switch (showIfAttribute.comparisonType)
                        {
                            case ComparisonType.Equals:
                                return comparedField.enumValueIndex.Equals((int)showIfAttribute.comparedValue);
                            case ComparisonType.NotEqual:
                                return !comparedField.enumValueIndex.Equals((int)showIfAttribute.comparedValue);
                            default:
                                return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                case "float":
                    if (showIfAttribute.comparedValue is float)
                    {
                        switch (showIfAttribute.comparisonType)
                        {
                            case ComparisonType.GreaterThan:
                                if (comparedField.floatValue > (float)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.GreaterOrEqual:
                                if (comparedField.floatValue >= (float)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.Equals:
                                if (comparedField.floatValue == (float)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.NotEqual:
                                if (comparedField.floatValue != (float)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerOrEqual:
                                if (comparedField.floatValue <= (float)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerThan:
                                if (comparedField.floatValue < (float)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            default:
                                return true;
                        }
                    }
                    else if (showIfAttribute.comparedValue is int)
                    {
                        switch (showIfAttribute.comparisonType)
                        {
                            case ComparisonType.GreaterThan:
                                if (comparedField.floatValue > (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.GreaterOrEqual:
                                if (comparedField.floatValue >= (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.Equals:
                                if (comparedField.floatValue == (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.NotEqual:
                                if (comparedField.floatValue != (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerOrEqual:
                                if (comparedField.floatValue <= (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerThan:
                                if (comparedField.floatValue < (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            default:
                                return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                case "int":
                    if (showIfAttribute.comparedValue is int)
                    {
                        switch (showIfAttribute.comparisonType)
                        {
                            case ComparisonType.GreaterThan:
                                if (comparedField.intValue > (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.GreaterOrEqual:
                                if (comparedField.intValue >= (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.Equals:
                                if (comparedField.intValue == (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.NotEqual:
                                if (comparedField.intValue != (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerOrEqual:
                                if (comparedField.intValue <= (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerThan:
                                if (comparedField.intValue < (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            default:
                                return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                case "string":
                    if (showIfAttribute.comparedValue is string)
                    {
                        switch (showIfAttribute.comparisonType)
                        {
                            case ComparisonType.GreaterThan:
                                if (comparedField.stringValue.Length > (int)showIfAttribute.comparedValue.ToString().Length)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.GreaterOrEqual:
                                if (comparedField.stringValue.Length >= (int)showIfAttribute.comparedValue.ToString().Length)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.Equals:
                                if (comparedField.stringValue.Equals((string)showIfAttribute.comparedValue))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.NotEqual:
                                if (!comparedField.stringValue.Equals((string)showIfAttribute.comparedValue))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerOrEqual:
                                if (comparedField.stringValue.Length <= (int)showIfAttribute.comparedValue.ToString().Length)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerThan:
                                if (comparedField.stringValue.Length < (int)showIfAttribute.comparedValue.ToString().Length)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            default:
                                return true;
                        }
                    }
                    else if (showIfAttribute.comparedValue is int)
                    {
                        switch (showIfAttribute.comparisonType)
                        {
                            case ComparisonType.GreaterThan:
                                if (comparedField.stringValue.Length > (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.GreaterOrEqual:
                                if (comparedField.stringValue.Length >= (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.Equals:
                                if (comparedField.stringValue.Length == (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.NotEqual:
                                if (comparedField.stringValue.Length != (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerOrEqual:
                                if (comparedField.stringValue.Length <= (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case ComparisonType.SmallerThan:
                                if (comparedField.stringValue.Length < (int)showIfAttribute.comparedValue)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            default:
                                return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return true;
            }
        }
    }

}

