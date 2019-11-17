using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyTools.MyExtensions;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class GameManager2 : MonoBehaviour
{
    public enum States
    {
        Intro1,
        Intro2,
        Equality,
        Equity,
        Liberation,
        FourthBox,
        MainMenu,
        Restart,
    }
    public static States nextSceneState = States.Intro1;
    public StateMachine<States> fsm;

    public static int unlockGameNum = 0; 

    public static GameManager2 instance { get; private set; }
    public Image blackScreen;
    public Animator textAnimator;
    public GameObject blackBarTop;
    public GameObject blackBarBottom;
    public Animator fourthyAnimator;

    public GameObject intro1Scene, introScene2, EqualityScene, EquityScene;
    public GameObject bkgBlurredParent, bkgBlurred, fenceBlurred, builderBlurred, dustBlurred, pitcherBlurred, batterBlurred, catcherBlurred;
    public GameObject fence, builder, dust, pitcher, batter, catcher, newFence1, newFence2, newFence3;

    public GameObject heartCounterButton, boxButton;

    //Equality related references
    public GameObject equalityUI, liberationUI, clickTutorialUI;
    public Animator equalityTextAnimator;
    public Image equalityBlackScreen;
    public Vector3 INIT_EQUALITY_POS = new Vector3(6.9f, -25.4f, -10f);
    public Character equality_ShortC, equality_BoxC1, equality_BoxC2;
    public GameObject successScene, equalityIntroScreen;
    public Text clickTutorialUIText;

    //Equity related references
    public GameObject equityIntroScreen, equityUI;
    public Animator equityTextAnimator;
    public Image equityBlackScreen;
    public Character equity_TallC, equity_ShortC, equity_BoxC1, equity_BoxC2;
    public Vector3 INIT_EQUITY_POS = new Vector3(6.9f, -25.4f, -10f);

    //ui buttons
    public GameObject mainMenuUI, cssLogo, game1Button, game2Button, game3Button, creditsButton, questionsButton, englishButton, spanishButton;

    public GameObject binocularView;
    public GameObject intro2Background;

    public GameObject equalityRuntimeObjsParent;
    public GameObject equityRuntimeObjsParent;
    public GameObject liberationRuntimeObjsParent;


    private Sequence instroS, intro2S, mainMenuS;
    private List<Character> equityCharacters = new List<Character>();

    // Use this for initialization
    void Awake()
    {
        instance = this;
        fsm = StateMachine<States>.Initialize(this);

        //Set screen size for Standalone
#if UNITY_STANDALONE
        Screen.SetResolution(768, 1228, false);
        Screen.fullScreen = false;
#endif

        if (nextSceneState == States.Intro1)
        {
            blackScreen.gameObject.SetActive(true);
            intro1Scene.SetActive(true);
            introScene2.SetActive(false);
            EqualityScene.SetActive(false);
            equalityUI.SetActive(false);
            EquityScene.SetActive(false);
            equityUI.SetActive(false);
            liberationUI.SetActive(false);
            clickTutorialUI.SetActive(false);
            mainMenuUI.SetActive(false);
        }

        fsm.ChangeState(nextSceneState);
        
        equityCharacters.Add(equity_TallC);
        equityCharacters.Add(equity_ShortC);
        equityCharacters.Add(equity_BoxC1);
        equityCharacters.Add(equity_BoxC2);
    }



    public void Intro1_Enter()
    {
        SoundManager.instance.musicSource.clip = SoundManager.instance.steadycheer;
        SoundManager.instance.musicSource.Play();
        blackScreen.DOFade(0f, 9f).SetEase(Ease.InQuad);//.SetLoops(1, LoopType.Restart);
        Debug.Log("Welcome entered");
        //welcomeUI.SetActive(true);
        instroS = DOTween.Sequence();
        instroS.Append(DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 12, 10f));
        instroS.Join(Camera.main.transform.DOMove(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 25f, Camera.main.transform.position.z), 10f, false));
        instroS.AppendInterval(3f);
        //instroS.Insert(4.5f, blackBarTop.transform.DOLocalMoveY(130f, 0.5f, false).SetEase(Ease.OutQuad));
        //instroS.Insert(4.5f, blackBarBottom.transform.DOLocalMoveY(-130f, 0.5f, false).SetEase(Ease.OutQuad));
        instroS.SetDelay(3f);
        //instroS.SetAutoKill(false);
        instroS.OnComplete(FinishedIntro1Sequence);
        instroS.InsertCallback(7f, () =>
        {
            fourthyAnimator.SetBool("isWatching", true);
        });
        instroS.InsertCallback(11f, () =>
        {
            fourthyAnimator.SetBool("isSerious", true);
        });
    }

    public void FinishedIntro1Sequence()
    {
        fsm.ChangeState(States.Intro2);
        //RestartTweens();
        //if (interactedIntro1 == false)
        //{
        //    instroS.Restart();
        //    readyForInteractionIntro1 = false;
        //    StartCoroutine(WaitForInteractionIntro1());
        //}
    }

    // Called by PLAY button OnClick event. Starts all tweens
    public void StartTweens()
    {
        DOTween.PlayAll();
    }

    // Called by RESTART button OnClick event. Restarts all tweens
    public void RestartTweens()
    {
        DOTween.RestartAll();
    }


    //public void Intro1_Update()
    //{
    //    if (Input.GetMouseButtonDown(0) && readyForInteractionIntro1 == true)
    //    {
    //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    //        if (hit.collider != null)
    //        {
    //            Debug.Log("Target Position: " + hit.collider.gameObject.transform.name);
    //            if(hit.collider.gameObject.transform.name == "Binoculars")
    //            {
    //                ChangeState(States.Intro2);
    //                interactedIntro1 = true; 
    //            }
    //            else if(hit.collider.gameObject.transform.name == "Background")
    //            {
    //                Sequence zoomToFan = DOTween.Sequence();
    //                zoomToFan.Append(DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 2, 1f));
    //                zoomToFan.Join(Camera.main.transform.DOMove(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, Camera.main.transform.position.z), 1f, false));
    //                zoomToFan.AppendInterval(1.5f);
    //                zoomToFan.OnComplete(() =>
    //                {
    //                    Debug.Log("finsiheddd12");
    //                    instroS.Restart();
    //                    interactedIntro1 = false;
    //                });
    //                interactedIntro1 = true;
    //            }
    //        }
    //    }
    //}

    public void Intro1_Exit()
    {
        Debug.Log("Intro1 exited");
        intro1Scene.SetActive(false);
        blackBarBottom.transform.localPosition = new Vector3(blackBarBottom.transform.localPosition.x, -200f, blackBarBottom.transform.localPosition.z);
        blackBarTop.transform.localPosition = new Vector3(blackBarTop.transform.localPosition.x, 200f, blackBarTop.transform.localPosition.z);
        //welcomeUI.SetActive(false);
    }



    public void Intro2_Enter()
    {
        //Debug.Log("Welcome to Intro 2");
        Camera.main.transform.position = new Vector3(6f, -25f, -10f);
        Camera.main.orthographicSize = 30f;
        introScene2.SetActive(true);
        clickTutorialUI.SetActive(true);
        clickTutorialUI.transform.DOLocalMoveX(clickTutorialUI.transform.localPosition.x + 300f, 0.5f).From().SetEase(Ease.OutQuad);

        binocularView.transform.DOLocalMoveY(36f, 5f, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private bool intro2Clicked = false; 
    public void Intro2_Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(binocularView.transform.position.y);
            clickTutorialUI.SetActive(false);
            if (newFence1.activeInHierarchy == true)
            {
                blackScreen.DOFade(0f, 1f).SetEase(Ease.InOutQuad).SetLoops(1, LoopType.Restart).OnComplete(() =>
                {
                    fsm.ChangeState(States.Equality);
                });
            }
            else if(intro2Clicked == false)
            {
                intro2Clicked = true; 
                DeBlur();
                if (binocularView.transform.localPosition.y < 5f)
                {
                    Sequence zoomToPlay = DOTween.Sequence();
                    binocularView.transform.DOGoto(0, false);
                    zoomToPlay.Append(intro2Background.transform.DOScale(new Vector3(3f, 3f, 3f), 1.5f));
                    zoomToPlay.Join(intro2Background.transform.DOLocalMoveY(-30f - 78.6f, 1.5f));
                    zoomToPlay.AppendInterval(2f);

                    zoomToPlay.AppendCallback(() =>
                    {
                        StartCoroutine(buildFence());
                    });

                }
                else
                {
                    //zoom in on the action and go back to yo yo.
                    Sequence zoomToPlay = DOTween.Sequence();
                    binocularView.transform.DOGoto(0, false);
                    zoomToPlay.Append(intro2Background.transform.DOScale(new Vector3(3f, 3f, 3f), 1.5f));
                    zoomToPlay.Join(intro2Background.transform.DOLocalMoveY(-30f + 86.3f, 1.5f));
                    zoomToPlay.AppendInterval(3f);
                    zoomToPlay.InsertCallback(1.5f, () =>
                    {
                        pitcher.GetComponent<Animator>().SetTrigger("inAction");
                    });
                    zoomToPlay.InsertCallback(1.75f, () =>
                    {
                        batter.GetComponent<Animator>().SetTrigger("inAction");
                    });

                    zoomToPlay.AppendCallback(() =>
                    {
                        intro2Background.transform.SetScaleXYZ(1.3424f, 1.3424f, 1.3424f);
                        intro2Background.transform.SetLocalY(-30f);
                        Blur();
                        binocularView.transform.DOLocalMoveY(36f, 5f, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                        intro2Clicked = false;
                        //binocularView.transform.DOLocalMoveY(20f, 3f, false).SetLoops(-1, LoopType.Yoyo);
                    });

                }
            }

        }
    }

    public IEnumerator buildFence()
    {
        dust.GetComponent<Animator>().SetTrigger("triggerBigPoof");
        yield return new WaitForSeconds(1.25f);
        newFence1.SetActive(true);
        yield return new WaitForSeconds(.25f);
        builder.transform.localPosition = new Vector3(builder.transform.localPosition.x + 14f, builder.transform.localPosition.y, builder.transform.localPosition.z);
        yield return new WaitForSeconds(3f);
        dust.GetComponent<Animator>().SetTrigger("triggerBigPoof");
        yield return new WaitForSeconds(1.25f);
        newFence2.SetActive(true);
        yield return new WaitForSeconds(.25f);
        builder.transform.localPosition = new Vector3(builder.transform.localPosition.x + 14f, builder.transform.localPosition.y, builder.transform.localPosition.z);
        yield return new WaitForSeconds(2f);
        clickTutorialUIText.text = "Tap anywhere to \ngo to the fence";
        clickTutorialUI.SetActive(true);
    }

    public void DeBlur()
    {
        intro2S = DOTween.Sequence();
        intro2S.Append(bkgBlurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fenceBlurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(builderBlurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(dustBlurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(pitcherBlurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(batterBlurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(catcherBlurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));

        intro2S.Join(fence.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(builder.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(dust.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(pitcher.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(batter.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(catcher.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
    }

    public void Blur()
    {
        bkgBlurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        fenceBlurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        builderBlurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        dustBlurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        pitcherBlurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        batterBlurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        catcherBlurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);

        fence.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        builder.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        dust.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        pitcher.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        batter.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        catcher.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
    }

    public void Intro2_Exit()
    {
        Debug.Log("Intro2 exited");


        introScene2.SetActive(false);
        clickTutorialUI.SetActive(false);


    }


    private bool boxTutTextShow;
    public void Equality_Enter()
    {
        Debug.Log("Welcome to Equality");

        SoundManager.instance.musicSource.clip = SoundManager.instance.ballparkBkg;
        SoundManager.instance.musicSource.Play();
        //reset heart counter, character Heart_StartPoint

        boxTutTextShow = false;
        successScene.SetActive(false);
        equalityTextAnimator.gameObject.SetActive(false);
        EqualityScene.SetActive(true);
        equalityUI.SetActive(true);
        equalityBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeFromBlack = equalityBlackScreen.DOFade(1f, 3f).SetEase(Ease.InOutQuad).From();
        fadeFromBlack.OnUpdate(()=> {
            if (fadeFromBlack.ElapsedPercentage() > 0.25f)
            {
                if (!equalityIntroScreen.activeInHierarchy)
                {
                    equalityIntroScreen.SetActive(true);
                }
                
            }
        });
        Camera.main.orthographicSize = 15;
        Camera.main.transform.position = INIT_EQUALITY_POS;
        equality_ShortC.transform.localPosition = equality_ShortC.offscreenPos;

        clickTutorialUIText.text = "Tap each heart\n to collect it";
        clickTutorialUI.transform.DOLocalMoveX(clickTutorialUI.transform.localPosition.x + 350f, 0.5f).From().SetEase(Ease.OutQuad).SetDelay(5f).OnPlay(() =>
        {
            clickTutorialUI.SetActive(true);
        });
    }

    public IEnumerator BoxPlacementTutorial()
    {
        yield return new WaitForSeconds(0.25f);
        clickTutorialUIText.text = "Tap anywhere to\nto drop the box";
        clickTutorialUI.transform.position = new Vector3(Screen.width * 0.7f, Screen.height * 0.12f, 0);
        clickTutorialUI.SetActive(true);
        clickTutorialUI.transform.DOLocalMoveY(clickTutorialUI.transform.localPosition.y - 300f, 0.5f).From().SetEase(Ease.OutQuad);
    }

    public void Equality_Update()
    {
        if (HeartCounter.Instance.HeartsNumber == 3 && boxTutTextShow == false)
        {
            boxTutTextShow = true;
            Debug.Log("3 hearts are there");
            clickTutorialUIText.text = "Tap the icon\nto make a box";
            clickTutorialUI.transform.position = new Vector3(Screen.width * 0.7f, Screen.height * 0.12f, 0);
            clickTutorialUI.SetActive(true);
            clickTutorialUI.transform.DOLocalMoveY(clickTutorialUI.transform.localPosition.y - 300f, 0.5f).From().SetEase(Ease.OutQuad);
        }
    }

    public void BackButtonHandler()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);

        nextSceneState = States.MainMenu;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Equality_Success()
    {
        Debug.Log("Equality Success calllled");
        equality_BoxC1.GetComponent<Character>().meter.SetActive(false);
        equality_BoxC2.GetComponent<Character>().meter.SetActive(false);
        equality_ShortC.GetComponent<Character>().meter.SetActive(false);
        DOTween.KillAll();
        Sequence successS = DOTween.Sequence();
        successS.Append(DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 25, 5f));
        successS.Join(Camera.main.transform.DOMove(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 12f, Camera.main.transform.position.z), 5f, false));
        successS.InsertCallback(2f, () =>
        {
            SoundManager.instance.musicSource.clip = SoundManager.instance.bigcheer;
            SoundManager.instance.musicSource.Play();           
        });
        successS.InsertCallback(5f, () =>
        {
            equalityTextAnimator.gameObject.SetActive(true);
            equalityTextAnimator.Play("Expand");
        });
        //successS.Append(equalityBlackScreen.DOFade(1f, 2f).SetEase(Ease.InOutQuad).SetDelay(3f));
        successS.InsertCallback(9f, () =>
        {
            unlockGameNum++;
            TurnOnResultsScreen(100f);
            
            //fsm.ChangeState(States.MainMenu);
        });
    }

    public void TurnOnResultsScreen(float percent)
    {
        SuccessScreen mySuccessScreen = successScene.GetComponent<SuccessScreen>();
        mySuccessScreen.percentText.text = percent.ToString() + "%";
        if(percent < 1f)
        {
            mySuccessScreen.nextButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            mySuccessScreen.nextButton.GetComponent<Button>().interactable = true;

        }
        mySuccessScreen.AnimateStars(percent);
        if (fsm.CurrentStateMap.state.ToString() == States.Equality.ToString())
        {
            successScene.transform.SetParent(equalityUI.transform);
            successScene.transform.SetAsLastSibling();
            equalityBlackScreen.transform.SetAsLastSibling();
            mySuccessScreen.infoText.text = "Equality Achieved";
        }
        else if (fsm.CurrentStateMap.state.ToString() == States.Equity.ToString())
        {
            successScene.transform.SetParent(equityUI.transform);
            successScene.transform.SetAsLastSibling();
            equityBlackScreen.transform.SetAsLastSibling();
            mySuccessScreen.infoText.text = "Equity Achieved"; 
        }
            
        successScene.SetActive(true);
    }

    public IEnumerator Failed()
    {
        clickTutorialUI.SetActive(false);
        yield return new WaitForSeconds(2f);
        if(fsm.CurrentStateMap.state.ToString() == States.Equality.ToString())
        {
            TurnOnResultsScreen(0f);
        }else if(fsm.CurrentStateMap.state.ToString() == States.Equity.ToString())
        {
            Debug.Log("should show Equity fail screen");
            float totalPassed = 4f;
            for(int i =0; i < equityCharacters.Count; i++)
            {
                if (equityCharacters[i].fsm.CurrentStateMap.state.ToString() != Character.States.Watching.ToString())
                {
                    totalPassed = totalPassed - 1f;
                }
            }

            TurnOnResultsScreen((totalPassed / 4f) * 100f);
        }
        
    }

    public IEnumerator Equality_Exit()
    {
        Debug.Log("Exiting Equality");

        if (clickTutorialUI.activeInHierarchy)
        {
            clickTutorialUI.SetActive(false);
        }
        equalityBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeToBlackTween = equalityBlackScreen.DOFade(1f, 2f).SetEase(Ease.OutQuad);
        yield return fadeToBlackTween.WaitForCompletion();
        //delete all instantiated boxes and hearts
        foreach (Transform child in equalityRuntimeObjsParent.transform)
        {
            Destroy(child.gameObject);
        }
        EqualityScene.SetActive(false);
        equalityUI.SetActive(false);
        DOTween.KillAll();
    }

    private bool callSuccessOnce = false;
    public void Equity_Enter()
    {
        Debug.Log("Welcome to Equity");
        callSuccessOnce = false;

        SoundManager.instance.musicSource.clip = SoundManager.instance.ballparkBkg;
        SoundManager.instance.musicSource.Play();
        //reset heart counter, character Heart_StartPoint

        successScene.SetActive(false);
        equityTextAnimator.gameObject.SetActive(false);
        EquityScene.SetActive(true);
        equityUI.SetActive(true);
        heartCounterButton.transform.SetParent(equityUI.transform);
        heartCounterButton.GetComponent<HeartCounter>().HeartsNumber = 0; 
        boxButton.transform.SetParent(equityUI.transform);
        equityBlackScreen.transform.SetAsLastSibling();
        equityBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeFromBlack = equityBlackScreen.DOFade(1f, 3f).SetEase(Ease.InOutQuad).From();
        fadeFromBlack.OnUpdate(() => {
            if (fadeFromBlack.ElapsedPercentage() > 0.25f)
            {
                if (!equityIntroScreen.activeInHierarchy)
                {
                    equityIntroScreen.SetActive(true);
                }

            }
        });
        Camera.main.orthographicSize = 23;
        Camera.main.transform.position = INIT_EQUITY_POS;
    }

    private int watchingEquityCharacters;
    public void Equity_Update()
    {
        //checking for succcess
        watchingEquityCharacters = 0; 
        for (int i = 0; i < equityCharacters.Count; i++)
        {
            if (equityCharacters[i].fsm.CurrentStateMap.state != null)
            {
                //Debug.Log(equityCharacters[i].transform.name + " : " + equityCharacters[i].fsm.CurrentStateMap.state);
                if (equityCharacters[i].fsm.CurrentStateMap.state.ToString() == Character.States.Watching.ToString())
                {
                    watchingEquityCharacters++;
                }
            }
        }
        
       // Debug.Log("how many watching: " +  watchingEquityCharacters.ToString());
        if(watchingEquityCharacters == equityCharacters.Count)
        {
            if (callSuccessOnce == false)
            {
                callSuccessOnce = true; 
                Equity_Success();
            }
        }

    }

    public void Equity_Success()
    {
        Debug.Log("Equity Success calllled");
        for (int i = 0; i < equityCharacters.Count; i++)
        {
            equityCharacters[i].meter.SetActive(false);
        }
        DOTween.KillAll();
        Sequence successS = DOTween.Sequence();
        successS.Append(DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 30, 5f));
        successS.Join(Camera.main.transform.DOMove(new Vector3(Camera.main.transform.position.x, -8.8f, Camera.main.transform.position.z), 5f, false));
        successS.InsertCallback(2f, () =>
        {
            SoundManager.instance.musicSource.clip = SoundManager.instance.bigcheer;
            SoundManager.instance.musicSource.Play();
        });
        successS.InsertCallback(5f, () =>
        {
            equityTextAnimator.gameObject.SetActive(true);
            equityTextAnimator.Play("Expand");
        });
        successS.InsertCallback(9f, () =>
        {
            unlockGameNum++;
            TurnOnResultsScreen(100f);
        });
    }

    public IEnumerator Equity_Exit()
    {
        Debug.Log("Exiting Equity");

        equityBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeToBlackTween = equalityBlackScreen.DOFade(1f, 2f).SetEase(Ease.OutQuad);
        yield return fadeToBlackTween.WaitForCompletion();
        //delete all instantiated boxes and hearts
        foreach (Transform child in equityRuntimeObjsParent.transform)
        {
            Destroy(child.gameObject);
        }
        EquityScene.SetActive(false);
        equityUI.SetActive(false);
        DOTween.KillAll();
    }


    public void MainMenu_Enter()
    {
        Debug.Log("Welcome to MainMenu");
        SoundManager.instance.musicSource.clip = SoundManager.instance.menuBkg;
        SoundManager.instance.musicSource.Play();
        mainMenuUI.SetActive(true);
        cssLogo.SetActive(false);
        if (unlockGameNum == 0)
        {
            game2Button.GetComponent<Button>().interactable = false;
            game3Button.GetComponent<Button>().interactable = false;
            questionsButton.GetComponent<Button>().interactable = false;
        }
        else if (unlockGameNum == 1)
        {
            game3Button.GetComponent<Button>().interactable = false;
            questionsButton.GetComponent<Button>().interactable = false;
        }
        else if (unlockGameNum == 2)
        {
            questionsButton.GetComponent<Button>().interactable = false;
        }
        //game1Button.SetActive(false);
        //game2Button.SetActive(false);
        //game3Button.SetActive(false);
        //creditsButton.SetActive(false);
        //questionsButton.SetActive(false);
        //englishButton.SetActive(false);
        //spanishButton.SetActive(false);

        blackScreen.DOFade(0f, 1f).SetEase(Ease.InOutQuad).SetLoops(1, LoopType.Restart);

        mainMenuS = DOTween.Sequence();
        mainMenuS.Append(game1Button.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        mainMenuS.Append(game2Button.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        mainMenuS.Append(game3Button.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        mainMenuS.Join(englishButton.transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutQuad));
        mainMenuS.Join(spanishButton.transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutQuad));
        mainMenuS.Join(creditsButton.transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutQuad));
        mainMenuS.Join(questionsButton.transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutQuad));
        mainMenuS.AppendCallback(() =>
        {
            cssLogo.SetActive(true);
            mainMenuS.Append(cssLogo.transform.DOLocalMoveY(cssLogo.transform.localPosition.y - 150f, 0.5f).From().SetEase(Ease.OutBounce));
        });
    }

    public void Game1ButtonPressed()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);

        nextSceneState = States.Equality;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Game2ButtonPressed()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);

        nextSceneState = States.Equity;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Game3ButtonPressed()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);

        nextSceneState = States.Equity;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuestionsButtonPressed()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.error);
        //fsm.ChangeState(States.Equality);
    }

    public void CreditsButtonPressed()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);
        //fsm.ChangeState(States.Equality);
    }

    public void MainMenu_Update()
    {

    }

    public void MainMenu_Exit()
    {
        Debug.Log("Exiting MainMenu");
        mainMenuUI.SetActive(false);
    }



    public void Restart_Enter()
    {
        nextSceneState = fsm.LastState;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Application.LoadLevel(Application.loadedLevel);
        //fsm.ChangeState(fsm.LastState);
    }

    public void Restart_Update()
    {

    }

    public void Restart_Exit()
    {

    }

    public Transform GetCurrentRuntimeObjsParent()
    {
        if (fsm.CurrentStateMap.state.ToString() == States.Equality.ToString())
        {
            return equalityRuntimeObjsParent.transform;
        }
        else if (fsm.CurrentStateMap.state.ToString() == States.Equity.ToString())
        {
            return equityRuntimeObjsParent.transform;
        }
        else if (fsm.CurrentStateMap.state.ToString() == States.Liberation.ToString())
        {
            return equityRuntimeObjsParent.transform;
        }
        else
        {
            Debug.Log("something is wrong");
            return equityRuntimeObjsParent.transform;
        }
    }

}
