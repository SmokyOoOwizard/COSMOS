using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.UI
{
    public class SlotUI : MonoBehaviour
    {
        public delegate void OnDropInSlotEvent(SlotUI slot, ICanPlaceInSlot newStuff, ICanPlaceInSlot oldStuff);
        public const string DEFAULT_EMPTY_SLOT_IMAGE_ID = "";
        public event OnDropInSlotEvent OnDropInSlot;
        Func<SlotUI, ICanPlaceInSlot, bool> customAcceptFunc;

        public ICanPlaceInSlot CurrentStuff { get; protected set; }
        public string EmptySlotImageID;


        public void SetCustomAcceptFunc(Func<SlotUI, ICanPlaceInSlot, bool> acceptFunc)
        {
            customAcceptFunc = acceptFunc;
        }
        public void ClearCustomAcceptFunc()
        {
            customAcceptFunc = null;
        }

        public virtual bool AcceptStuff(ICanPlaceInSlot stuff)
        {
            if (customAcceptFunc == null || customAcceptFunc(this, stuff))
            {
                CurrentStuff = stuff;
                if (OnDropInSlot != null)
                {
                    OnDropInSlot(this, stuff, CurrentStuff);
                }
                return true;
            }
            return false;
        }
        public void SetStuff(ICanPlaceInSlot stuff)
        {
            CurrentStuff = stuff;
        }

        public virtual void UpdateData()
        {
            // update icons and others
        }
    }
}
