using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private ActionSetSO[] actionSets;
    [SerializeField] private Animator professorAnimator;
    [SerializeField] private Animator studentAnimator;
    [SerializeField] private ParticleSystem talkingVFX;

    [Header("Tutorial GameObjects")] [SerializeField]
    private GameObject backgroundIntro;

    [SerializeField] private GameObject ruleIntro;
    [SerializeField] private GameObject spaceKeyIntro;
    [SerializeField] private GameObject holdPressIntro; // drink
    [SerializeField] private GameObject releaseIntro; // 放下杯子

    [SerializeField] private GameObject successIntro;
    [SerializeField] private GameObject failIntro;

    [SerializeField] private GameObject yKeyIntro;

    // public interface
    [Header("public debug")] public float points;
    public int playerReactNumber; // 0-Hahaha, 1-yeeeaa
    public bool isHahaPressed;
    public bool isYeePressed;

    // private
    private int currentSetIndex;

    private bool inSetProcess;
    private bool isOneLevelOver;
    private bool isSpacePressedDuringUp;
    private bool isStudentAk;
    private bool isInHahaAnimation;
    private bool isInYeeAnimation;

    // tutorial bool
    private bool inFirstTutorialSet;
    private bool inSecondTutorialSet;


    private void Start()
    {
        CloseAllTutorial();
        currentSetIndex = 0;
        points = 3;
        StartCoroutine(PerformActionSet(actionSets[currentSetIndex]));
    }

    private void Update()
    {
        UpdateKey();
        DetectDeath();

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

        // Check if in tutorial sets
        switch (currentSetIndex)
        {
            case 0:
                playerReactNumber = 0;
                inFirstTutorialSet = true;
                inSecondTutorialSet = false;
                break;
            case 1:
                playerReactNumber = 1;
                inFirstTutorialSet = false;
                inSecondTutorialSet = true;
                break;
            default:
                inFirstTutorialSet = false;
                inSecondTutorialSet = false;
                break;
        }

        Vector2[] actions = actionSet.actions;

        foreach (Vector2 action in actions)
        {
            // Reset the Animator speed every action
            professorAnimator.speed = 1;
            studentAnimator.speed = 1;
            CloseAllTutorial();

            if ((Action)action.x == Action.Idle)
            {
                talkingVFX.Stop();
                PerformTutorAction((Action)action.x);

                if (inFirstTutorialSet)
                {
                    backgroundIntro.SetActive(true);
                }

                yield return new WaitForSeconds(action.y);
            }
            else if ((Action)action.x == Action.Speak)
            {
                PerformTutorAction(Action.Speak);
                talkingVFX.Play();

                if (inFirstTutorialSet)
                {
                    ruleIntro.SetActive(true);
                }

                float speakTime = 0;
                while (speakTime < action.y)
                {
                    if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Y))
                    {
                        points--;
                        print("the professor is angry! point: " + points);

                        PerformTutorAction(Action.Angry);
                        if (inFirstTutorialSet || inSecondTutorialSet)
                        {
                            failIntro.SetActive(true);
                        }

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
                talkingVFX.Stop();

                if (inFirstTutorialSet)
                {
                    spaceKeyIntro.SetActive(true);
                }
                else if (inSecondTutorialSet)
                {
                    yKeyIntro.SetActive(true);
                }

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
                                if (inFirstTutorialSet || inSecondTutorialSet)
                                {
                                    failIntro.SetActive(true);
                                }

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
                                if (inFirstTutorialSet || inSecondTutorialSet)
                                {
                                    failIntro.SetActive(true);
                                }

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
                talkingVFX.Stop();

                if (inFirstTutorialSet || inSecondTutorialSet)
                {
                    holdPressIntro.SetActive(true);
                }

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
                                if (inFirstTutorialSet || inSecondTutorialSet)
                                {
                                    failIntro.SetActive(true);
                                }

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
                                if (inFirstTutorialSet || inSecondTutorialSet)
                                {
                                    failIntro.SetActive(true);
                                }

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
                talkingVFX.Stop();

                if (inFirstTutorialSet)
                {
                    releaseIntro.SetActive(true);
                }

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

                                if (inFirstTutorialSet)
                                {
                                    successIntro.SetActive(true);
                                }
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
                talkingVFX.Stop();
                studentAnimator.Play("student_ak");
                break;
        }
    }

    private void SetRandomReactNumber()
    {
        // Set Random React number [0, 1]
        playerReactNumber = Random.Range(0, 2);

        switch (playerReactNumber)
        {
            case 0:
                print("Should Hahahah");
                break;
            case 1:
                print("Should Yeeee");
                break;
        }
    }

    private void UpdateKey()
    {
        isHahaPressed = Input.GetKey(KeyCode.Space);
        isYeePressed = Input.GetKey(KeyCode.Y);

        if (isStudentAk) return;

        if (isHahaPressed && !isInHahaAnimation)
        {
            studentAnimator.Play("student_haha");
            isInHahaAnimation = true;
        }
        else if (isYeePressed && !isInYeeAnimation)
        {
            studentAnimator.Play("student_yee");
            isInYeeAnimation = true;
        }
        else if (!isHahaPressed && !isYeePressed)
        {
            studentAnimator.Play("student_Idle");
            isInHahaAnimation = false;
            isInYeeAnimation = false;
        }
    }

    private void CloseAllTutorial()
    {
        backgroundIntro.SetActive(false);
        ruleIntro.SetActive(false);
        spaceKeyIntro.SetActive(false);
        holdPressIntro.SetActive(false);
        releaseIntro.SetActive(false);
        successIntro.SetActive(false);
        failIntro.SetActive(false);
        yKeyIntro.SetActive(false);
    }

    private void DetectDeath()
    {
        if (points <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}