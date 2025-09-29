using TMPro;
using UnityEngine;

namespace NoMorePals
{
    public class TextTurn : MonoBehaviour
    {
        [SerializeField] private TMP_Text turnText;

        private void OnEnable()
        {
            GameManager.Instance.OnTurnChanged += UpdateTurnText;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnTurnChanged -= UpdateTurnText;
        }

        private void UpdateTurnText(int currentTurn, ILevel totalTurns)
        {
            string tag = currentTurn <= 0 ? "<color=red>" : "<color=white>";
            
            turnText.text = $"Action: {tag}{currentTurn}</color>/{totalTurns.GetLevelTurns()}";
        }
    }
}