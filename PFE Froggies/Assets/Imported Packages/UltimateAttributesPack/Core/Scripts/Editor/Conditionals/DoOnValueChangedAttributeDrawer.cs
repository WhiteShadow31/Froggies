using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(DoOnValueChangedAttribute))]
    public class DoOnValueChangedAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get current attribute
            DoOnValueChangedAttribute currentAttribute = attribute as DoOnValueChangedAttribute;

            // Get target function script
            MonoBehaviour targetScript = null;
            MonoBehaviour targetObject = property.serializedObject.targetObject as MonoBehaviour; // Get target object
            if(targetObject.gameObject.GetComponent(currentAttribute.ClassType) != null)
            {
                targetScript = targetObject.GetComponent(currentAttribute.ClassType) as MonoBehaviour;
            }
            // If target script is not found in object, log warning and return
            else
            {
                Debug.LogWarning($"{nameof(DoOnValueChangedAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {currentAttribute.ClassType.Name} not found in object");
                return;
            }

            // If function is not found, log warning and return
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            if (currentAttribute.ClassType.GetMethod(currentAttribute.FunctionName, bindingFlags) == null)
            {
                Debug.LogWarning($"{nameof(DoOnValueChangedAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : Function {currentAttribute.FunctionName} not found in {currentAttribute.ClassType.Name}");
                return;
            }

            EditorGUI.BeginChangeCheck();

            // Draw property
            EditorGUI.PropertyField(position, property, label);

            // If variable has changed, execute function
            if (EditorGUI.EndChangeCheck()) 
            {
                property.serializedObject.ApplyModifiedProperties();

                // Save run in edit mode state and enable it
                bool runInEditModeState = targetScript.runInEditMode;
                targetScript.runInEditMode = true;
                
                // Execute function
                targetScript.SendMessage(currentAttribute.FunctionName);

                // Reset run in edit mode state
                targetScript.runInEditMode = runInEditModeState;
            }           
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        // Check if target function exists in target script
        bool TargetFunctionFound(string functionName, MonoBehaviour script)
        {
            foreach(MethodInfo methodInfo in script.GetType().GetMethods())
            {
                if(methodInfo.Name == functionName)
                    return true;
            }
            return false;
        }
    }
}