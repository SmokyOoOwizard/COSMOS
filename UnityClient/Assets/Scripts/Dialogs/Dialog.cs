using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class Dialog
    {
        public ReadOnlyCollection<DialogNode> EntryNodes => entryNodes.AsReadOnly();
        private List<DialogNode> entryNodes;
        public ReadOnlyDictionary<string, DialogNode> Nodes => new ReadOnlyDictionary<string, DialogNode>(nodes);
        private Dictionary<string, DialogNode> nodes;

        public RawDialog RawDialog { get; private set; }
        public Dialog()
        {

        }
        public Dialog(RawDialog rawDialog)
        {
            RawDialog = rawDialog;

            if (rawDialog != null)
            {
                foreach (var node in rawDialog.EntryNodes)
                {
                    var dNode = DialogNode.CreateNode(node);
                    entryNodes.Add(dNode);
                    nodes.Add(dNode.ID, dNode);
                }

                foreach (var node in rawDialog.Nodes)
                {
                    var dNode = DialogNode.CreateNode(node);
                    if (!nodes.ContainsKey(dNode.ID))
                    {
                        nodes.Add(dNode.ID, dNode);
                    }
                }

                foreach (var node in nodes)
                {
                    node.Value.RestoreLinks(this);
                }
            }
        }
    }
}
