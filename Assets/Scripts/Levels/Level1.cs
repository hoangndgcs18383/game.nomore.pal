using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SAGE.Framework.Core.Addressable;
using SAGE.Framework.UI;
using UnityEngine;

namespace NoMorePals
{
    public interface ILevel
    {
        UniTask StartLevel(MagnetBlock magnetA, MagnetBlock magnetB);
        void EndLevel();
    }

    public class Level1 : MonoBehaviour, ILevel 
    {
        public async UniTask StartLevel(MagnetBlock magnetA, MagnetBlock magnetB)
        {
            Debug.Log("Level 1 Started");
            await UniTask.WaitUntil(() => !magnetA.IsMagnetizing() && !magnetB.IsMagnetizing());
            List<SlotData> slots = new List<SlotData>
            {
                new SlotData { id = Constants.TableQuestID },
                new SlotData { id = "" },
                new SlotData { id = "" }
            };
            SlotUIData uiData = new SlotUIData { slots = slots };
            await UIManager.Instance.ShowAndLoadScreenAsync<UIGameplay>(BaseScreenAddress.UIGAMEPLAY, uiData);
        }

        public void EndLevel()
        {
            Debug.Log("Level 1 Ended");
        }
    }
}