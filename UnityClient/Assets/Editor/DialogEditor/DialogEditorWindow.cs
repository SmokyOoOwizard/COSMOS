using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace DialogEditor
{
    public class DialogEditorWindow : EditorWindow
    {
        private DialogEditorView graphView;
        private DialogEditorContainer currentContainer;
        private TextField dialogNameLabel;

        [MenuItem("Window/Dialog Editor")]
        public static DialogEditorWindow OpenGraphEditorWindow()
        {
            var window = CreateWindow<DialogEditorWindow>();
            window.titleContent.text = "Dialog Editor";


            return window;
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject as DialogEditorContainer != null)
            {
                var window = OpenGraphEditorWindow();
                window.Load(Selection.activeObject as DialogEditorContainer);
                return true; //catch open file
            }
            return false; // let unity open the file
        }



        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            dialogNameLabel = new TextField();
            dialogNameLabel.isReadOnly = true;

            toolbar.Add(new Button(() => Clear()) { text = "Clear" });
            toolbar.Add(new Button(() => Save()) { text = "Save Data" });
            toolbar.Add(new Button(() => OnLoadButton()) { text = "Load Data" });
            toolbar.Add(dialogNameLabel);
            rootVisualElement.Add(toolbar);
        }

        private void OnLoadButton()
        {
            AssetDatabase.Refresh();
            if (currentContainer != null)
            {
                if(EditorUtility.DisplayDialog("Save", "Save current open dialog?", "Yes", "No"))
                {
                    Save();
                }
            }
            string path = EditorUtility.OpenFilePanel("Select dialog", "", "asset");
            int pathStart = path.IndexOf("Assets/");
            if(pathStart < 0)
            {
                Debug.LogError("Wrong path: " + path);
                return;
            }
            path = path.Substring(pathStart);
            if (!string.IsNullOrEmpty(path))
            {
                var data = AssetDatabase.LoadAssetAtPath<DialogEditorContainer>(path);
                Load(data);
            }
        }
        public void Load(DialogEditorContainer container)
        {
            if (container != null)
            {
                Clear();
                graphView.Load(container);
                currentContainer = container;
                UpdateDialogName();
            }
            else
            {
                Debug.Log("Dialog is null");
            }
        }
        public void Save()
        {
            var data = graphView.Save();
            string path = string.Empty;
            if (currentContainer != null)
            {
                path = AssetDatabase.GetAssetPath(currentContainer);
            }
            if (string.IsNullOrEmpty(path))
            {
                path = EditorUtility.SaveFilePanelInProject("Save dialog", "New Dialog", "asset", "");
            }
            if (!string.IsNullOrEmpty(path))
            {
                currentContainer = data;
                AssetDatabase.CreateAsset(data, path);
                AssetDatabase.SaveAssets();
                UpdateDialogName();
            }
            else
            {
                Debug.LogError("Save failed. Path is null");
            }
        }
        public void Clear()
        {
            graphView.ClaerGraph();
            currentContainer = null;
        }

        public void UpdateDialogName()
        {
            if (currentContainer != null)
            {
                dialogNameLabel.SetValueWithoutNotify("Name: "+currentContainer.name);
            }
        }
        private void ConstructGraphView()
        {
            graphView = new DialogEditorView(this)
            {
                name = "Dialog Editor"
            };

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
    }
}