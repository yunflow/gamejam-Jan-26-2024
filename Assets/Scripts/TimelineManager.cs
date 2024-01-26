using System.Collections;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private ActionSetSO[] actionSets;

    private int points;
    public bool isSpacePressedDuringUp;


    private void Start()
    {
        StartCoroutine(PerformActionSet(actionSets[0]));
    }

    private void Update()
    {
    }

    private IEnumerator PerformActionSet(ActionSetSO actionSet)
    {
        Vector2[] actions = actionSet.actions;

        foreach (Vector2 action in actions)
        {
            switch ((Action)action.x)
            {
                case Action.Speak:
                {
                    PerformAction(Action.Speak);

                    float speakTime = 0;
                    while (speakTime < action.y)
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            PerformAction(Action.Angry);

                            yield return new WaitForSeconds(3f); // Angry animation
                            print("Finish one set but Fail");
                            yield break;
                        }

                        speakTime += Time.deltaTime;
                        yield return null;
                    }

                    break;
                }
                case Action.Up:
                {
                    PerformAction(Action.Up);

                    float upTime = 0;
                    while (upTime < action.y)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            isSpacePressedDuringUp = true;
                        }

                        upTime += Time.deltaTime;
                        yield return null;
                    }

                    break;
                }
                case Action.Down:
                {
                    PerformAction(Action.Down);

                    float downTime = 0;
                    while (downTime < action.y)
                    {
                        if (isSpacePressedDuringUp && !Input.GetKey(KeyCode.Space))
                        {
                            // Successful action
                            points++;
                            Debug.Log("Point earned! Total points: " + points);
                            isSpacePressedDuringUp = false; // Reset the flag
                        }

                        downTime += Time.deltaTime;
                        yield return null;
                    }

                    break;
                }
                default:
                    PerformAction((Action)action.x);
                    yield return new WaitForSeconds(action.y);
                    break;
            }
        }

        print("Finish one set");
    }

    private void PerformAction(Action action)
    {
        switch (action)
        {
            case Action.Idle:
                print("Idle");
                break;
            case Action.Speak:
                print("Speak");
                break;
            case Action.Up:
                print("Up");
                break;
            case Action.Drink:
                print("Drink");
                break;
            case Action.Down:
                print("Down");
                break;
            case Action.Angry:
                print("Angry");
                break;
        }
    }
}