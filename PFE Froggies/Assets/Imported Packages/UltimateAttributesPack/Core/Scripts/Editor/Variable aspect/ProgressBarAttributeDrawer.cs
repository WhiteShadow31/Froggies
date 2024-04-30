using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarAttributeDrawer : PropertyDrawer
    {
        float foldoutWidth = 20;
        float progressBarHeight = 18f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(ProgressBarAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with float and int)");
                return;
            }

            // Get current attribute
            ProgressBarAttribute currentAttribute = attribute as ProgressBarAttribute;

            // Get current progress bar value
            float propertyValue = Mathf.InverseLerp(currentAttribute.Min, currentAttribute.Max, property.propertyType == SerializedPropertyType.Float ? property.floatValue : property.intValue);

            // Display progress bar
            Rect progressBarRect = new Rect(position.xMin, position.yMin, position.width - foldoutWidth - 5, progressBarHeight);
            string progressBarText = currentAttribute.DisplayPercent ? $"{currentAttribute.Text} ({(propertyValue * 100).ToString("F0")} / 100)" : currentAttribute.Text;
            EditorGUI.ProgressBar(progressBarRect, propertyValue, progressBarText);

            // Draw foldout
            Rect foldoutRect = new Rect(position.xMax - 20, position.yMin - 2, EditorGUIUtility.singleLineHeight, foldoutWidth);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "");       
            if (property.isExpanded)
            {
                // Begin drawing background
                EditorGUILayout.BeginVertical(GUI.skin.box);

                // Draw foldout infos
                EditorGUILayout.LabelField(new GUIContent($"Min : {currentAttribute.Min} Max : {currentAttribute.Max} Value : {(property.propertyType == SerializedPropertyType.Float ? property.floatValue : property.intValue)} Percent : {Mathf.Round(propertyValue * 100)}"));

                // End drawing background
                EditorGUILayout.EndVertical();
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