using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(LineTitleAttribute))]
    public class LineTitleAttributeDrawer : DecoratorDrawer
    {
        float height = 30;
        float spacing = 12;

        public override void OnGUI(Rect position)
        {
            LineTitleAttribute lineTitleAttribute = attribute as LineTitleAttribute;

            // Get color with name       
            Color lineDrawColor = AttributesFunctions.GetColorByName(lineTitleAttribute.LineColor, Color.white);
            Color titleDrawColor = AttributesFunctions.GetColorByName(lineTitleAttribute.TitleColor, Color.white);

            EditorGUILayout.BeginHorizontal();

            // Draw first line
            Rect firstLineRect = new Rect(position.xMin, position.yMin + spacing, position.width / 3, 2);
            EditorGUI.DrawRect(firstLineRect, lineDrawColor);

            // Draw title
            Rect titleRect = new Rect(position.xMin, position.yMin + spacing, position.width, height);
            EditorGUI.DropShadowLabel(titleRect, lineTitleAttribute.Title);

            // Draw second line
            Rect secondLineRect = new Rect(position.xMax, position.yMin + spacing, position.width / 3, 2);
            EditorGUI.DrawRect(secondLineRect, lineDrawColor);

            EditorGUILayout.EndHorizontal();
        }

        // Set total spacing
        public override float GetHeight()
        {
            return spacing + height + spacing;
        }

    }
}