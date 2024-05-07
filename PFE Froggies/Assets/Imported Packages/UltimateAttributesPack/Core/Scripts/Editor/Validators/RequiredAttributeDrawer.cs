using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredAttributeDrawer : PropertyDrawer
    {
        float errorBoxHeight = 30f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property.propertyType))
            {
                Debug.LogWarning($"{nameof(RequiredAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with string, object reference, exposed reference and managed reference)");
                return;
            }

            if (IsNull(property)) // If property is empty, draw error text box
            {
                // Draw error text box
                Rect errorTextBoxRect = new Rect(position.xMin, position.yMin + 3, position.width, errorBoxHeight);
                EditorGUI.HelpBox(errorTextBoxRect, property.name + " is required", MessageType.Error);

                // Draw property
                Rect propertyRect = new Rect(position.xMin, position.yMin + errorBoxHeight + 7, position.width, EditorGUI.GetPropertyHeight(property));
                EditorGUI.PropertyField(propertyRect, property, label);

                // Get current attribute
                RequiredAttribute currentAttribute = attribute as RequiredAttribute;

                // Log error
                if(currentAttribute.LogError)
                    Debug.LogError(property.name + " is required in " + property.serializedObject.targetObject.name + "." + (property.serializedObject.targetObject as MonoBehaviour).GetType());
            }
            else // If property isn't empty, draw property
            {
                // Draw property
                Rect propertyRect = new Rect(position.xMin, position.yMin, position.width, position.height);
                EditorGUI.PropertyField(propertyRect, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsNull(property))
                return errorBoxHeight + EditorGUI.GetPropertyHeight(property) + 7;
            else
                return EditorGUI.GetPropertyHeight(property);
        }

        bool IsNull(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    return string.IsNullOrEmpty(property.stringValue);
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue == null;
                case SerializedPropertyType.ExposedReference:
                    return property.exposedReferenceValue == null;
#if UNITY_2021_2_OR_NEWER
                case SerializedPropertyType.ManagedReference:
                    return property.managedReferenceValue == null;
#endif
            }

            return false;
        }

        bool IsTypeSupported(SerializedPropertyType type)
        {
            switch (type)
            {
                case SerializedPropertyType.String:
                    return true;
                case SerializedPropertyType.ObjectReference:
                    return true;
                case SerializedPropertyType.ExposedReference:
                    return true;
#if UNITY_2021_2_OR_NEWER
                case SerializedPropertyType.ManagedReference:
                    return true;
#endif
            }

            return false;
        }
    }
}