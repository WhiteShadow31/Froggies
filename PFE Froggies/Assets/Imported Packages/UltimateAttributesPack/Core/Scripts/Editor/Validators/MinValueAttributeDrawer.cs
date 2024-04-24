using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(MinValueAttribute))]
    public class MinValueAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(MinValueAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with float and int)");
                return;
            }

            // Get current attribute
            MinValueAttribute currentAttribute = attribute as MinValueAttribute;

            EditorGUI.BeginChangeCheck();

            // Draw property
            EditorGUI.PropertyField(position, property, label);

            // Clamp value to min
            if (EditorGUI.EndChangeCheck())
            {
                if(property.propertyType == SerializedPropertyType.Float)
                {
                    if (property.floatValue < currentAttribute.MinValue)
                        property.floatValue = currentAttribute.MinValue;
                }
                else if(property.propertyType == SerializedPropertyType.Integer)
                {
                    if (property.intValue < currentAttribute.MinValue)
                        property.intValue = (int)currentAttribute.MinValue;
                }
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
                default:
                    return false;
            }
        }
    }
}