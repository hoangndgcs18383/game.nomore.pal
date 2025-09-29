using UnityEngine;

namespace NoMorePals
{
    public class BlackBorderController : MonoBehaviour
    {
        [SerializeField] private DoorComponent doorComponentA;
        [SerializeField] private DoorComponent doorComponentB;

        private void OnEnable()
        {
            GameManager.Instance.OnTurnChanged += HandleTurnChanged;
        }

        private void HandleTurnChanged(int currentTurn, ILevel level)
        {
            doorComponentA.gameObject.SetActive(level.LevelIndex == 3);
            doorComponentB.gameObject.SetActive(level.LevelIndex == 3);
            GameManager.Instance.OnTurnChanged -= HandleTurnChanged;
        }
    }
}