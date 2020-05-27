using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    [Serializable]
    public class DialogData
    {
        public EntryNode.DialogEntryNodeData[] EntryNodes;
        public DialogNodeData[] Nodes;
    }
}
