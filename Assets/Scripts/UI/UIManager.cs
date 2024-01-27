using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Energy Bar")]
        [SerializeField] private Image progressBar;

        [Header("Bubble")]
        [SerializeField] private Animator bubbleAni; 
        [SerializeField] private Image thirdBubble; 
        [SerializeField] private Sprite hahaReactBubble; 
        [SerializeField] private Sprite yeaReactBubble; 

        private TimelineManager tlm;

        private void Start()
        {
            tlm = FindObjectOfType<TimelineManager>();
        }

        private void Update()
        {
            UpdateProgressBar();

            if (Input.GetKeyDown(KeyCode.F)) {
                ShowReactBubble();
            }
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

        private void ShowReactBubble() {
            if (tlm.playerReactNumber == 0) {
                thirdBubble.sprite = hahaReactBubble;
            } else {
                thirdBubble.sprite = yeaReactBubble;
            }

            bubbleAni.SetTrigger("ShowBubble");
        }

    }
}