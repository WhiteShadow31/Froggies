using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {
        float spacing = 3f;
        float minHeight = 30f;
        float helpBoxHeight;

        public override void OnGUI(Rect position)
        {
            // Get current attribute
            HelpBoxAttribute currentAttribute = attribute as HelpBoxAttribute;

            // Calculate total help box height
            string helpText = currentAttribute.HelpText;
            helpBoxHeight = GUI.skin.box.CalcHeight(new GUIContent(helpText), EditorGUIUtility.currentViewWidth);

            // Set spacing up
            position.yMin += spacing;

            // Draw help box
            EditorGUI.HelpBox(position, helpText, GetMessageType(currentAttribute.MessageType));
        }

        public override float GetHeight()
        {
            return Mathf.Max(minHeight, helpBoxHeight) + spacing * 2;
        }

        // Get message type
        MessageType GetMessageType(HelpBoxMessageType helpBoxMessageType)
        {
            switch (helpBoxMessageType)
            {
                case HelpBoxMessageType.None:
                    return MessageType.None;
                case HelpBoxMessageType.Info:
                    return MessageType.Info;
                case HelpBoxMessageType.Warning:
                    return MessageType.Warning;
                case HelpBoxMessageType.Error:
                    return MessageType.Error;
                default:
                    return MessageType.None;
            }
        }
    }
}