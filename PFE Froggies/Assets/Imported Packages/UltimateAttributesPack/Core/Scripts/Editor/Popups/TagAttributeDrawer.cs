using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if(!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(TagAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with string and int)");
                return;
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.String:

                    // Draw tag field and set value
                    property.stringValue = EditorGUI.TagField(position, label, property.stringValue);

                    return;
                default:
                    return;
            }      
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    return true;
                default:
                    return false;
            }
        }
    }
}