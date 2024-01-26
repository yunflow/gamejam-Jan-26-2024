using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;
        [SerializeField] private Image progressBarImage;

        private void Start()
        {
            progressBar.value = 0;
        }

        private void Update()
        {
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {

        }
    }
}