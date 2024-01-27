using System.Collections;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private ActionSetSO[] actionSets;
    [SerializeField] private Animator professorAnimator;

    public int points;
    public bool isSpacePressedDuringUp;


    private void Start()
    {
        points = 3;
        StartCoroutine(PerformActionSet(actionSets[0]));
    }

    private IEnumerator PerformActionSet(ActionSetSO actionSet)
    {
        print("!!!! Start A New Set !!!!");

        Vector2[] actions = actionSet.actions;

        foreach (Vector2 action in actions)
        {
            if ((Action)action.x == Action.Speak)
            {
                PerformAction(Action.Speak);

                float speakTime = 0;
                while (speakTime < action.y)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        points--;
                        print("the professor is angry! point: " + points);

                        PerformAction(Action.Angry);
                        yield return new WaitForSeconds(3f);

                        print("!!!! Finish One Set But Fail !!!!");
                        yield break;
                    }

                    speakTime += Time.deltaTime;
                    yield return null;
                }
            }
            else if ((Action)action.x == Action.Up)
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
            }
            else if ((Action)action.x == Action.Drink)
            {
                PerformAction(Action.Drink);

                float drinkTime = 0;
                while (drinkTime < action.y)
                {
                    if (!Input.GetKey(KeyCode.Space))
                    {
                        yield return new WaitForSeconds(action.y); // 继续播放Drink动画
                        isSpacePressedDuringUp = false;

                        points--;
                        print("the professor is angry! point: " + points);

                        PerformAction(Action.Angry);
                        yield return new WaitForSeconds(3f);

                        print("!!!! Finish One Set But Fail !!!!");
                        yield break;
                    }

                    drinkTime += Time.deltaTime;
                    yield return null;
                }
            }
            else if ((Action)action.x == Action.Down)
            {
                PerformAction(Action.Down);

                float downTime = 0;
                while (downTime < action.y)
                {
                    if (isSpacePressedDuringUp && !Input.GetKey(KeyCode.Space))
                    {
                        // Successful action
                        Debug.Log("Success laugh");
                        isSpacePressedDuringUp = false; // Reset the flag
                    }

                    downTime += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                PerformAction((Action)action.x);
                yield return new WaitForSeconds(action.y);
            }
        }

        print("!!!! Finish One Set!!!!");
    }

    private void PerformAction(Action action)
    {
        switch (action)
        {
            case Action.Idle:
                print("animation: Professor Idle");
                break;
            case Action.Speak:
                print("animation: Professor Speak");
                break;
            case Action.Up:
                print("animation: Professor Up");
                break;
            case Action.Drink:
                print("animation: Professor Drink");
                break;
            case Action.Down:
                print("animation: Professor Down");
                break;
            case Action.Angry:
                print("animation: Professor Angry");
                break;
        }
    }
}