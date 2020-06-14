using System;

namespace COSMOS.Core
{
    public abstract class AData
    {
        public bool Deleted { get; private set; } = false;
        public bool Immortal { get; private set; } = false;
        public Action<AData> OnDelete;

        public void Delete()
        {
            if (!Deleted && !Immortal)
            {
                Deleted = true;
                OnDelete?.Invoke(this);
            }
        }
        public void MarkAsImmortal()
        {
            Immortal = true;
        }
    }
}
