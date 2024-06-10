using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    using UltimateAttributesPack.Utility;

    [CustomPropertyDrawer(typeof(LineTitleAttribute))]
    public class LineTitleAttributeDrawer : DecoratorDrawer
    {
        float spacing = 5f;
        float lineVerticalSpacing = 7f;
        float lineHorizontalSpacing = 7f;
        float lineHeight = 2f;
        float textHeight;

        public override void OnGUI(Rect position)
        {
            // Get current attribute
            LineTitleAttribute currentAttribute = attribute as LineTitleAttribute;

            // Get color with name
            Color textColor = ColorUtility.GetColorByName(currentAttribute.TextColor, GUI.skin.box.normal.textColor);
            Color linesColor = ColorUtility.GetColorByName(currentAttribute.LineColor, GUI.skin.box.normal.textColor);

            // Calculate title size and set text style
            GUIStyle style = new GUIStyle();
            style.fontSize = 16;
            style.normal.textColor = textColor;
            GUIContent title = new GUIContent(currentAttribute.Title);
            Vector2 titleSize = style.CalcSize(title);
            textHeight = titleSize.y;

            // Draw title
            Rect titleRect = new Rect((position.width / 2) - (titleSize.x / 2), position.yMin + spacing, titleSize.x, titleSize.y);
            EditorGUI.LabelField(titleRect, currentAttribute.Title, style);

            // Draw left line
            Rect leftLineRect = new Rect(position.xMin, position.yMin + spacing + lineVerticalSpacing, titleRect.xMin - lineHorizontalSpacing, lineHeight);
            EditorGUI.DrawRect(leftLineRect, linesColor);

            // Draw right line
            Rect rightLineRect = new Rect(titleRect.xMax + lineHorizontalSpacing, position.yMin + spacing + lineVerticalSpacing, position.width - titleRect.xMax, lineHeight);
            EditorGUI.DrawRect(rightLineRect, linesColor);
        }

        public override float GetHeight()
        {
            return spacing * 2 + textHeight;
        }
    }
}