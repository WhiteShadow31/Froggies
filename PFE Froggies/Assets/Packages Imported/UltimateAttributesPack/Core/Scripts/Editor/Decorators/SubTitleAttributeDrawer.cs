using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    using UltimateAttributesPack.Utility;

    [CustomPropertyDrawer(typeof(SubTitleAttribute))]
    public class SubTitleAttributeDrawer : DecoratorDrawer
    {
        float spacing = 3;
        float titleHeight = 18;
        float iconSize = 14;

        public override void OnGUI(Rect position)
        {
            // Get attribute
            SubTitleAttribute currentAttribute = attribute as SubTitleAttribute;

            if (!string.IsNullOrEmpty(currentAttribute.BackgroundColor))
            {
                // Draw background
                Color backgroundColor = ColorUtility.GetColorByName(currentAttribute.BackgroundColor, Color.gray);
                Rect backgroundRect = new Rect(position.xMin, position.yMin + spacing, position.width / 2, titleHeight);
                EditorGUI.DrawRect(backgroundRect, backgroundColor);
            }

            // Draw title
            GUISkin titleSkin = Resources.Load<GUISkin>("Styles/GUIStyles/AttributesTextsSkin");
            Rect titleRect = new Rect(position.xMin, position.yMin + spacing, position.width / 2, titleHeight);
            GUIStyle titleStyle = titleSkin.GetStyle("Subtitle");
            titleStyle.normal.textColor = ColorUtility.GetColorByName(currentAttribute.TextColor, GUI.skin.box.normal.textColor);
            EditorGUI.DropShadowLabel(titleRect, currentAttribute.Title, titleStyle);

            // Calculate title width
            float titleWidth = titleStyle.CalcSize(new GUIContent(currentAttribute.Title)).x;

            // Draw icon
            if (currentAttribute.IconPath != string.Empty)
            {
                Texture iconTexture = (Texture)AssetDatabase.LoadAssetAtPath<Texture>(currentAttribute.IconPath);
                if (iconTexture != null)
                {
                    Rect iconRect = new Rect(position.xMax / 4 - titleWidth / 2 - iconSize - 10, position.yMin + 2 + spacing, iconSize, iconSize);
                    GUI.DrawTexture(iconRect, iconTexture);
                }
                else
                    Debug.LogWarning($"{nameof(currentAttribute)} : title with name '{currentAttribute.Title}' cannot find icon as texture at path : {currentAttribute.IconPath}");
            }
        }

        public override float GetHeight()
        {
            return spacing * 2 + titleHeight;
        }
    }
}