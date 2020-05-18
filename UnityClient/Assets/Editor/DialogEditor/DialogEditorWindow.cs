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

        [MenuItem("Window/Dialog Editor")]
        public static void OpenGraphEditorWindow()
        {
            var window = CreateWindow<DialogEditorWindow>();
            window.titleContent.text = "Dialog Editor";

        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            toolbar.Add(new Button(() => Clear()) { text = "Clear" });
            toolbar.Add(new Button(() => Save()) { text = "Save Data" });
            toolbar.Add(new Button(() => Load()) { text = "Load Data" });
            rootVisualElement.Add(toolbar);
        }

        public void Load()
        {
            AssetDatabase.Refresh();
            var data = AssetDatabase.LoadAssetAtPath<DialogEditorContainer>(@"Assets/TestDialog.asset");
            if (data != null)
            {
                Clear();
                graphView.Load(data);
            }
            else
            {
                Debug.Log("Dialog not found");
            }
        }
        public void Save()
        {
            var data = graphView.Save();
            AssetDatabase.CreateAsset(data, @"Assets/TestDialog.asset");
            AssetDatabase.SaveAssets();
        }
        public void Clear()
        {
            graphView.ClaerGraph();
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