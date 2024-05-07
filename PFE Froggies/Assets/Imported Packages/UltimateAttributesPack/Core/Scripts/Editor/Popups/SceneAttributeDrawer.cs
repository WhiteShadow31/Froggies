using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(SceneAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Type not supported (only work with string and int)");
                return;
            }

            // Get scenes names
            string[] scenes = GetScenes();

            // If there is no scene in build settings, log warning and return
            if (scenes.Length == 0)
            {
                Debug.LogWarning($"{nameof(SceneAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : No scenes found in build settings");
                return;
            }

            // Get scenes options
            string[] scenesOptions = GetSceneOptions(scenes);

            switch (property.propertyType)
            {
                case SerializedPropertyType.String:

                    int index = Mathf.Clamp(Array.IndexOf(scenes, property.stringValue), 0, scenes.Length - 1);
                    int newIndex = EditorGUI.Popup(position, label.text, index, scenesOptions);
                    string newScene = scenes[newIndex];

                    if (property.stringValue != newScene)
                        property.stringValue = scenes[newIndex];
                    return;
                case SerializedPropertyType.Integer:

                    // Get current index and draw popup
                    int intIndex = property.intValue;
                    int newIntIndex = EditorGUI.Popup(position, label.text, intIndex, scenesOptions);

                    // If selected scene has changed, set new one
                    if (property.intValue != newIntIndex)
                        property.intValue = newIntIndex;
                    return;
                default:                   
                    return;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        private string[] GetScenes()
        {
            return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
        }

        private string[] GetSceneOptions(string[] scenes)
        {
            return (from scene in scenes select Regex.Match(scene ?? string.Empty, @".+\/(.+).unity").Groups[1].Value).ToArray();
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return true;
                case SerializedPropertyType.String:
                    return true;
                default:
                    return false;
            }
        }
    }
}