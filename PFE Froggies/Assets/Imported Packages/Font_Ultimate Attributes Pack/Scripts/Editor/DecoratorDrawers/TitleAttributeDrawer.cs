using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleAttributeDrawer : DecoratorDrawer
    {
        float height = 30;
        float spacing = 12;

        public override void OnGUI(Rect position)
        {
            TitleAttribute titleAttribute = attribute as TitleAttribute;

            // Get color with name       
            Color backgroundDrawColor = AttributesFunctions.GetColorByName(titleAttribute.BackgroundColor, Color.white);
            Color titleDrawColor = AttributesFunctions.GetColorByName(titleAttribute.TitleColor, Color.white);

            // Draw background
            Rect backgroundRect = new Rect(position.xMin, position.yMin + spacing, position.width, height);
            EditorGUI.DrawRect(backgroundRect, backgroundDrawColor);
        
            // Draw title
            GUISkin titleSkin = Resources.Load<GUISkin>("Styles/GUIStyles/AttributesTextsSkin");
            Rect titleRect = new Rect(position.xMin, position.yMin + spacing, position.width, height);
            GUIStyle titleStyle = titleSkin.GetStyle("Title");
            titleStyle.normal.textColor = titleDrawColor;
            EditorGUI.DropShadowLabel(titleRect, titleAttribute.Title, titleStyle);
        }
    
        // Set total spacing
        public override float GetHeight()
        {      
            return spacing + height + spacing;
        }
    }
}