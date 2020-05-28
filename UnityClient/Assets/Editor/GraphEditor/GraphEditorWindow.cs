using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;

namespace GraphEditor
{
    public class GraphEditorWindow : EditorWindow
    {
        private GraphEditorView graphView;

        //[MenuItem("Window/Graph Editor")]
        public static void OpenGraphEditorWindow()
        {
            var window = CreateWindow<GraphEditorWindow>();
            window.titleContent.text = "Graph Editor";
        }

        private void OnEnable()
        {
            ConstructGraphView();
        }

        private void ConstructGraphView()
        {
            graphView = new GraphEditorView()
            {
                name = "Graph Editor"
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
