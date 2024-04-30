using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(GetComponentInParentAttribute))]
    public class GetComponentInParentAttributeDrawer : PropertyDrawer
    {
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else log warning and return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(GetComponentInParentAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with object reference, exposed reference and managed reference)");
                return;
            }

            // Draw property
            EditorGUI.PropertyField(position, property);

            // Get and set component
            GetAndSetComponent(property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        void GetAndSetComponent(SerializedProperty property)
        {
            // Get current attribute
            GetComponentInParentAttribute currentAttribute = attribute as GetComponentInParentAttribute;

            MonoBehaviour target = property.serializedObject.targetObject as MonoBehaviour; // Get object

            if (target.gameObject.transform.parent.gameObject == null) // If game object's parent is null, return
                return;

            FieldInfo[] fields = target.GetType().GetFields(bindingFlags); // Get all fields of the script

            foreach (FieldInfo info in fields)
            {
                IEnumerable<Attribute> attributes = info.GetCustomAttributes(); // Get all custom attributes
                foreach (Attribute attribute in attributes)
                {
                    // If the field don't have current attribute, continue
                    if (attribute is not GetComponentInParentAttribute)
                        continue;

                    // If property is an array or a list, log warning and return
                    if (info.FieldType.IsArray)
                    {
                        Debug.LogWarning($"{nameof(GetComponentInParentAttribute)} of {property.displayName} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Arrays and lists are not supported");
                        return;
                    }

                    if (target.gameObject.transform.parent.gameObject.TryGetComponent(info.FieldType, out Component component)) // If component is present in game object, get it and set it
                        info.SetValue(target, component);
                    else if (component == null && currentAttribute.CreateOneIfEmpty) // Else, add it if CreateOneIfEmpty is true
                        target.gameObject.transform.parent.gameObject.AddComponent(info.FieldType);
                }
            }
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                    return true;
                case SerializedPropertyType.ExposedReference:
                    return true;
#if UNITY_2021_2_OR_NEWER
                case SerializedPropertyType.ManagedReference:
                    return true;
#endif
            }

            return false;
        }
    }
}
