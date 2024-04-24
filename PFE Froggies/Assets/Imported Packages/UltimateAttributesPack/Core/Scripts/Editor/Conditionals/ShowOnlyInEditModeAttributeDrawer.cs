using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(ShowOnlyInEditModeAttribute))]
    public class ShowOnlyInEditModeAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            ShowOnlyInEditModeAttribute currentAttribute = attribute as ShowOnlyInEditModeAttribute;

            // Draw property if application is in edit mode
            if (!Application.isPlaying)
            {
                // Draw property
                EditorGUI.PropertyField(position, property);
            }
            // Set property to readonly if attribute parameter is set to readonly
            else if (currentAttribute.DisablingType == DisablingType.ReadOnly)
            {
                // Save gui state and disable it for readonly
                bool lastGUIState = GUI.enabled;
                GUI.enabled = false;

                // Draw property
                EditorGUI.PropertyField(position, property);

                // Reset gui state
                GUI.enabled = lastGUIState;
            }           
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Get attribute
            ShowOnlyInEditModeAttribute currentAttribute = attribute as ShowOnlyInEditModeAttribute;

            // Set property height if application is in edit mode
            if (!Application.isPlaying || currentAttribute.DisablingType == DisablingType.ReadOnly)
                return EditorGUI.GetPropertyHeight(property);
            else
                return 0;
        }
    }
}