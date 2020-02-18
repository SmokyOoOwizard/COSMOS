using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core.EventDispacher
{
    public interface IDataWithEvents
    {
        void Notify(ushort flag = ushort.MaxValue);
        void AddListener(Action<ushort> listener);
        void RemoveListener(Action<ushort> listener);
        bool ContaintsListener(Action<ushort> listener);
    }
}
