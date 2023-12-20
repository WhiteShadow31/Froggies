using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace UltimateAttributesPack
{
    [CustomPropertyDrawer(typeof(OpenProjectFileButtonAttribute))]
    public class OpenProjectFileButtonAttributeDrawer : DecoratorDrawer
    {
        bool isExpanded;
        float foldoutWidth = 20;

        public override void OnGUI(Rect position)
        {
            OpenProjectFileButtonAttribute openProjectFileButtonAttribute = attribute as OpenProjectFileButtonAttribute;

            EditorGUILayout.BeginHorizontal();

            // Display button
            Rect buttonRect = new Rect(position.xMin, position.yMin + 5, position.width - foldoutWidth - 5, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, openProjectFileButtonAttribute.Name))
            {
                OpenFile(openProjectFileButtonAttribute.Path);
            }

            // Display foldout button
            Rect foldoutGroupRect = new Rect(position.xMax - 20, position.yMin + 3, EditorGUIUtility.singleLineHeight, foldoutWidth);
            isExpanded = EditorGUI.Foldout(foldoutGroupRect, isExpanded, "");

            EditorGUILayout.EndHorizontal();

            if (isExpanded)
            {
                Rect textFieldRect = new Rect(position.xMin, position.yMin + EditorGUIUtility.singleLineHeight + 5, position.width - foldoutWidth, EditorGUIUtility.singleLineHeight);
                GUILayout.BeginArea(textFieldRect);           
                EditorGUILayout.LabelField(new GUIContent("Open file : " + openProjectFileButtonAttribute.Path, openProjectFileButtonAttribute.Path), GUILayout.Width(position.width - foldoutWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUILayout.EndArea();
            }
        }

        // Set total spacing
        public override float GetHeight()
        {
            int lines = 1;

            if (isExpanded)
            {
                lines = 2;
            }
            else
            {
                lines = 1;
            }

            return EditorGUIUtility.singleLineHeight * lines + 10;
        }

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