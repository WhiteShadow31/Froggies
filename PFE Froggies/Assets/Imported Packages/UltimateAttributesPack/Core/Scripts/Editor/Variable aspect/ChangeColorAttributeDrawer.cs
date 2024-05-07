using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    using UltimateAttributesPack.Utility;

    [CustomPropertyDrawer(typeof(ChangeColorAttribute))]
    public class ChangeColorAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            ChangeColorAttribute currentAttribute = attribute as ChangeColorAttribute;

            // Change text color
            Color baseTextColor = GUI.color;
            GUI.contentColor = ColorUtility.GetColorByName(currentAttribute.TextColor, baseTextColor);

            // Change background color
            Color baseBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = ColorUtility.GetColorByName(currentAttribute.BackgroundColor, baseBackgroundColor);

            // Draw property
            EditorGUI.PropertyField(position, property, label);

            // Reset GUI colors with base colors
            GUI.contentColor = baseTextColor;
            GUI.backgroundColor = baseBackgroundColor;     
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}