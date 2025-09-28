using System.Collections.Generic;
using SAGE.Framework.UI;
using UnityEngine;

namespace NoMorePals
{
    public struct SlotUIData : IUIData
    {
        public List<SlotData> slots;
    }

    public class UIGameplay : BaseScreen
    {
        [SerializeField] private Slot slotPrefab;
        [SerializeField] private Transform slotsParent;

        private List<Slot> _slots = new List<Slot>();
        private SlotUIData _uiData;

        public override void SetData(IUIData data = null)
        {
            base.SetData(data);

            if (data is SlotUIData slotUIData)
            {
                _uiData = slotUIData;
            }
        }

        public override void Show()
        {
            base.Show();
            PopulateSlots();
        }

        public override void Hide()
        {
            base.Hide();
            foreach (var slot in _slots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        private void PopulateSlots()
        {
            for (int i = 0; i < _uiData.slots.Count; i++)
            {
                Slot slot;
                if (i < _slots.Count)
                {
                    slot = _slots[i];
                    slot.Show();
                }
                else
                {
                    slot = Instantiate(slotPrefab, slotsParent);
                    _slots.Add(slot);
                }

                slot.SetData(_uiData.slots[i]);
            }
        }
    }
}