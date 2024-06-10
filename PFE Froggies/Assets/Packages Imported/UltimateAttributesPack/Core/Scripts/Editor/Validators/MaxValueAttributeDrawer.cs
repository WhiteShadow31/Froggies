using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(MaxValueAttribute))]
    public class MaxValueAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(MaxValueAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with float and int)");
                return;
            }

            // Get current attribute
            MaxValueAttribute currentAttribute = attribute as MaxValueAttribute;

            EditorGUI.BeginChangeCheck();

            // Draw property
            EditorGUI.PropertyField(position, property, label);

            // Clamp value to max
            if (EditorGUI.EndChangeCheck())
            {
                if(property.propertyType == SerializedPropertyType.Float)
                {
                    if (property.floatValue > currentAttribute.MaxValue)
                        property.floatValue = currentAttribute.MaxValue;
                }
                else if(property.propertyType == SerializedPropertyType.Integer)
                {
                    if (property.intValue > currentAttribute.MaxValue)
                        property.intValue = (int)currentAttribute.MaxValue;
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