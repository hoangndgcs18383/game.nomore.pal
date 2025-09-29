using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SAGE.Framework.Core.Addressable;
using SAGE.Framework.UI;
using UnityEngine;

namespace NoMorePals
{
    public class Level3 : BaseQuest
    {
        public override int LevelIndex => 3;
        
        private Dictionary<string, bool> _questTargets = new Dictionary<string, bool>
        {
            { Constants.UseBarrier, false },
        };

        protected override Dictionary<string, bool> QuestTargets
        {
            get => _questTargets;
            set => _questTargets = value;
        }

        public override async UniTask StartLevel(MagnetBlock magnetA, MagnetBlock magnetB)
        {
            Debug.Log("Level 3 Started");
            await UniTask.WaitUntil(() => !magnetA.IsMagnetizing() && !magnetB.IsMagnetizing());
            List<SlotData> slots = new List<SlotData>
            {
                new SlotData { id = Constants.TableQuestID, count = 0 },
                new SlotData { id = Constants.UseBarrier, count = 1 },
            };
            SlotUIData uiData = new SlotUIData { slots = slots };
            await UIManager.Instance.ShowAndLoadScreenAsync<UIGameplay>(BaseScreenAddress.UIGAMEPLAY, uiData);
        }
    }
}