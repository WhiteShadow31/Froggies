using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;

namespace UltimateAttributesPack.Editors
{
    [CustomPropertyDrawer(typeof(OpenFolderButtonAttribute))]
    public class OpenFolderButtonAttributeDrawer : DecoratorDrawer
    {
        bool isExpanded;
        float buttonHeight = 20f;
        float folderIconSize = 20f;
        float foldoutWidth = 20;

        public override void OnGUI(Rect position)
        {
            // Get current attribute
            OpenFolderButtonAttribute currentAttribute = attribute as OpenFolderButtonAttribute;

            // If folder or file not found with path, log warning and return
            if (!AssetDatabase.IsValidFolder(currentAttribute.FolderPath) && !File.Exists(currentAttribute.FolderPath))
            {
                Debug.LogWarning($"{nameof(OpenFolderButtonAttribute)} : Folder or file not found with path {currentAttribute.FolderPath}");
                return;
            }

            // Draw open file button
            Rect buttonRect = new Rect(position.xMin, position.yMin + 5, position.width - foldoutWidth - 5, buttonHeight);
            if (GUI.Button(buttonRect, currentAttribute.Text))             
                OpenFile(currentAttribute.FolderPath); // Open file if button clicked

            // Calculate button text width
            float textWidth = GUI.skin.box.CalcSize(new GUIContent(currentAttribute.Text)).x;

            // Draw folder icon
            Rect folderIconRect = new Rect(position.xMax / 2 - textWidth / 2 - folderIconSize - 10, position.yMin + 5, folderIconSize, folderIconSize);
            if(EditorGUIUtility.IconContent("Folder Icon") != null)
            {
                GUIContent folderIcon = EditorGUIUtility.IconContent("Folder Icon");
                EditorGUI.LabelField(folderIconRect, folderIcon);
            }

            // Draw foldout button
            Rect foldoutRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
            isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, "");
            if (isExpanded)
            {
                EditorGUILayout.Space(buttonHeight + 2); // Set space          
                EditorGUILayout.BeginVertical(GUI.skin.box); // Start drawing background
                EditorGUILayout.LabelField(new GUIContent($"Open folder : {currentAttribute.FolderPath}")); // Draw foldout infos
                EditorGUILayout.EndVertical(); // End drawing background
            }
        }

        public override float GetHeight()
        {
            return buttonHeight + (isExpanded ? EditorGUIUtility.singleLineHeight + 3 : 0) + 10;
        }

        // Open specific project file with path
        private void OpenFile(string path)
        {
            var getInstanceIDMethod = typeof(AssetDatabase).GetMethod("GetMainAssetInstanceID",
            BindingFlags.Static | BindingFlags.NonPublic);
            int instanceID = (int)getInstanceIDMethod.Invoke(null, new object[] { path });

            Assembly editorAssembly = typeof(Editor).Assembly;
            System.Type projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");

            MethodInfo showFolderContents = projectBrowserType.GetMethod("ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic);
            Object[] projectBrowserInstances = Resources.FindObjectsOfTypeAll(projectBrowserType);
            if(projectBrowserInstances.Length > 0)
            {
                for(int i = 0; i < projectBrowserInstances.Length; i++)
                {
                    SerializedObject serializedObject = new SerializedObject(projectBrowserInstances[i]);
                    bool inTwoColumnMode = serializedObject.FindProperty("m_ViewMode").enumValueIndex == 1;

                    if (!inTwoColumnMode)
                    {
                        MethodInfo setTwoColumns = projectBrowserInstances[i].GetType().GetMethod(
                            "SetTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
                        setTwoColumns.Invoke(projectBrowserInstances[i], null);
                    }

                    bool revealAndFrameInFolderTree = true;
                    showFolderContents.Invoke(projectBrowserInstances[i], new object[] { instanceID, revealAndFrameInFolderTree });
                }
            }
            else
            {
                EditorWindow projectBrowser = OpenNewProjectBrowser(projectBrowserType);

                SerializedObject serializedObject = new SerializedObject(projectBrowser);
                bool inTwoColumnMode = serializedObject.FindProperty("m_ViewMode").enumValueIndex == 1;

                if (!inTwoColumnMode)
                {
                    MethodInfo setTwoColumns = projectBrowser.GetType().GetMethod(
                        "SetTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
                    setTwoColumns.Invoke(projectBrowser, null);
                }

                bool revealAndFrameInFolderTree = true;
                showFolderContents.Invoke(projectBrowser, new object[] { instanceID, revealAndFrameInFolderTree });
            }
        }

        private EditorWindow OpenNewProjectBrowser(System.Type projectBrowserType)
        {
            EditorWindow projectBrowser = EditorWindow.GetWindow(projectBrowserType);
            projectBrowser.Show();

            MethodInfo init = projectBrowserType.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
            init.Invoke(projectBrowser, null);

            return projectBrowser;
        }
    }
}