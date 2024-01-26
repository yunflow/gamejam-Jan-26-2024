using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] public SpriteRenderer oscarHead;

    private TimelineManager timelineManager;

    public bool isFail;


    private void Awake()
    {
        timelineManager = GetComponent<TimelineManager>();
    }

    private void Update()
    {
        if (isFail) return;

        ChangeColorForDebug();
        DetectTimeline();
    }

    private void DetectTimeline()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerFailEarly();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            TriggerFailLate();
        }
    }

    private void TriggerFailEarly()
    {
        isFail = true;
        oscarHead.color = Color.cyan;
        print("oh no");
    }

    private void TriggerFailLate()
    {
        isFail = true;
        print("oh no");
    }

    public void DetectSuccess()
    {
        if (isFail)
        {
            print("Fail");
        }
        else
        {
            print("Success");
            oscarHead.color = Color.red;
        }
    }


    private void ChangeColorForDebug()
    {
        image.color = Input.GetKey(KeyCode.Space) ? Color.red : Color.green;
    }
}