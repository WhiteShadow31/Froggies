using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarAttributeDrawer : DecoratorDrawer
    {
        bool isExpanded;
        float foldoutWidth = 20;

        public override void OnGUI(Rect position)
        {
            ProgressBarAttribute progressBarAttribute = attribute as ProgressBarAttribute;

            EditorGUILayout.BeginHorizontal();

            // Display progress bar
            float value = Mathf.InverseLerp(progressBarAttribute.Min, progressBarAttribute.Max, progressBarAttribute.Value);
            Rect progressBarRect = new Rect(position.xMin, position.yMin + 5, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);
            EditorGUI.ProgressBar(progressBarRect, value, progressBarAttribute.Name);

            // Display foldout button
            Rect foldoutGroupRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
            isExpanded = EditorGUI.Foldout(foldoutGroupRect, isExpanded, "");

            EditorGUILayout.EndHorizontal();

            if (isExpanded)
            {
                Rect textFieldRect = new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight + 5, position.width - foldoutWidth, EditorGUIUtility.singleLineHeight);
                GUILayout.BeginArea(textFieldRect);
                EditorGUILayout.LabelField(new GUIContent("Min : " + progressBarAttribute.Min + ", Max : " + progressBarAttribute.Max + ", Value : " + progressBarAttribute.Value + ". Percent : " + Mathf.Round(value * 100) + " (" + value.ToString("F2") + ")", "Min : " + progressBarAttribute.Min + ", Max : " + progressBarAttribute.Max + ", Value : " + progressBarAttribute.Value + ". Percent : " + Mathf.Round(value * 100) + " (" + value.ToString("F2") + ")"), GUILayout.Width(position.width - foldoutWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUILayout.EndArea();
            }
        }

        // Set total spacing
        public override float GetHeight()
        {
            int lines = 1;

            if (isExpanded)
            {
                lines = 2;
            }
            else
            {
                lines = 1;
            }

            return EditorGUIUtility.singleLineHeight * lines + 10;
        }
    }
}