using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class DialogNode
    {
        public virtual string Text { get; }

        public virtual bool HasNextNode()
        {
            return false;
        }
        public virtual DialogNode GetNextNode()
        {
            return null;
        }
        public virtual bool HasDialogAction()
        {
            return false;
        }
        public virtual Action GetDialogAction()
        {
            return null;
        }
    }
    public class EntryNode : DialogNode
    {
        public class DialogEntryNodeData : DialogNodeData
        {
            public string NextNodeGUID;

            public override DialogNode restoreData()
            {
                return new EntryNode();
            }

            public override bool restoreLinks(DialogNode data, Dictionary<string, DialogNode> nodes)
            {
                if (data is EntryNode)
                {
                    if (nodes.ContainsKey(NextNodeGUID))
                    {
                        (data as EntryNode).NextNode = nodes[NextNodeGUID];
                        return true;
                    }
                }
                return false;
            }
        }

        public string Name;

        public DialogNode NextNode { get; private set; }

        public override bool HasNextNode()
        {
            return NextNode != null;
        }
        public override DialogNode GetNextNode()
        {
            return NextNode;
        }
    }
}
