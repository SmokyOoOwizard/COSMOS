using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogEditor
{
    [Serializable]
    public class DialogEditorContainer : ScriptableObject
    {
        public List<DialogNodeData> Nodes = new List<DialogNodeData>();
    }
}
