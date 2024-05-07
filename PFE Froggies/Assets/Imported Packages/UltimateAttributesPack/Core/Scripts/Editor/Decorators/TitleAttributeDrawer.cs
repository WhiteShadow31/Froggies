using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    using UltimateAttributesPack.Utility;

    [CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleAttributeDrawer : DecoratorDrawer
    {
        float titleHeight = 30;
        float spacing = 10;
        float iconSize = 24;
        
        public override void OnGUI(Rect position)
        {
            // Get attribute
            TitleAttribute currentAttribute = attribute as TitleAttribute;

            if (!string.IsNullOrEmpty(currentAttribute.BackgroundColor))
            {
                // Draw background
                Color backgroundColor = ColorUtility.GetColorByName(currentAttribute.BackgroundColor, Color.gray);
                Rect backgroundRect = new Rect(position.xMin, position.yMin + spacing, position.width, titleHeight);
                EditorGUI.DrawRect(backgroundRect, backgroundColor);
            }

            // Draw title
            GUISkin titleSkin = Resources.Load<GUISkin>("Styles/GUIStyles/AttributesTextsSkin");
            GUIStyle titleStyle = titleSkin.GetStyle("Title");
            Rect titleRect = new Rect(position.xMin, position.yMin + spacing, position.width, titleHeight);
            titleStyle.normal.textColor = ColorUtility.GetColorByName(currentAttribute.TextColor, GUI.skin.box.normal.textColor);
            EditorGUI.DropShadowLabel(titleRect, currentAttribute.Title, titleStyle);

            // Calculate title width
            float titleWidth = titleStyle.CalcSize(new GUIContent(currentAttribute.Title)).x;

            // Draw icon
            if(currentAttribute.IconPath != string.Empty)
            {
                Texture iconTexture = AssetDatabase.LoadAssetAtPath<Texture>(currentAttribute.IconPath);
                if(iconTexture != null)
                {
                    Rect iconRect = new Rect(position.xMax / 2 - titleWidth / 2 - iconSize - 10, position.yMin + spacing + 3, iconSize, iconSize);
                    GUI.DrawTexture(iconRect, iconTexture);
                }
                else
                    Debug.LogWarning($"{nameof(TitleAttribute)} : title with name {currentAttribute.Title} cannot find icon as texture at path : {currentAttribute.IconPath}");
            }
        }
    
        public override float GetHeight()
        {      
            return spacing * 2 + titleHeight;
        }
    }
}