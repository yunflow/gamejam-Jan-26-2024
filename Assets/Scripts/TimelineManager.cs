using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private ActionSetSO[] actionSets;
    [SerializeField] private Animator professorAnimator;
    [SerializeField] private Animator studentAnimator;

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
    private bool isStudentAk;


    private void Start()
    {
        currentSetIndex = 0;
        points = 3;
        StartCoroutine(PerformActionSet(actionSets[currentSetIndex]));
    }

    private void Update()
    {
        UpdateKey();

        if (inSetProcess || isOneLevelOver) return;

        currentSetIndex++;
        if (currentSetIndex > actionSets.Length - 1)
        {
            print("**** All Sets finish ****");
            PerformTutorAction(Action.Idle);
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
            studentAnimator.speed = 1;

            if ((Action)action.x == Action.Speak)
            {
                PerformTutorAction(Action.Speak);

                float speakTime = 0;
                while (speakTime < action.y)
                {
                    if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Y))
                    {
                        points--;
                        print("the professor is angry! point: " + points);

                        PerformTutorAction(Action.Angry);
                        yield return new WaitForSeconds(2f);
                        isStudentAk = false;

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
                professorAnimator.speed = 1.0f / action.y;
                PerformTutorAction(Action.Up);

                float upTime = 0;
                while (upTime < action.y)
                {
                    switch (playerReactNumber)
                    {
                        case 0:
                        {
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                isSpacePressedDuringUp = true;
                            }
                            else if (Input.GetKeyDown(KeyCode.Y))
                            {
                                points--;
                                print("the professor is angry! point: " + points);

                                PerformTutorAction(Action.Angry);
                                yield return new WaitForSeconds(2f);
                                isStudentAk = false;

                                print("**** Finish One Set But Fail ****");
                                inSetProcess = false;
                                yield break;
                            }

                            upTime += Time.deltaTime;
                            yield return null;
                        }
                            break;
                        case 1:
                        {
                            if (Input.GetKeyDown(KeyCode.Y))
                            {
                                isSpacePressedDuringUp = true;
                            }
                            else if (Input.GetKeyDown(KeyCode.Space))
                            {
                                points--;
                                print("the professor is angry! point: " + points);

                                PerformTutorAction(Action.Angry);
                                yield return new WaitForSeconds(2f);
                                isStudentAk = false;

                                print("**** Finish One Set But Fail ****");
                                inSetProcess = false;
                                yield break;
                            }

                            upTime += Time.deltaTime;
                            yield return null;
                        }
                            break;
                    }
                }
            }
            else if ((Action)action.x == Action.Drink)
            {
                PerformTutorAction(Action.Drink);

                float drinkTime = 0;
                while (drinkTime < action.y)
                {
                    switch (playerReactNumber)
                    {
                        case 0:
                        {
                            if (!Input.GetKey(KeyCode.Space))
                            {
                                yield return new WaitForSeconds(action.y); // 继续播放Drink动画
                                isSpacePressedDuringUp = false;

                                points--;
                                print("the professor is angry! point: " + points);

                                PerformTutorAction(Action.Angry);
                                yield return new WaitForSeconds(2f);
                                isStudentAk = false;

                                print("**** Finish One Set But Fail ****");
                                inSetProcess = false;
                                yield break;
                            }

                            drinkTime += Time.deltaTime;
                            yield return null;
                        }
                            break;
                        case 1:
                        {
                            if (!Input.GetKey(KeyCode.Y))
                            {
                                yield return new WaitForSeconds(action.y); // 继续播放Drink动画
                                isSpacePressedDuringUp = false;

                                points--;
                                print("the professor is angry! point: " + points);

                                PerformTutorAction(Action.Angry);
                                yield return new WaitForSeconds(2f);
                                isStudentAk = false;

                                print("**** Finish One Set But Fail ****");
                                inSetProcess = false;
                                yield break;
                            }

                            drinkTime += Time.deltaTime;
                            yield return null;
                        }
                            break;
                    }
                }
            }
            else if ((Action)action.x == Action.Down)
            {
                professorAnimator.speed = 1.0f / action.y;
                PerformTutorAction(Action.Down);

                float downTime = 0;
                while (downTime < action.y)
                {
                    switch (playerReactNumber)
                    {
                        case 0:
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
                            break;
                        case 1:
                        {
                            if (isSpacePressedDuringUp && !Input.GetKey(KeyCode.Y))
                            {
                                // Successful action
                                Debug.Log("**** Professor Not Angry ****");
                                isSpacePressedDuringUp = false; // Reset the flag
                            }

                            downTime += Time.deltaTime;
                            yield return null;
                        }
                            break;
                    }
                }
            }
            else
            {
                PerformTutorAction((Action)action.x);
                yield return new WaitForSeconds(action.y);
            }
        }

        print("**** Finish One Set ****");
        inSetProcess = false;
    }

    private void PerformTutorAction(Action action)
    {
        switch (action)
        {
            case Action.Idle:
                print("animation: Professor Idle");
                professorAnimator.Play("Tutor_Idle");
                break;
            case Action.Speak:
                print("animation: Professor Speak");
                professorAnimator.Play("Tutor_Idle_speak");
                break;
            case Action.Up:
                print("animation: Professor Up");
                professorAnimator.Play("Tutor_speak_drink");
                break;
            case Action.Drink:
                print("animation: Professor Drink");
                professorAnimator.Play("Tutor_drink");
                break;
            case Action.Down:
                print("animation: Professor Down");
                professorAnimator.Play("Tutor_drink_idle");
                break;
            case Action.Angry:
                print("animation: Professor Angry");
                professorAnimator.speed = 0.5f;
                studentAnimator.speed = 0.5f;
                isStudentAk = true;
                professorAnimator.Play("Tutor_angry");
                studentAnimator.Play("student_ak");
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

    private void UpdateKey()
    {
        isHahaPressed = Input.GetKey(KeyCode.Space);
        isYeePressed = Input.GetKey(KeyCode.Y);

        if (isStudentAk) return;

        if (isHahaPressed)
        {
            studentAnimator.Play("student_haha");
        }
        else if (isYeePressed)
        {
            studentAnimator.Play("student_yee");
        }
        else
        {
            studentAnimator.Play("student_Idle");
        }
    }
}