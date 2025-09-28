using TMPro;
using UnityEngine;

namespace NoMorePals
{
    public class LineController : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        
        [SerializeField] private Texture[] lineTextures;


        [SerializeField] private float fps = 30f;
        
        private int animationStep;
        private float fptCounter;
    }
}
