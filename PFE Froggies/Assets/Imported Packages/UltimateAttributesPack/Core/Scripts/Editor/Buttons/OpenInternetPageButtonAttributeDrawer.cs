using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(OpenInternetPageButtonAttribute))]
    public class OpenInternetPageButtonAttributeDrawer : DecoratorDrawer
    {
        bool isExpanded;
        float buttonHeight = 20f;
        float internetIconSize = 20;
        float foldoutWidth = 20;

        public override void OnGUI(Rect position)
        {
            // Get current attribute
            OpenInternetPageButtonAttribute currentAttribute = attribute as OpenInternetPageButtonAttribute;

            // Display button
            Rect buttonRect = new Rect(position.xMin, position.yMin + 5, position.width - foldoutWidth - 5, buttonHeight);
            if(GUI.Button(buttonRect, currentAttribute.Text))
                Application.OpenURL(currentAttribute.Link); // Open internet page if button clicked

            // Calculate button text width
            float textWidth = GUI.skin.box.CalcSize(new GUIContent(currentAttribute.Text)).x;

            // Draw internet icon
            Rect internetIconRect = new Rect(position.xMax / 2 - textWidth / 2 - internetIconSize - 10, position.yMin + 4, internetIconSize, internetIconSize);
            if (EditorGUIUtility.IconContent("BuildSettings.Web.Small") != null)
            {
                GUIContent internetIcon = EditorGUIUtility.IconContent("BuildSettings.Web.Small");
                EditorGUI.LabelField(internetIconRect, internetIcon);
            }

            // Display foldout button
            Rect foldoutGroupRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
            isExpanded = EditorGUI.Foldout(foldoutGroupRect, isExpanded, "");
            if (isExpanded)
            {
                EditorGUILayout.Space(buttonHeight + 2); // Set space          
                EditorGUILayout.BeginVertical(GUI.skin.box); // Start drawing background
                EditorGUILayout.LabelField(new GUIContent($"Open URL : {currentAttribute.Link}")); // Draw foldout infos
                EditorGUILayout.EndVertical(); // End drawing background
            }
        }

        public override float GetHeight()
        {
            return buttonHeight + (isExpanded ? EditorGUIUtility.singleLineHeight + 3 : 0) + 10;
        }
    }
}