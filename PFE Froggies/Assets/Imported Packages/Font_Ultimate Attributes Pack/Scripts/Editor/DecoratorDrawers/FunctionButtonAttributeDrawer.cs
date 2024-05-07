using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(FunctionButtonAttribute))] 
    public class FunctionButtonAttributeDrawer : DecoratorDrawer
    {
        bool isExpanded;
        float foldoutWidth = 20;

        public override void OnGUI(Rect position)
        {
            FunctionButtonAttribute functionButtonAttribute = attribute as FunctionButtonAttribute;
            MonoBehaviour mb = null;

            if(Selection.activeGameObject != null)
            {
                if(Selection.activeGameObject.GetComponent(functionButtonAttribute.ClassType) != null)
                {
                    mb = Selection.activeGameObject.GetComponent(functionButtonAttribute.ClassType) as MonoBehaviour;                                                                                                              
                }
            }
            else
            {
                mb = null;
            }

            if(mb != null)
            {
                EditorGUILayout.BeginHorizontal();

                // Display button
                Rect buttonRect = new Rect(position.x, position.yMin + 5, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(buttonRect, functionButtonAttribute.Name))
                {
                    bool runInEditModeState = mb.runInEditMode;
                    mb.runInEditMode = true;
                    mb.SendMessage(functionButtonAttribute.FunctionName);
                    mb.runInEditMode = runInEditModeState;
                }

                // Display foldout button
                Rect foldoutGroupRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
                isExpanded = EditorGUI.Foldout(foldoutGroupRect, isExpanded, "");

                EditorGUILayout.EndHorizontal();

                if (isExpanded)
                {
                    Rect textFieldRect = new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight + 5, position.width - foldoutWidth, EditorGUIUtility.singleLineHeight);
                    GUILayout.BeginArea(textFieldRect);
                    EditorGUILayout.LabelField(new GUIContent("Execute function : " + functionButtonAttribute.FunctionName, functionButtonAttribute.FunctionName), GUILayout.Width(position.width - foldoutWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    GUILayout.EndArea();
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                // Display warning icon
                Rect warningIconRect = new Rect(position.xMin, position.yMin, 22, 22);
                GUIContent warningIcon = EditorGUIUtility.IconContent("console.warnicon");
                GUILayout.BeginArea(warningIconRect);
                GUILayout.Label(warningIcon);
                GUILayout.EndArea();

                // Draw warning text
                Rect labelRect = new Rect(position.xMin + 25, position.yMin + 5, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);
                GUILayout.BeginArea(labelRect);
                GUI.color = Color.yellow;
                EditorGUILayout.LabelField(new GUIContent("Select the object to show the function button", "Select the object to show the function button"), GUILayout.Width(position.width - foldoutWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.color = Color.white;
                GUILayout.EndArea();

                // Display foldout button
                Rect foldoutGroupRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
                isExpanded = EditorGUI.Foldout(foldoutGroupRect, isExpanded, "");

                EditorGUILayout.EndHorizontal();

                if (isExpanded)
                {
                    Rect textFieldRect = new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight + 5, position.width - foldoutWidth, EditorGUIUtility.singleLineHeight);
                    GUILayout.BeginArea(textFieldRect);
                    EditorGUILayout.LabelField(new GUIContent("Execute method : " + functionButtonAttribute.FunctionName, functionButtonAttribute.FunctionName), GUILayout.Width(position.width - foldoutWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    GUILayout.EndArea();
                }
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

