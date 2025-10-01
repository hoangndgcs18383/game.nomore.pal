using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoMorePals
{
    public class BlackBorderController : MonoBehaviour
    {
        [SerializeField] private DoorComponent doorComponentA;
        [SerializeField] private DoorComponent doorComponentB;

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "Level3")
            {
                doorComponentA.gameObject.SetActive(true);
                doorComponentB.gameObject.SetActive(true);
            }
            else
            {
                doorComponentA.gameObject.SetActive(false);
                doorComponentB.gameObject.SetActive(false);
            }
        }
    }
}