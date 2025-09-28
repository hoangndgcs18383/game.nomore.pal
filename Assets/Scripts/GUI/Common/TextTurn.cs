using TMPro;
using UnityEngine;

namespace NoMorePals
{
    public class TextTurn : MonoBehaviour
    {
        [SerializeField] private TMP_Text turnText;

        private void OnEnable()
        {
            GameManager.Instance.OnTurnComplete += UpdateTurnText;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnTurnComplete -= UpdateTurnText;
        }

        private void UpdateTurnText(int currentTurn, int totalTurns)
        {
            string tag = currentTurn < totalTurns ? "<color=red>" : "<color=green>";
            
            turnText.text = $"Action: {tag}{currentTurn}</color>/{totalTurns}";
        }
    }
}