using UnityEngine;
using UnityEditor;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(AssetPreviewAttribute))]
    public class AssetPreviewAttributeDrawer : PropertyDrawer
    {
        float foldoutWidth = 20f;
        float spacingBeforePreview;
        Vector2 previewSize;

        Editor objectEditor;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if property type is supported, else return
            if (!IsTypeSupported(property))
            {
                Debug.LogWarning($"{nameof(AssetPreviewAttribute)} of {property.name} in {property.serializedObject.targetObject.name}.{(property.serializedObject.targetObject as MonoBehaviour).GetType()} : {property.propertyType} type not supported (only work with sprite, texture, texture2D, game object and mesh)");
                return;
            }

            // Get current attribute
            AssetPreviewAttribute currentAttribute = attribute as AssetPreviewAttribute;

            EditorGUI.BeginChangeCheck();

            // Draw property
            Rect propertyRect = new Rect(position.xMin, position.yMin, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property, label);

            // Display foldout button
            Rect foldoutRect = new Rect(position.xMax - foldoutWidth, position.yMin - 1, EditorGUIUtility.singleLineHeight, foldoutWidth);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "");
                      
            if (property.objectReferenceValue != null)
            {
                // Set preview size
                SetPreviewSize(currentAttribute.PreviewSize);

                // If it's a sprite or a texture
                if (IsImage(property))
                {
                    if (property.isExpanded)
                    {
                        // Get preview texture
                        Texture previewTexture = GetPreviewTexture(property);

                        // Draw image preview
                        Rect previewRect = new Rect(position.xMax / 2 - previewSize.x / 2, position.yMin + EditorGUIUtility.singleLineHeight + 5 + spacingBeforePreview, previewSize.x, previewSize.y);
                        GUI.DrawTexture(previewRect, previewTexture, ScaleMode.ScaleToFit);
                    }
                }
                // If it's an object or a mesh
                else
                {
                    // Destroy current preview if property has changed
                    if (EditorGUI.EndChangeCheck())
                    {
                        if(objectEditor != null) Object.DestroyImmediate(objectEditor);
                    }

                    // Draw object preview
                    if(property.isExpanded)
                    {
                            // If there is no preview for the selected object, draw warning helpbox and return
                            if (AssetPreview.GetAssetPreview(property.objectReferenceValue) == null)
                            {
                                Rect warningBoxRect = new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight + 3, position.width, EditorGUIUtility.singleLineHeight);
                                EditorGUI.HelpBox(warningBoxRect, "No preview for this", MessageType.Warning);
                                return;
                            }

                        if (objectEditor == null)

                        objectEditor = Editor.CreateEditor(property.objectReferenceValue); // Create editor for preview
                        Rect previewRect = new Rect(position.xMax / 2 - previewSize.x / 2, position.yMin + EditorGUIUtility.singleLineHeight + 5 + spacingBeforePreview, previewSize.x, previewSize.y);
                        objectEditor.OnInteractivePreviewGUI(previewRect, GUI.skin.box); // Draw preview               
                    }
                }
            }
            // If object is null and foldout is expanded, show warning box
            else if (property.isExpanded)
            {
                Rect warningBoxRect = new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight + 3, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.HelpBox(warningBoxRect, "No object selected", MessageType.Warning);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(property.objectReferenceValue != null && property.isExpanded && AssetPreview.GetAssetPreview(property.objectReferenceValue) != null)              
                return EditorGUIUtility.singleLineHeight + previewSize.y + spacingBeforePreview * 2 + 5; // Draw the property and the preview
            else if(property.isExpanded)
                return EditorGUIUtility.singleLineHeight * 2 + 3; // When drawing warning box if no object selected
            else
                return EditorGUIUtility.singleLineHeight; // When drawing just the property
        }

        // Set size of preview by chosen size 
        void SetPreviewSize(PreviewSize toSize)
        {
            switch (toSize)
            {
                case PreviewSize.Small:
                    previewSize.x = 106;
                    previewSize.y = 60;
                    spacingBeforePreview = 6;
                    return;
                case PreviewSize.Medium:
                    previewSize.x = 160;
                    previewSize.y = 90;
                    spacingBeforePreview = 8;
                    return;
                case PreviewSize.Large:
                    previewSize.x = 214;
                    previewSize.y = 120;
                    spacingBeforePreview = 10;
                    return;
            }
        }

        // Get preview Texture of property
        Texture GetPreviewTexture(SerializedProperty property)
        {
            switch (property.type)
            {
                // If it's a sprite or a texture
                case "PPtr<$Sprite>":
                    return (property.objectReferenceValue as Sprite).texture;
                case "PPtr<$Texture>":
                    return property.objectReferenceValue as Texture;
                case "PPtr<$Texture2D>":
                    return (property.objectReferenceValue as Texture);
            }
            return null;
        }

        // Check if property is a texture
        bool IsImage(SerializedProperty property)
        {
            switch (property.type)
            {
                // If it's a sprite or a texture
                case "PPtr<$Sprite>":
                    return true;
                case "PPtr<$Texture>":
                    return true;
                case "PPtr<$Texture2D>":
                    return true;
                // If it's a game object or a mesh
                default:
                    return false;
            }
        }

        bool IsTypeSupported(SerializedProperty property)
        {
            // Return true if property type is supported
            switch (property.type)
            {
                case "PPtr<$Sprite>":
                    return true;
                case "PPtr<$Texture>":
                    return true;
                case "PPtr<$Texture2D>":
                    return true;
                case "PPtr<$GameObject>":
                    return true;
                case "PPtr<$Mesh>":
                    return true;
                default:
                    return false;
            }
        }
    }
}