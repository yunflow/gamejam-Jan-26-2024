using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private ActionSetSO[] actionSets;
    [SerializeField] private Animator professorAnimator;
    [SerializeField] private Animator studentAnimator;
    [SerializeField] private ParticleSystem talkingVFX;

    [Header("Tutorial GameObjects")] 
    [SerializeField] private GameObject backgroundIntro;
    [SerializeField] private GameObject ruleIntro;
    [SerializeField] private GameObject spaceKeyIntro;
    [SerializeField] private GameObject holdPressIntro; // drink
    [SerializeField] private GameObject releaseIntro; // 放下杯子
    [SerializeField] private GameObject successIntro;
    [SerializeField] private GameObject failIntro;
    [SerializeField] private GameObject yKeyIntro;

    // public interface
    [Header("public debug")] 
    public float points;
    public int playerReactNumber; // 0-Hahaha, 1-yeeeaa
    public bool isHahaPressed;
    public bool isYeePressed;

    // private
    private UIManager uiManager;
    private AudioManager audioManager;
    private int currentSetIndex;
    private bool inSetProcess;
    private bool isOneLevelOver;
    private bool isSpacePressedDuringUp;
    private bool isStudentAk;
    private bool isInHahaAnimation;
    private bool isInYeeAnimation;

    // tutorial bools
    private bool inFirstTutorialSet;
    private bool inSecondTutorialSet;


    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        currentSetIndex = 0;
        points = 3;
        CloseAllTutorial();
        StartCoroutine(PerformActionSet(actionSets[currentSetIndex]));
    }

    private void Update()
    {
        UpdateKey();
        DetectDeath();

        if (inSetProcess) return;
        if (isOneLevelOver)
        {
            SceneManager.LoadScene(3);
        }

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
            // Reset the Animator speed every action
            professorAnimator.speed = 1;
            studentAnimator.speed = 1;
            CloseAllTutorial();

            if ((Action)action.x == Action.Idle)
            {
                PerformAction(Action.Idle);
                uiManager.ShowReactBubble();

                if (inFirstTutorialSet)
                {
                    backgroundIntro.SetActive(true);
                }

                yield return new WaitForSeconds(action.y);
            }
            else if ((Action)action.x == Action.Speak)
            {
                PerformAction(Action.Speak);

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

                        PerformAction(Action.Angry);
                        if (inFirstTutorialSet || inSecondTutorialSet)
                        {
                            failIntro.SetActive(true);
                            yield return new WaitForSeconds(4f);
                        }
                        else
                        {
                            yield return new WaitForSeconds(2f);
                        }
                        
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
                PerformAction(Action.Up);

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

                                PerformAction(Action.Angry);
                                if (inFirstTutorialSet || inSecondTutorialSet)
                                {
                                    failIntro.SetActive(true);
                                    yield return new WaitForSeconds(4f);
                                }
                                else
                                {
                                    yield return new WaitForSeconds(2f);
                                }
                                
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

                                PerformAction(Action.Angry);
                                if (inFirstTutorialSet || inSecondTutorialSet)
                                {
                                    failIntro.SetActive(true);
                                    yield return new WaitForSeconds(4f);
                                }
                                else
                                {
                                    yield return new WaitForSeconds(2f);
                                }
                                
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
                PerformAction(Action.Drink);

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
                                //yield return new WaitForSeconds(action.y); // 继续播放Drink动画
                                isSpacePressedDuringUp = false;

                                points--;
                                print("the professor is angry! point: " + points);

                                PerformAction(Action.Angry);
                                if (inFirstTutorialSet || inSecondTutorialSet)
                                {
                                    failIntro.SetActive(true);
                                    yield return new WaitForSeconds(4f);
                                }
                                else
                                {
                                    yield return new WaitForSeconds(2f);
                                }
                                
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

                                PerformAction(Action.Angry);
                                if (inFirstTutorialSet || inSecondTutorialSet)
                                {
                                    failIntro.SetActive(true);
                                    yield return new WaitForSeconds(4f);
                                }
                                else
                                {
                                    yield return new WaitForSeconds(2f);
                                }

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
                PerformAction(Action.Down);

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
                                    releaseIntro.SetActive(false);
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
                                
                                if (inSecondTutorialSet)
                                {
                                    releaseIntro.SetActive(false);
                                    successIntro.SetActive(true);
                                }
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

    private void PerformAction(Action action)
    {
        switch (action)
        {
            case Action.Idle:
                print("animation: Professor Idle");
                talkingVFX.Stop();
                audioManager.PauseTutorSpeech();
                professorAnimator.Play("Tutor_Idle");
                break;
            case Action.Speak:
                print("animation: Professor Speak");
                talkingVFX.Play();
                audioManager.PlayTutorSpeech();
                professorAnimator.Play("Tutor_Idle_speak");
                break;
            case Action.Up:
                print("animation: Professor Up");
                talkingVFX.Stop();
                audioManager.PauseTutorSpeech();
                audioManager.PlaySound("Cough");
                professorAnimator.Play("Tutor_speak_drink");
                break;
            case Action.Drink:
                print("animation: Professor Drink");
                talkingVFX.Stop();
                audioManager.PauseTutorSpeech();
                professorAnimator.Play("Tutor_drink");
                break;
            case Action.Down:
                print("animation: Professor Down");
                talkingVFX.Stop();
                audioManager.PauseTutorSpeech();
                professorAnimator.Play("Tutor_drink_idle");
                break;
            case Action.Angry:
                print("animation: Professor Angry");
                talkingVFX.Stop();
                audioManager.PauseTutorSpeech();
                CloseAllTutorial();
                if (inFirstTutorialSet || inSecondTutorialSet)
                {
                    professorAnimator.speed = 0.25f;
                    studentAnimator.speed = 0.25f;
                }
                else
                {
                    professorAnimator.speed = 0.5f;
                    studentAnimator.speed = 0.5f;
                }
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
            audioManager.PlaySound("Hehe");
            isInHahaAnimation = true;
        }
        else if (isYeePressed && !isInYeeAnimation)
        {
            studentAnimator.Play("student_yee");
            audioManager.PlaySound("Yea");
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
            SceneManager.LoadScene(0);
        }
    }
}