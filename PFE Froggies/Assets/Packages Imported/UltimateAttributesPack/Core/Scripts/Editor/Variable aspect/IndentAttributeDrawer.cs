using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(IndentAttribute))]
    public class IndentAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            IndentAttribute currentAttribute = attribute as IndentAttribute;

            // Get new indent level
            int newIndentLevel = currentAttribute.IndentLevel;

            // Set new indent level
            EditorGUI.indentLevel += newIndentLevel;

            // Draw property
            EditorGUI.PropertyField(position, property, label);

            // Reset indent level
            EditorGUI.indentLevel -= newIndentLevel;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}