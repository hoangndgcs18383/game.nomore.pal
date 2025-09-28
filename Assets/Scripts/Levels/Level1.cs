using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SAGE.Framework.Core.Addressable;
using SAGE.Framework.UI;
using UnityEngine;

namespace NoMorePals
{
    public interface ILevel
    {
        UniTask StartLevel(MagnetBlock magnetA, MagnetBlock magnetB);
        int GetLevelTurns();
        void SetQuestComplete(string questID);
        bool AreAllQuestsComplete();
    }

    public class Level1 : MonoBehaviour, ILevel
    {
        public int GetLevelTurns() => _questTargets.Count;

        private Dictionary<string, bool> _questTargets = new Dictionary<string, bool>
        {
            { Constants.TableQuestID, false },
            { Constants.GoToPcScene, false }
        };

        public void SetQuestComplete(string questID)
        {
            if (_questTargets.ContainsKey(questID))
            {
                _questTargets[questID] = true;
                Debug.Log($"Quest {questID} completed.");
            }
            else
            {
                Debug.LogWarning($"Quest ID {questID} not found.");
            }
        }

        public bool AreAllQuestsComplete()
        {
            return _questTargets.All(q => q.Value);
        }

        public async UniTask StartLevel(MagnetBlock magnetA, MagnetBlock magnetB)
        {
            Debug.Log("Level 1 Started");
            await UniTask.WaitUntil(() => !magnetA.IsMagnetizing() && !magnetB.IsMagnetizing());
            List<SlotData> slots = new List<SlotData>
            {
                new SlotData { id = Constants.TableQuestID },
            };
            SlotUIData uiData = new SlotUIData { slots = slots };
            await UIManager.Instance.ShowAndLoadScreenAsync<UIGameplay>(BaseScreenAddress.UIGAMEPLAY, uiData);
        }
    }
}