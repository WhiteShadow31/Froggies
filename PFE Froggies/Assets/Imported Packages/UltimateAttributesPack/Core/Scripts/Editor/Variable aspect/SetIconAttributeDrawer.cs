using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(SetIconAttribute))]
    public class SetIconAttributeDrawer : PropertyDrawer
    {
        float spacing = 3f;
        float spaceAfterIcon = 10f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            SetIconAttribute currentAttribute = attribute as SetIconAttribute;

            // Get icon size
            float iconSize = GetIconSize(currentAttribute.IconSize);

            // Draw property rect
            float maxIconWidth = GetIconSize(IconSize.Large);
            Rect propertyRect = new Rect();
            if (iconSize > EditorGUIUtility.singleLineHeight) // If icon height is bigger than property height, align property to icon center
                propertyRect = new Rect(position.xMin + maxIconWidth + spaceAfterIcon, position.yMin + (iconSize - EditorGUI.GetPropertyHeight(property)) / 2 + spacing, position.width - maxIconWidth - spaceAfterIcon, EditorGUI.GetPropertyHeight(property));
            else
                propertyRect = new Rect(position.xMin + maxIconWidth + spaceAfterIcon, position.yMin + spacing, position.width - maxIconWidth - spaceAfterIcon, EditorGUI.GetPropertyHeight(property));

            // Draw property
            EditorGUI.PropertyField(propertyRect, property, label);

            // Draw icon
            Texture iconTexture = AssetDatabase.LoadAssetAtPath<Texture>(currentAttribute.IconPath);
            if (iconTexture == null) // If icon not found with path, log warning and return
            {
                Debug.LogWarning($"{nameof(SetIconAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} :  Cannot find icon as texture with path {currentAttribute.IconPath}");
                return;
            }

            // Draw icon
            Rect iconRect = new Rect(position.xMin, position.yMin + spacing, iconSize, iconSize);
            GUI.DrawTexture(iconRect, iconTexture);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            SetIconAttribute currentAttribute = attribute as SetIconAttribute;

            // Get icon size
            float iconSize = GetIconSize(currentAttribute.IconSize);
            
            if (iconSize > EditorGUI.GetPropertyHeight(property))
                return iconSize + spacing * 2;
            else
                return EditorGUI.GetPropertyHeight(property) + spacing * 2;
        }

        float GetIconSize(IconSize size)
        {
            switch (size)
            {
                case IconSize.Small:
                    return 20f;
                case IconSize.Medium:
                    return 30f;
                case IconSize.Large:
                    return 40f;
                default:
                    return 0f;
            }
        }
    }
}