using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Dialogs
{
    public class DialogNode
    {
        public string ID { get; private set; }
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

        public static DialogNode CreateNode(RawDialogNode node)
        {
            if (node != null)
            {
                if (!string.IsNullOrEmpty(node.Type))
                {
                    var classes = Core.AttributeFinder.GetTypesWithAttribute(typeof(DialogNodeAttribute));
                    foreach (var att in classes)
                    {
                        if ((att.Key as DialogNodeAttribute).name == node.Type)
                        {
                            if (att.Value.IsSubclassOf(typeof(DialogNode)))
                            {
                                var instance = Activator.CreateInstance(att.Value) as DialogNode;
                                instance.restore(node);
                                return instance;
                            }
                            else
                            {
                                Log.Error($"Dialog node with Type:{node.Type} is not DialogNode. ID:{node.GUID}", "Dialog");
                                break;
                            }
                        }
                    }
                }
            }
            return null;
        }
        protected virtual void restore(RawDialogNode node)
        {
            if(node != null)
            {
                ID = node.GUID;
            }
        }
        public virtual void RestoreLinks(Dialog dialog)
        {

        }
    }

    [DialogNode("StartNode")]
    public class EntryNode : DialogNode
    {
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
    public class DialogNodeAttribute : Attribute
    {
        public string name;
        public DialogNodeAttribute(string name)
        {
            this.name = name;
        }
    }
}
