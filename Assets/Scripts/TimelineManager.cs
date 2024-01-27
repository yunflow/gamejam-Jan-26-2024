using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private ActionSetSO[] actionSets;
    [SerializeField] private Animator professorAnimator;

    // public interface
    public float points;
    public int playerReactNumber; // 0-Hahaha, 1-yeeeaa
    public bool isHahaPressed;
    public bool isYeePressed;

    // private
    private int currentSetIndex;

    private bool inSetProcess;
    private bool isOneLevelOver;
    private bool isSpacePressedDuringUp;


    private void Start()
    {
        currentSetIndex = 0;
        points = 3;
        StartCoroutine(PerformActionSet(actionSets[currentSetIndex]));
    }

    private void Update()
    {
        if (inSetProcess || isOneLevelOver) return;

        currentSetIndex++;
        if (currentSetIndex > actionSets.Length - 1)
        {
            print("**** All Sets finish ****");
            PerformAction(Action.Idle);
            isOneLevelOver = true;
            return;
        }

        StartCoroutine(PerformActionSet(actionSets[currentSetIndex]));
    }

    private IEnumerator PerformActionSet(ActionSetSO actionSet)
    {
        print("**** Start A New Set: " + (currentSetIndex + 1) + "****");
        inSetProcess = true;
        isSpacePressedDuringUp = false;

        SetRandomReactNumber();

        Vector2[] actions = actionSet.actions;

        foreach (Vector2 action in actions)
        {
            professorAnimator.speed = 1; // Reset the Animator speed every action

            if ((Action)action.x == Action.Speak)
            {
                PerformAction(Action.Speak);

                float speakTime = 0;
                while (speakTime < action.y)
                {
                    if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Y))
                    {
                        points--;
                        print("the professor is angry! point: " + points);

                        PerformAction(Action.Angry);
                        yield return new WaitForSeconds(3f);

                        print("**** Finish One Set But Fail ****");
                        inSetProcess = false;
                        yield break;
                    }

                    speakTime += Time.deltaTime;
                    yield return null;
                }
            }
            else if ((Action)action.x == Action.Up)
            {
                professorAnimator.speed = 0.67f / action.y;
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

                        print("**** Finish One Set But Fail ****");
                        inSetProcess = false;
                        yield break;
                    }

                    drinkTime += Time.deltaTime;
                    yield return null;
                }
            }
            else if ((Action)action.x == Action.Down)
            {
                professorAnimator.speed = 0.625f / action.y;
                PerformAction(Action.Down);

                float downTime = 0;
                while (downTime < action.y)
                {
                    if (isSpacePressedDuringUp && !Input.GetKey(KeyCode.Space))
                    {
                        // Successful action
                        Debug.Log("**** Professor Not Angry ****");
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

        print("**** Finish One Set ****");
        inSetProcess = false;
    }

    private void PerformAction(Action action)
    {
        switch (action)
        {
            case Action.Idle:
                print("animation: Professor Idle");
                professorAnimator.Play("Tutor Idle");
                break;
            case Action.Speak:
                print("animation: Professor Speak");
                professorAnimator.Play("Tutor Speak");
                break;
            case Action.Up:
                print("animation: Professor Up");
                professorAnimator.Play("Tutor Up");
                break;
            case Action.Drink:
                print("animation: Professor Drink");
                professorAnimator.Play("Tutor Drink");
                break;
            case Action.Down:
                print("animation: Professor Down");
                professorAnimator.Play("Tutor Down");
                break;
            case Action.Angry:
                print("animation: Professor Angry");
                professorAnimator.Play("Tutor Angry");
                break;
        }
    }

    private void SetRandomReactNumber()
    {
        // Set Random React number [0, 1]
        playerReactNumber = Random.Range(0, 2);

        if (playerReactNumber == 0)
        {
            print("Should Hahahah");
        }
        else if (playerReactNumber == 1)
        {
            print("Should Yeeee");
        }
    }
}