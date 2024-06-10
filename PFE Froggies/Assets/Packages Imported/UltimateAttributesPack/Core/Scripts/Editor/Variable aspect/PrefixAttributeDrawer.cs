using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(PrefixAttribute))]
    public class PrefixAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            PrefixAttribute currentAttribute = attribute as PrefixAttribute;

            // Draw property
            EditorGUI.PropertyField(position, property, label);

            // Draw prefix
            GUIContent prefixContent = new GUIContent(currentAttribute.PrefixText);
            float prefixWidth = GUI.skin.box.CalcSize(prefixContent).x;
            Rect prefixRect = new Rect(position.xMin + EditorGUIUtility.labelWidth - prefixWidth, position.yMin, prefixWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(prefixRect, prefixContent);           
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}