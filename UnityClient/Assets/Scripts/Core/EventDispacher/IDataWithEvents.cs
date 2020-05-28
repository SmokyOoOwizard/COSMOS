using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core.EventDispacher
{
    public interface IDataWithEvents
    {
        void Notify(uint flag = uint.MaxValue, EventArg args = null);
        void AddListener(Action<uint, EventArg> listener, uint flag);
        void RemoveListener(Action<uint, EventArg> listener, uint flag);
        bool ContaintsListener(Action<uint, EventArg> listener, uint flag);
    }
}
