using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(FunctionButtonAttribute))] 
    public class FunctionButtonAttributeDrawer : DecoratorDrawer
    {
        bool isExpanded;
        float functionButtonHeight = 20;
        float scriptIconSize = 18f;
        float foldoutWidth = 20;

        public override void OnGUI(Rect position)
        {
            // Get current attribute
            FunctionButtonAttribute currentAttribute = attribute as FunctionButtonAttribute;

            // Get target function script if it's selected in hierarchy
            MonoBehaviour targetScript = null;
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent(currentAttribute.ClassType) != null)
            {
                targetScript = Selection.activeGameObject.GetComponent(currentAttribute.ClassType) as MonoBehaviour;
            }
            // If target script is not found in object, log warning and return
            else
            {
                Debug.LogWarning($"{nameof(FunctionButtonAttribute)} : {currentAttribute.ClassType.Name} not found in object");
                return;
            }

            // If function is not found, log warning and return
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            if(currentAttribute.ClassType.GetMethod(currentAttribute.FunctionName, bindingFlags) == null)
            {
                Debug.LogWarning($"{nameof(FunctionButtonAttribute)} : Function {currentAttribute.FunctionName} not found in {currentAttribute.ClassType.Name}");
                return;
            }
             
            // If target object is selected
            if(targetScript != null)
            {
                // Draw function button
                Rect functionButtonRect = new Rect(position.xMin, position.yMin + 5, position.width - foldoutWidth - 5, functionButtonHeight);
                if (GUI.Button(functionButtonRect, currentAttribute.Text))
                {
                    // Save runInEditMode state and enable it
                    bool runInEditModeState = targetScript.runInEditMode;
                    targetScript.runInEditMode = true;

                    // Execute function in script
                    targetScript.SendMessage(currentAttribute.FunctionName);
                    
                    // Reset runInEditMode state
                    targetScript.runInEditMode = runInEditModeState;
                }

                // Calculate button text width
                float textWidth = GUI.skin.box.CalcSize(new GUIContent(currentAttribute.Text)).x;
                
                // Draw script icon
                Rect scriptIconRect = new Rect(position.xMax / 2 - textWidth / 2 - scriptIconSize - 10, position.yMin + 6, scriptIconSize, scriptIconSize);
                if (EditorGUIUtility.IconContent("cs Script Icon") != null)
                {
                    GUIContent scriptIcon = EditorGUIUtility.IconContent("cs Script Icon");
                    EditorGUI.LabelField(scriptIconRect, scriptIcon);
                }
                
                // Draw foldout button
                Rect foldoutRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
                isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, "");
                if (isExpanded)
                {
                    EditorGUILayout.Space(functionButtonHeight + 2); // Set space
                    EditorGUILayout.BeginVertical(GUI.skin.box); // Start drawing background
                    EditorGUILayout.LabelField($"Execute function {currentAttribute.FunctionName} in {targetScript.name}.{currentAttribute.ClassType.Name}"); // Draw foldout infos
                    EditorGUILayout.EndVertical(); // End drawing background
                }
            }
            else
            {
                // Draw warning icon
                Rect warningIconRect = new Rect(position.xMin, position.yMin + 3, 22, 22);
                GUIContent warningIcon = EditorGUIUtility.IconContent("console.warnicon");
                if(warningIcon != null)
                    EditorGUI.LabelField(warningIconRect, warningIcon);

                // Draw warning text
                Rect warningTextRect = new Rect(position.xMin + 25, position.yMin + 5, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);

                // Save GUI color and set it to yellow
                Color savedGUIColor = GUI.color;
                GUI.color = Color.yellow;

                // Draw warning text
                EditorGUI.LabelField(warningTextRect, "Select the object to show the function button");

                // Reset GUI color
                GUI.color = savedGUIColor;

                // Draw foldout button and foldout infos
                Rect foldoutRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
                isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, "");
                if (isExpanded) // Draw foldout infos
                {             
                    EditorGUILayout.Space(functionButtonHeight + 2); // Set space
                    EditorGUILayout.BeginVertical(GUI.skin.box); // Start drawing background
                    EditorGUILayout.LabelField($"Execute function {currentAttribute.FunctionName}"); // Draw foldout infos
                    EditorGUILayout.EndVertical(); // End drawing background
                }
            }
        }

        public override float GetHeight()
        {
            return functionButtonHeight + (isExpanded ? EditorGUIUtility.singleLineHeight + 3 : 0) + 10;
        }
    }
}

