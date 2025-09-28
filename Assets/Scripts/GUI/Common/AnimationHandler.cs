using UnityEngine;

namespace NoMorePals
{
    public class AnimationHandler : MonoBehaviour
    {
        private Animator animator;
        
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        
        public void SetBool(string animationName, bool value)
        {
            if (animator != null)
            {
                int hash = Animator.StringToHash(animationName);
                animator.SetBool(hash, value);
            }
            else
            {
                Debug.LogWarning("Animator component not found.");
            }
        }
    }
}