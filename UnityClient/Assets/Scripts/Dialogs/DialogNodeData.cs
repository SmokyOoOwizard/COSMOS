using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    [Serializable]
    public abstract class DialogNodeData
    {
        public string GUID;

        public abstract DialogNode restoreData();
        public abstract bool restoreLinks(DialogNode data, Dictionary<string, DialogNode> nodes);

    }
}

