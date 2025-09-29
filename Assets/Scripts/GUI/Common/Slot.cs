using SAGE.Framework.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoMorePals
{
    public struct SlotData
    {
        public string id;
        public Sprite sprite;
        public int count;
        public QuestTrigger questTrigger;
    }

    public class Slot : DraggableItem
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        private bool _activeQuest = false;

        public override void SetData(SlotData data)
        {
            base.SetData(data);
            UpdateTextCount();
        }

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

        public override void OnPointUp()
        {
            base.OnPointUp();
            _data.count--;
            UpdateTextCount();
        }

        private void UpdateTextCount()
        {
            _countText.text = $"X{_data.count}";
            if (_data.count <= 0)
            {
                _icon.color = Color.gray;
                _canvasGroup.blocksRaycasts = false;
            }
            else
            {
                _icon.color = new Color(255f, 255f, 255f, 255f);
                _canvasGroup.blocksRaycasts = true;
            }
        }

        public void Show()
        {
        }
    }
}