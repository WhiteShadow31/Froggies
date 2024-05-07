using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(AddSubtractButtonsAttribute))]
    public class AddSubtractButtonsAttributeDrawer : PropertyDrawer
    {
        float buttonWidth = 20f;
        float foldoutWidth = 20;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(AddSubtractButtonsAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with float and int)");
                return;
            }

            // Get current attribute
            AddSubtractButtonsAttribute currentAttribute = attribute as AddSubtractButtonsAttribute;

            // Draw property
            Rect propertyRect = new Rect(position.xMin, position.yMin, position.width - (buttonWidth * 2) - foldoutWidth - 5, position.height);
            EditorGUI.PropertyField(propertyRect, property, label);

            // Draw substract button
            Rect substractButtonRect = new Rect(position.xMax - (buttonWidth * 2) - foldoutWidth - 3, position.yMin, buttonWidth, position.height);
            if(GUI.Button(substractButtonRect, "-"))
            {
                if(property.propertyType == SerializedPropertyType.Float)
                    property.floatValue -= Mathf.Abs(currentAttribute.SubtractValue); // Substract float value
                else if(property.propertyType == SerializedPropertyType.Integer)                
                    property.intValue -= Mathf.CeilToInt(Mathf.Abs(currentAttribute.SubtractValue)); // Substract int value
            }

            // Draw add button
            Rect addButtonRect = new Rect(position.xMax - buttonWidth - foldoutWidth - 3, position.yMin, buttonWidth, position.height);
            if (GUI.Button(addButtonRect, "+"))
            {
                if (property.propertyType == SerializedPropertyType.Float)
                    property.floatValue += currentAttribute.AddValue; // Add float value
                else if (property.propertyType == SerializedPropertyType.Integer)
                    property.intValue += Mathf.CeilToInt(currentAttribute.AddValue); // Add int value
            }

            // Draw foldout
            Rect foldoutRect = new Rect(position.xMax - foldoutWidth, position.yMin, foldoutWidth, position.height);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "");

            // Draw foldout infos
            if (property.isExpanded)
            {
                // Start draw background
                EditorGUILayout.BeginVertical(GUI.skin.box);

                // Draw infos
                EditorGUILayout.LabelField(new GUIContent("Substract : " + Mathf.Abs(currentAttribute.SubtractValue) + ". Add : " + Mathf.Abs(currentAttribute.AddValue), "Substract : " + Mathf.Abs(currentAttribute.SubtractValue) + ". Add : " + Mathf.Abs(currentAttribute.AddValue)));

                // End draw background
                EditorGUILayout.EndVertical();
            }
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