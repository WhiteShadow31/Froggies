using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(OpenInternetPageButtonAttribute))]
    public class OpenInternetPageButtonAttributeDrawer : DecoratorDrawer
    {
        bool isExpanded;
        float foldoutWidth = 20;

        public override void OnGUI(Rect position)
        {
            OpenInternetPageButtonAttribute openInternetPageButtonAttribute = attribute as OpenInternetPageButtonAttribute;

            EditorGUILayout.BeginHorizontal();

            // Display button
            Rect buttonRect = new Rect(position.xMin, position.yMin + 5, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);
            if(GUI.Button(buttonRect, openInternetPageButtonAttribute.Name))
            {
                OpenInternetPage(openInternetPageButtonAttribute.Link);
            }
        
            // Display foldout button
            Rect foldoutGroupRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
            isExpanded = EditorGUI.Foldout(foldoutGroupRect, isExpanded, "");

            EditorGUILayout.EndHorizontal();

            if (isExpanded)
            {
                Rect textFieldRect = new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight + 5, position.width - foldoutWidth, EditorGUIUtility.singleLineHeight);
                GUILayout.BeginArea(textFieldRect);
                EditorGUILayout.LabelField(new GUIContent("Open URL : " + openInternetPageButtonAttribute.Link, openInternetPageButtonAttribute.Link), GUILayout.Width(position.width - foldoutWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight));
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

        private void OpenInternetPage(string url)
        {
            Application.OpenURL(url);
        }
    }
}