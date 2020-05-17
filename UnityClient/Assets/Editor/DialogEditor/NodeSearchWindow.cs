using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogEditor
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogEditorView editorView;
        private DialogEditorWindow editorWindow;

        private Texture2D indentationIcon;

        public void Init(DialogEditorWindow window, DialogEditorView view)
        {
            editorView = view;
            editorWindow = window;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
                new SearchTreeGroupEntry(new GUIContent("Checks"), 1),
                new SearchTreeEntry(new GUIContent("Entry Node", indentationIcon))
                {
                    userData = new EntryNode(), level = 1
                },
                new SearchTreeEntry(new GUIContent("Speech Node", indentationIcon))
                {
                    userData = new SpeechNode(), level = 1
                },
                new SearchTreeEntry(new GUIContent("Choice Node", indentationIcon))
                {
                    userData = new ChoiceNode(), level = 1
                }
            };
            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            if (editorView != null)
            {
                var worldMousePosition =
                    editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent,
                    context.screenMousePosition - editorWindow.position.position);
                var localMousePosition = editorView.contentViewContainer.WorldToLocal(worldMousePosition);
                switch (SearchTreeEntry.userData)
                {
                    case ChoiceNode cn:
                    case SpeechNode sn:
                    case EntryNode en:
                        var node = SearchTreeEntry.userData as Node;
                        editorView.AddElement(node);
                        var rect = node.GetPosition();
                        rect.position = localMousePosition;
                        node.SetPosition(rect);
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
