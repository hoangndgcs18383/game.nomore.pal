using SAGE.Framework.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace NoMorePals
{
    public struct SlotData
    {
        public string id;
        public Sprite sprite;
    }

    public class Slot : DraggableItem
    {
        [SerializeField] private Image _icon;

        private SlotData _data;
        private bool _activeQuest = false;

        public void SetData(SlotData data)
        {
            _data = data;
            gameObject.name = data.id;
        }

        public void SetSprite(Sprite sprite)
        {
            _icon.sprite = sprite;
            _icon.SetNativeSize();
            RectTransform rt = transform as RectTransform;
            if (rt != null) rt.sizeDelta = _icon.sprite.rect.size;
        }

        public void Show()
        {
            if (!IsValid()) gameObject.SetSafeActive(true);
        }

        public override string GetQuestID()
        {
            return _data.id;
        }

        public override bool IsValid()
        {
            return _activeQuest;
        }

        public override bool TriggerEvent(string questID, Collider2D collider = null)
        {
            return _activeQuest = !base.TriggerEvent(questID, collider);
        }
    }
}