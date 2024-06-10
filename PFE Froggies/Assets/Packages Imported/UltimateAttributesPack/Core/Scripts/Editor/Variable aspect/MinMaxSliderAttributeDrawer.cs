using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(MinMaxSliderAttribute)} of {property.name} in {property.serializedObject.targetObject.name + "." + (property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type is not supported (only work with Vector2 and Vector2Int)");
                return;
            }

            // Get current attribute
            MinMaxSliderAttribute currentAttribute = attribute as MinMaxSliderAttribute;

            SerializedPropertyType type = property.propertyType;

            Rect controlRect = EditorGUI.PrefixLabel(position, label);
            Rect[] splittedRect = SplitRect(controlRect, 3);

            EditorGUI.BeginChangeCheck();

            // Get vector of property
            var vector = type == SerializedPropertyType.Vector2 ? property.vector2Value : property.vector2IntValue;

            // Set and draw min and max values
            float min = vector.x;
            min = EditorGUI.FloatField(splittedRect[0], float.Parse(min.ToString("F2")));
            float max = vector.y;
            max = EditorGUI.FloatField(splittedRect[2], float.Parse(max.ToString("F2")));

            // Draw min max slider
            EditorGUI.MinMaxSlider(splittedRect[1], ref min, ref max, currentAttribute.Min, currentAttribute.Max);

            // Clamp min and max values
            if(min < currentAttribute.Min)
                min = currentAttribute.Min;
            if(max > currentAttribute.Max)
                max = currentAttribute.Max;

            // Set new vector values
            vector = type == SerializedPropertyType.Vector2 ? new Vector2(min > max ? max : min, max) : new Vector2Int(Mathf.FloorToInt(min > max ? max : min), Mathf.FloorToInt(max));

            // If values has changed, set property value
            if (EditorGUI.EndChangeCheck())
            {
                if (type == SerializedPropertyType.Vector2)
                    property.vector2Value = vector;
                else if (type == SerializedPropertyType.Vector2Int)
                    property.vector2IntValue = Vector2Int.RoundToInt(vector);
            } 
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
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

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    return true;
                case SerializedPropertyType.Vector2Int:
                    return true;
                default:
                    return false;
            }
        }
    }
}