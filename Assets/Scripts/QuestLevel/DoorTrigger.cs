using UnityEngine;

namespace NoMorePals
{
    public class DoorTrigger : MonoBehaviour
    {
        public string DoorId;
        public DoorComponent doorComponent;

        private void Start()
        {
            DoorComponent[] doorComponents = FindObjectsOfType<DoorComponent>(true);

            foreach (var door in doorComponents)
            {
                if (door.DoorId == DoorId)
                {
                    doorComponent = door;
                    doorComponent.gameObject.SetActive(true);
                    break;
                }
            }
            
            Vector3 doorScreenPos = Camera.main.WorldToScreenPoint(doorComponent.triggerTransform.position);
            doorScreenPos.z = 0;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(doorScreenPos);
            worldPos.z = 0;
            transform.position = worldPos;
        }
    }
}