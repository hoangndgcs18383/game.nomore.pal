using System;
using System.Collections.Generic;
using System.Linq;
using SAGE.Framework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace NoMorePals
{
    public struct SlotUIData : IUIData
    {
        public List<SlotData> slots;
    }

    [Serializable]
    public struct SpriteData
    {
        public string id;
        public Sprite sprite;
    }

    public class UIGameplay : BaseScreen
    {
        [SerializeField] private SpriteData[] spritesData;
        [SerializeField] private Slot slotPrefab;
        [SerializeField] private Transform slotsParent;
        [SerializeField] private Button playButton;

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

                slot.SetSprite(GetSpriteByID(_uiData.slots[i].id));
                slot.SetData(_uiData.slots[i]);
            }
        }

        public Sprite GetSpriteByID(string id)
        {
            Sprite sprite = spritesData.FirstOrDefault(s => s.id == id).sprite;
            return sprite;
        }
    }
}