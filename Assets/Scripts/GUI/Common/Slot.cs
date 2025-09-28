using SAGE.Framework.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace NoMorePals
{
    public struct SlotData
    {
        public string id;
        public Sprite sprite;
        public QuestTrigger questTrigger;
    }

    public class Slot : DraggableItem
    {
        [SerializeField] private Image _icon;
        
        private bool _activeQuest = false;

        public void SetSprite(Sprite sprite)
        {
            _icon.sprite = sprite;
            _icon.SetNativeSize();
            RectTransform rt = transform as RectTransform;
            if (rt != null) rt.sizeDelta = _icon.sprite.rect.size;
            _data.sprite = sprite;
        }

        public void SetQuestTrigger(QuestTrigger questTriggerPrefab)
        {
            _data.questTrigger = questTriggerPrefab;
            Debug.Log($"Quest Trigger set for slot {_data.questTrigger}");
        }

        public void Show()
        {
        }
    }
}