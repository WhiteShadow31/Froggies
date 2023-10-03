using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(SubTitleAttribute))]
    public class SubTitleAttributeDrawer : DecoratorDrawer
    {
        float height = 18;
        float spacing = 6;

        public override void OnGUI(Rect position)
        {
            SubTitleAttribute subTitleAttribute = attribute as SubTitleAttribute;

            // Get color with name
            Color backgroundDrawColor = AttributesFunctions.GetColorByName(subTitleAttribute.BackgroundColor, Color.white);
            Color titleDrawColor = AttributesFunctions.GetColorByName(subTitleAttribute.TitleColor, Color.white);

            // Draw background
            Rect backgroundRect = new Rect(position.xMin, position.yMin, position.width / 2, height);
            EditorGUI.DrawRect(backgroundRect, backgroundDrawColor);

            // Draw title
            GUISkin titleSkin = Resources.Load<GUISkin>("Styles/GUIStyles/AttributesTextsSkin");
            Rect titleRect = new Rect(position.xMin, position.yMin, position.width / 2, height);
            GUIStyle titleStyle = titleSkin.GetStyle("Subtitle");
            titleStyle.normal.textColor = titleDrawColor;
            EditorGUI.DropShadowLabel(titleRect, subTitleAttribute.Title, titleStyle);
        }

        // Set total spacing
        public override float GetHeight()
        {
            return spacing + height + spacing;
        }
    }
}