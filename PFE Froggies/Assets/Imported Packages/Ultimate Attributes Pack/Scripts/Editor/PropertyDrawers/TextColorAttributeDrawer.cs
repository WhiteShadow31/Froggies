using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(TextColorAttribute))]
    public class TextColorAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            TextColorAttribute textColorAttribute = attribute as TextColorAttribute;

            Color originalColor = GUI.color;
            GUI.color = AttributesFunctions.GetColorByName(textColorAttribute.Color, Color.white);

            EditorGUI.PropertyField(position, property, label);

            GUI.color = originalColor;
        }
    }
}