using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        private TimelineManager tlm;

        private void Start()
        {
            tlm = FindObjectOfType<TimelineManager>();
        }

        private void Update()
        {
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, tlm.points / 3, 0.05f);
            if (tlm.points == 3) {
                progressBar.color = Color.green;
            } else if (tlm.points == 2) {
                progressBar.color = Color.yellow;
            } else {
                progressBar.color = Color.red;
            }
            
        }
    }
}