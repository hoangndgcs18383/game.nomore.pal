using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NoMorePals
{
    public abstract class BaseQuest : MonoBehaviour, ILevel
    {
        public abstract int LevelIndex { get; }
        public int GetLevelTurns() => QuestTargets.Count;
        protected abstract Dictionary<string, bool> QuestTargets { get; set;}
        public void SetQuestComplete(string questID)
        {
            if (QuestTargets.ContainsKey(questID))
            {
                QuestTargets[questID] = true;
                Debug.Log($"Quest {questID} completed.");
            }
            else
            {
                Debug.LogWarning($"Quest ID {questID} not found.");
            }
        }

        public bool AreAllQuestsComplete()
        {
            return QuestTargets.All(q => q.Value);
        }

        public abstract UniTask StartLevel(MagnetBlock magnetA, MagnetBlock magnetB);
    }
}