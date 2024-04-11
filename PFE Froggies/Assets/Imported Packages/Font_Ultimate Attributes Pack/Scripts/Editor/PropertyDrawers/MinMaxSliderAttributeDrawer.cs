using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinMaxSliderAttribute minMaxAttribute = attribute as MinMaxSliderAttribute;
            var variableType = property.propertyType;

            Rect controlRect = EditorGUI.PrefixLabel(position, label);
            Rect[] splittedRect = SplitRect(controlRect, 3);

            if(variableType == SerializedPropertyType.Vector2)
            {
                EditorGUI.BeginChangeCheck();
            
                Vector2 vector = property.vector2Value;
                float min = vector.x;
                float max = vector.y;

                min = EditorGUI.FloatField(splittedRect[0], float.Parse(min.ToString("F2")));
                max = EditorGUI.FloatField(splittedRect[2], float.Parse(max.ToString("F2")));

                EditorGUI.MinMaxSlider(splittedRect[1], ref min, ref max, minMaxAttribute.Min, minMaxAttribute.Max);
                if(min < minMaxAttribute.Min)
                {
                    min = minMaxAttribute.Min;
                }
                if(max > minMaxAttribute.Max)
                {
                    max = minMaxAttribute.Max;
                }
                vector = new Vector2(min > max ? max : min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = vector;
                }
            }
            else if(variableType == SerializedPropertyType.Vector2Int)
            {
                EditorGUI.BeginChangeCheck();

                Vector2Int vector = property.vector2IntValue;
                float min = vector.x;
                float max = vector.y;

                min = EditorGUI.FloatField(splittedRect[0], min);
                max = EditorGUI.FloatField(splittedRect[2], max);

                EditorGUI.MinMaxSlider(splittedRect[1], ref min, ref max, minMaxAttribute.Min, minMaxAttribute.Max);
                if(min < minMaxAttribute.Min)
                {
                    max = minMaxAttribute.Min;
                }
                if(min > minMaxAttribute.Max)
                {
                    max = minMaxAttribute.Max;
                }
                vector = new Vector2Int(Mathf.FloorToInt(min > max ? max : min), Mathf.FloorToInt(max));

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2IntValue = vector;
                }
            }   
        }

        Rect[] SplitRect(Rect rectToSplit, int n)
        {
            Rect[] rects = new Rect[n];

            for (int i = 0; i < n; i++)
            {
                rects[i] = new Rect(rectToSplit.position.x + (i * rectToSplit.width / n), rectToSplit.position.y, rectToSplit.width / n, rectToSplit.height);
            }

            int padding = (int)rects[0].width - 40;
            int space = 5;
            rects[0].width -= padding + space;
            rects[2].width -= padding + space;

            rects[1].x -= padding;
            rects[1].width += padding * 2;

            rects[2].x += padding + space;

            return rects;
        }
    }
}