using UnityEngine;
using UnityEditor;
using System;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(RandomizeButtonAttribute))]
    public class RandomizeButtonAttributeDrawer : PropertyDrawer
    {
        float randomizeButtonWidth = 20f;
        float randomizeButtonIconSize = 20f;
        float foldoutWidth = 20;
        int maxDigitsCount = 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(RandomizeButtonAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with float and int)");
                return;
            }

            // Get current attribute
            RandomizeButtonAttribute currentAttribute = attribute as RandomizeButtonAttribute;

            // Draw property
            Rect propertyRect = new Rect(position.xMin, position.yMin, position.width - randomizeButtonWidth - foldoutWidth - 5, position.height);
            EditorGUI.PropertyField(propertyRect, property, label);
            
            // Get clamped digits number
            int digitsCount = Mathf.Clamp(currentAttribute.Digits, 0, maxDigitsCount);

            // Draw randomize button
            Rect buttonRect = new Rect(position.xMax - randomizeButtonWidth - foldoutWidth - 3, position.yMin, randomizeButtonWidth, position.height);
            if (GUI.Button(buttonRect, ""))
            {
                // Get random value between min and max
                float randomValue = UnityEngine.Random.Range(currentAttribute.MinValue, currentAttribute.MaxValue);

                if(property.propertyType == SerializedPropertyType.Float)
                {
                    // Set digits on random value and set variable
                    float randomValueWithDigits = (float)Math.Round(randomValue, digitsCount);
                    property.floatValue = randomValueWithDigits;
                }
                else if(property.propertyType == SerializedPropertyType.Integer)
                {
                    // Round random value to int and set variable
                    int roundedRandomValue = Mathf.RoundToInt(randomValue);
                    property.intValue = roundedRandomValue;
                }      
            }

            // Draw randomize button icon
            Rect randomizeButtonIconRect = new Rect(position.xMax - randomizeButtonWidth - foldoutWidth - 3, position.yMin - 1, randomizeButtonIconSize, randomizeButtonIconSize);
            GUI.Label(randomizeButtonIconRect, EditorGUIUtility.IconContent("Refresh"));

            // Draw foldout infos
            Rect foldoutRect = new Rect(position.xMax - foldoutWidth, position.yMin, foldoutWidth, position.height);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "");
            if (property.isExpanded)
            {
                // Start drawing background
                EditorGUILayout.BeginVertical(GUI.skin.box);

                // Draw foldout infos
                EditorGUILayout.LabelField(new GUIContent("Min : " + currentAttribute.MinValue + ", Max : " + currentAttribute.MaxValue + ", Digits : " + digitsCount, "Min : " + currentAttribute.MinValue + ", Max : " + currentAttribute.MaxValue + ", Digits : " + digitsCount));

                // End drawing background
                EditorGUILayout.EndVertical();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    return true;
                case SerializedPropertyType.Integer:
                    return true;
                default:
                    return false;
            }
        }
    }
}