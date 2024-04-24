using UnityEngine;
using UnityEditor;
using System;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(PredefinedValuesAttribute))]
    public class PredefinedValuesAttributeDrawer : PropertyDrawer
    {
        float buttonWidth = 20f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            PredefinedValuesAttribute currentAttribute = attribute as PredefinedValuesAttribute;

            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(PredefinedValuesAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with float, int and string)");
                return;
            }

            // Check if param values are correct, else log warning and return
            if (!ValuesCorrect(currentAttribute.Values, property))
            {
                Debug.LogWarning($"{nameof(PredefinedValuesAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : One or multiple param values are wrong (not the same type as variable)");
                return;
            }

            // Draw property
            Rect propertyRect = new Rect(position.xMin, position.yMin, position.width - buttonWidth - 5, position.height);
            EditorGUI.PropertyField(propertyRect, property, label);

            // Set popup rect
            Rect popupRect = new Rect(position.xMax - buttonWidth, position.yMin, buttonWidth, position.height);

            // Get values as string
            string[] valuesAsString = GetValuesAsString(currentAttribute.Values);

            switch (property.propertyType)
            {
                // If property is float
                case SerializedPropertyType.Float:
                    // Get float values
                    float[] floatValues = new float[valuesAsString.Length];
                    for(int i = 0; i < valuesAsString.Length; i++)
                    {
                        floatValues[i] = float.Parse(valuesAsString[i]);
                    }

                    int floatIndex = Mathf.Clamp(Array.IndexOf(floatValues, property.floatValue), 0, floatValues.Length - 1); // Get current index
                    int newFloatIndex = EditorGUI.Popup(popupRect, floatIndex, valuesAsString); // Draw popup and get new index selected

                    if (floatIndex != newFloatIndex) // If selected index has changed, set new value
                        property.floatValue = floatValues[newFloatIndex];
                    return;
                // If property is int
                case SerializedPropertyType.Integer:
                    // Get int values
                    int[] intValues = new int[valuesAsString.Length];
                    for (int i = 0; i < valuesAsString.Length; i++)
                    {
                        intValues[i] = int.Parse(valuesAsString[i]);
                    }

                    int intIndex = Mathf.Clamp(Array.IndexOf(intValues, property.intValue), 0, intValues.Length - 1); // Get current index
                    int newIntIndex = EditorGUI.Popup(popupRect, intIndex, valuesAsString); // Draw popup and get new index selected

                    if (intIndex != newIntIndex) // If selected index has changed, set new value
                        property.intValue = intValues[newIntIndex];
                    return;
                // If property is string
                case SerializedPropertyType.String:

                    int stringIndex = Mathf.Clamp(Array.IndexOf(valuesAsString, property.stringValue), 0, valuesAsString.Length - 1); // Get current index
                    int newStringIndex = EditorGUI.Popup(popupRect, stringIndex, valuesAsString); // Draw popup and get new index selected

                    if (stringIndex != newStringIndex) // If selected index has changed, set new value
                        property.stringValue = valuesAsString[newStringIndex];
                    return;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        // Get values as string
        string[] GetValuesAsString(object[] values)
        {            
            string[] valuesAsString = new string[values.Length];
            for(int i = 0; i < values.Length; i++)
            {
                valuesAsString[i] = values[i].ToString();
            }
            return valuesAsString;
        }

        // Check if values are correct
        bool ValuesCorrect(object[] values, SerializedProperty property)
        {           
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    foreach(object value in values)
                    {
                        if (value is not float && value is not int)
                            return false;
                    }
                    return true;
                case SerializedPropertyType.Integer:
                    foreach (object value in values)
                    {
                        if (value is not int)
                            return false;
                    }
                    return true;
                case SerializedPropertyType.String:
                    foreach (object value in values)
                    {
                        if (value is not string && string.IsNullOrEmpty(value.ToString()))
                            return false;
                    }
                    return true;
                default:                   
                    return false;
            }
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    return true;
                case SerializedPropertyType.Integer:
                    return true;
                case SerializedPropertyType.String:
                    return true;
                default:
                    return false;
            }
        }
    }
}