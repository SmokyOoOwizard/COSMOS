using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using COSMOS.Dialogs;

namespace DialogEditor
{
    [Serializable]
    public class DialogEditorContainer : ScriptableObject
    {
        public List<DialogNodeData> Nodes = new List<DialogNodeData>();


        public string Export()
        {
            var js = JsonSerializer.CreateDefault();
            using(MemoryStream ms = new MemoryStream())
            {
                using(TextWriter tw = new StreamWriter(ms))
                {
                    var rawDialog = ConvertToRawDialog();
                    js.Serialize(tw, rawDialog);
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public RawDialog ConvertToRawDialog()
        {
            var raw = new RawDialog();

            foreach (var node in Nodes)
            {
                try
                {
                    Type type = Type.GetType(node.Type);
                    if (type != null)
                    {
                        if (type.IsSubclassOf(typeof(DialogNode)))
                        {
                            var instance = Activator.CreateInstance(type) as DialogNode;

                            instance.Restore(node);

                            var ex = instance.Export();
                            if(ex != null)
                            {
                                if(instance is EntryNode)
                                {
                                    raw.EntryNodes.Add(ex);
                                }
                                raw.Nodes.Add(ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }

            return raw;
        }
    }
}
