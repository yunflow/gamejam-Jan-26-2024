using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        [SerializeField] private Image progressBarImage;

        [SerializeField] private float testHealth = 3;

        private void Start()
        {
            // progressBar.value = 0;
        }

        private void Update()
        {
            UpdateProgressBar();

            if (Input.GetKeyDown(KeyCode.F)) {
                testHealth--;
            }
        }

        private void UpdateProgressBar()
        {
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, testHealth / 3, 0.05f);
            if (testHealth == 3) {
                progressBar.color = Color.green;
            } else if (testHealth == 2) {
                progressBar.color = Color.yellow;
            } else {
                progressBar.color = Color.red;
            }
            
            
        }
    }
}