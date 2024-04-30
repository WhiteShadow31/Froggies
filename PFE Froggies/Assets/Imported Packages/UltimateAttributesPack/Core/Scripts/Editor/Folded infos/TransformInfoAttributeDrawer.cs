using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(TransformInfoAttribute))]
    public class TransformInfoAttributeDrawer : PropertyDrawer
    {
        float foldoutWidth = 20;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(TransformInfoAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with Transform)");
                return;
            }
            
            // Get current attribute
            TransformInfoAttribute currentAttribute = attribute as TransformInfoAttribute;

            // Draw property
            Rect propertyRect = new Rect(position.xMin, position.yMin, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property, label);

            // Display foldout button
            Rect foldoutRect = new Rect(position.xMax - foldoutWidth, position.yMin - 1, EditorGUIUtility.singleLineHeight, foldoutWidth);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "");

            // Draw transform info in foldout
            if (property.objectReferenceValue != null && property.isExpanded)
            {
                // Get transform
                Transform propertyTransform = property.objectReferenceValue as Transform;

                if (currentAttribute.CanModifyValues)
                {
                    // Draw modifiable values
                    DrawInfos(propertyTransform);
                }
                else
                {
                    // Save gui state and disable it for readonly
                    bool lastGUIState = GUI.enabled;
                    GUI.enabled = false;

                    // Draw rect transform infos
                    DrawInfos(propertyTransform);

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

        void DrawInfos(Transform rectTransform)
        {
            // Begin drawing background
            EditorGUILayout.BeginVertical(GUI.skin.box);

            // Draw transform position fields
            rectTransform.position = EditorGUILayout.Vector3Field("Position", rectTransform.position);

            // Draw transform rotation fields
            rectTransform.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", rectTransform.eulerAngles));

            // Draw transform scale fields
            rectTransform.localScale = EditorGUILayout.Vector3Field("Scale", rectTransform.localScale);

            // End drawing background
            EditorGUILayout.EndVertical();
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.type)
            {
                case "PPtr<$Transform>":
                    return true;
                default:
                    return false;
            }
        }
    }
}