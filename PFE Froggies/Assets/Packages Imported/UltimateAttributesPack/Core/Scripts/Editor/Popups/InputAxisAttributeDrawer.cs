using UnityEngine;
using UnityEditor;
using System;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(InputAxisAttribute))]
    public class InputAxisAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if(!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(InputAxisAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with string or int)");
                return;
            }

            // Get input axis array
            string[] inputAxis = GetInputAxis(property);

            // Get current axis index
            int index = 0;
            if(property.propertyType == SerializedPropertyType.String)
                index = Mathf.Clamp(Array.IndexOf(inputAxis, property.stringValue), 0, inputAxis.Length - 1);
            else if(property.propertyType == SerializedPropertyType.Integer)
                index = Mathf.Clamp(property.intValue, 0, inputAxis.Length - 1);

            // Draw popup and get new index selected
            int newIndex = EditorGUI.Popup(position, label.text, index, inputAxis);
            string newAxis = inputAxis[newIndex];

            // If property is a string and axis selected has changed, set new
            if (property.propertyType == SerializedPropertyType.String && property.stringValue?.Equals(newAxis, StringComparison.Ordinal) == false)
                property.stringValue = inputAxis[newIndex];
            // If property is a int and axis selected has changed, set new
            else if (property.propertyType == SerializedPropertyType.Integer && property.intValue.Equals(newIndex) == false)
                property.intValue = newIndex;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        string[] GetInputAxis(SerializedProperty property)
        {
            // Get input axis in input manager
            var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            if (inputManager == null) // If input manager not found, log warning and return
            {
                Debug.LogWarning($"{nameof(InputAxisAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Input Manager not found (ProjectSettings/InputManager.asset)");
                return null;
            }
            SerializedObject obj = new SerializedObject(inputManager);
            SerializedProperty axisArray = obj.FindProperty("m_Axes");
            if (axisArray.arraySize == 0) // Log warning and return if no axis
            {
                Debug.LogWarning($"{nameof(InputAxisAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : InputAxis list of input manager is empty");
                return null;
            }

            // Get input axis names
            string[] axisNames = new string[axisArray.arraySize];
            for (int i = 0; i < axisArray.arraySize; i++)
            {
                axisNames[i] = axisArray.GetArrayElementAtIndex(i).FindPropertyRelative("m_Name").stringValue;
            }

            return axisNames;
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    return true;
                case SerializedPropertyType.Integer:
                    return true;
                default:                    
                    return false;
            }
        }
    }
}