using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(RectTransformInfoAttribute))]
    public class RectTransformInfoAttributeDrawer : PropertyDrawer
    {
        float foldoutWidth = 20;
        bool anchorsFoldout;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(RectTransformInfoAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with RectTransform)");
                return;
            }
            
            // Get current attribute
            RectTransformInfoAttribute currentAttribute = attribute as RectTransformInfoAttribute;

            // Draw property
            Rect propertyRect = new Rect(position.xMin, position.yMin, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property, label);

            // Display foldout button
            Rect foldoutRect = new Rect(position.xMax - foldoutWidth, position.yMin - 1, EditorGUIUtility.singleLineHeight, foldoutWidth);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "");

            // Draw rect transform info in foldout
            if (property.objectReferenceValue != null && property.isExpanded)
            {
                // Get rect transform
                RectTransform propertyRectTransform = property.objectReferenceValue as RectTransform;

                if (currentAttribute.CanModifyValues)
                {
                    // Draw modifiable values
                    DrawInfos(propertyRectTransform);
                }
                else
                {
                    // Save gui state and disable it for readonly
                    bool lastGUIState = GUI.enabled;
                    GUI.enabled = false;

                    // Draw rect transform infos
                    DrawInfos(propertyRectTransform);

                    // Reset gui state
                    GUI.enabled = lastGUIState;
                }
            }
            // Draw warning box in foldout if no object selected
            else if (property.isExpanded)
            {
                Rect warningBoxRect = new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight + 3, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.HelpBox(warningBoxRect, "No object selected", MessageType.Warning);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null && property.isExpanded)
                return EditorGUI.GetPropertyHeight(property) + EditorGUIUtility.singleLineHeight + 3; // When drawing warning box if no object selected
            else
                return EditorGUI.GetPropertyHeight(property);
        }

        void DrawInfos(RectTransform rectTransform)
        {
            // Begin drawing background
            EditorGUILayout.BeginVertical(GUI.skin.box);

            // Draw rect transform position fields
            rectTransform.position = EditorGUILayout.Vector3Field("Position", rectTransform.position);

            EditorGUILayout.Space(5);

            // Draw rect transform size fields
            rectTransform.sizeDelta = EditorGUILayout.Vector2Field("Size", rectTransform.sizeDelta);

            // Draw rect transform anchors fields
            anchorsFoldout = EditorGUILayout.Foldout(anchorsFoldout, "Anchors");
            if (anchorsFoldout)
            {
                EditorGUI.indentLevel++;
                rectTransform.anchorMin = EditorGUILayout.Vector2Field("Min", rectTransform.anchorMin);
                rectTransform.anchorMax = EditorGUILayout.Vector2Field("Max", rectTransform.anchorMax);
                EditorGUI.indentLevel--;

                EditorGUILayout.Space(5);
            }

            // Draw rect transform pivot fields
            rectTransform.pivot = EditorGUILayout.Vector2Field("Pivot", rectTransform.pivot);

            EditorGUILayout.Space(5);

            // Draw rect transform rotation fields
            rectTransform.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", rectTransform.eulerAngles));

            // Draw rect transform scale fields
            rectTransform.localScale = EditorGUILayout.Vector3Field("Scale", rectTransform.localScale);

            // End drawing background
            EditorGUILayout.EndVertical();
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.type)
            {
                case "PPtr<$RectTransform>":
                    return true;
                default:
                    return false;
            }
        }
    }
}