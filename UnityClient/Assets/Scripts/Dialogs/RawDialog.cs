using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public sealed class RawDialog
    {
        public List<RawDialogNode> EntryNodes;
        public List<RawDialogNode> Nodes;
    }
    public sealed class RawDialogNode
    {
        public string GUID;
        public string Type;
        public Dictionary<string, string> Data = new Dictionary<string, string>();
    }
}
