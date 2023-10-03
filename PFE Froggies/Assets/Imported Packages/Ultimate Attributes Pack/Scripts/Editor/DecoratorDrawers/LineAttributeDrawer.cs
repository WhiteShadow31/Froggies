using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(LineAttribute))]
    public class LineAttributeDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            LineAttribute lineAttribute = attribute as LineAttribute;

            // Get color with name
            Color lineDrawColor = AttributesFunctions.GetColorByName(lineAttribute.Color, Color.white);
        
            // Draw line
            Rect lineRect = new Rect(position.xMin, position.yMin + lineAttribute.Spacing, position.width, lineAttribute.Height);
            EditorGUI.DrawRect(lineRect, lineDrawColor);
        }

        // Set total spacing
        public override float GetHeight()
        {
            LineAttribute lineAttribute = attribute as LineAttribute;
            float totalSpacing = lineAttribute.Spacing + lineAttribute.Height + lineAttribute.Spacing;
            return totalSpacing;
        }
    }
}