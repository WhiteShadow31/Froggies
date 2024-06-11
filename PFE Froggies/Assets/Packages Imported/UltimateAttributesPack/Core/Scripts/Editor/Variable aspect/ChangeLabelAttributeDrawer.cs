using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(ChangeLabelAttribute))]
    public class ChangeLabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            ChangeLabelAttribute currentAttribute = attribute as ChangeLabelAttribute;

            // Draw property with new label
            EditorGUI.PropertyField(position, property, new GUIContent(currentAttribute.NewLabel));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}