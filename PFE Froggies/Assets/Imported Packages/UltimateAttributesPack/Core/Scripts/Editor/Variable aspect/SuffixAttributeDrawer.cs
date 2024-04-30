using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(SuffixAttribute))]
    public class SuffixAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            SuffixAttribute currentAttribute = attribute as SuffixAttribute;

            // Calculate suffix text width
            GUIContent suffixContent = new GUIContent(currentAttribute.SuffixText);
            float suffixWidth = GUI.skin.box.CalcSize(suffixContent).x;

            // Draw property
            Rect propertyRect = new Rect(position.xMin, position.yMin, position.width - suffixWidth - 5, position.height);
            EditorGUI.PropertyField(propertyRect, property, label);

            // Draw suffix            
            Rect suffixRect = new Rect(position.xMax - suffixWidth, position.yMin, suffixWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(suffixRect, suffixContent);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}