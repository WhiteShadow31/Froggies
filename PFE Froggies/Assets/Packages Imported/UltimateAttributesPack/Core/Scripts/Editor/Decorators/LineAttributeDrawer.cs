using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    using UltimateAttributesPack.Utility;

    [CustomPropertyDrawer(typeof(LineAttribute))]
    public class LineAttributeDrawer : DecoratorDrawer
    {
        float spacing = 10f;
        float lineHeight = 2f;

        public override void OnGUI(Rect position)
        {
            // Get attribute
            LineAttribute currentAttribute = attribute as LineAttribute;

            // Get color with name
            Color lineColor = ColorUtility.GetColorByName(currentAttribute.Color, GUI.skin.box.normal.textColor);

            // Draw line
            Rect lineRect = new Rect(position.xMin, position.yMin + spacing, position.width, lineHeight);
            EditorGUI.DrawRect(lineRect, lineColor);
        }

        public override float GetHeight()
        {
            return spacing * 2 + lineHeight;
        }
    }
}