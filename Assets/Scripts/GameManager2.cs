using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyTools.MyExtensions;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Lean.Localization;


[System.Serializable]
public class FencePoint
{
    [SerializeField]
    public Transform transformPoint;
    [SerializeField]
    public Character occupant;
}

public class GameManager2 : MonoBehaviour
{
    public enum States
    {
        LanguageMenu,
        Intro1,
        Intro2,
        Equality,
        Equity,
        Liberation,
        EndScene,
        Credits,
        Questions,
        FourthBox,
        MainMenu,
        Restart,
    }
    public static States nextSceneState = States.LanguageMenu;
    public StateMachine<States> fsm;

    public static int unlockGameNum = 0;

    public static GameManager2 instance { get; private set; }
    public Camera UICamera;
    public Image blackScreen;
    public Animator textAnimator;
    public GameObject blackBarTop;
    public GameObject blackBarBottom;
    public Animator fourthyAnimator;

    public GameObject intro1Scene, introScene2, EqualityScene, EquityScene, LiberationScene, EndScene;
    public GameObject bkgBlurredParent, bkgBlurred, fenceBlurred, builderBlurred, dustBlurred, pitcherBlurred, batterBlurred, catcherBlurred, builder2Blurred, fan1Blurred, fan2Blurred, fan3Blurred, fan4Blurred;
    public GameObject fence, builder, dust, pitcher, batter, catcher, newFence1, newFence2, newFence3, builder2, fan1, fan2, fan3, fan4;

    public GameObject heartCounterButton, boxButton;

    public GameObject quoteUI, noClickUI;
    public Text quoteText, quoteByLineText;

    //Equality related references
    public GameObject introUI, equalityQuestionsUI, equityQuestionsUI, liberationQuestionsUI, fourthBoxQuestionsUI, equalityUI, liberationUI, clickTutorialUI, textTutorialUI, wideTutorialUI;
    public Animator clickTutorialAnimator; 
    public Animator equalityTextAnimator;
    public Image equalityBlackScreen;
    public Vector3 INIT_EQUALITY_POS = new Vector3(6.9f, -25.4f, -10f);
    public Character equality_ShortC, equality_BoxC1, equality_BoxC2;
    public GameObject successScene, equalityIntroScreen;
    public Text clickTutorialUIText, textTutorialText, wideTutorialText;

    //Equity related references
    public GameObject equityIntroScreen, equityUI;
    public Animator equityTextAnimator;
    public Image equityBlackScreen;
    public Character equity_TallC, equity_ShortC, equity_BoxC1, equity_BoxC2;
    public Vector3 INIT_EQUITY_POS = new Vector3(6.9f, -25.4f, -10f);

    //Liberation related references
    public GameObject liberationIntroScreen;
    public Animator liberationTextAnimator;
    public Image liberationBlackScreen;
    public Vector3 INIT_LIBERATION_POS = new Vector3(6.9f, -10.3f, -10f);
    public GameObject ideaBubble, ideaBubble2;
    public NewDrag panScript;
    public GameObject fireworks;
    public GameObject bottomBkgObject, topBkgObject;
    public GameObject liberationFourthy, winningTeam, gamePlayers;
    public Animator noCaneBerthaAnimator;

    //endscene references
    public GameObject endSceneUI;
    public GameObject endSceneIntroScreen;
    public Image endSceneBlackScreen;
    public GameObject endSceneChars, endFenceBuilder, lastFence1, lastFence2, lastFence3, lastFence4, endBuilderDust, sawer;
    public GameObject unfinishedGardenBox, finishedGardenBox;
    public Vector3 INIT_ENDSCENE_POS = new Vector3(18.3f, -9.6f, -10f);

    //credits references
    public GameObject creditsUI;
    public Image creditsBlackScreen;

    //questions references
    public GameObject questionsUI;
    public Image questionsBlackScreen;

    //ui buttons
    public GameObject mainMenuUI, cssLogo, game1Button, game2Button, game3Button, creditsButton, questionsButton, englishButton, spanishButton;
    public Sprite englishButtonSelectedSprite, englishButtonUnselectedSprite, spanishButtonSelectedSprite, spanishButtonUnselectedSprite;
    public Image titleImage;
    public Sprite engTitleSprite, spnTitleSprite;
    
    public LeanLocalization leanLocalizationScript; 

    //language buttons
    public GameObject languageMenuUI, langEnglishButton, langSpanishButton, langcssLogo;
    public Image languageBlackScreen;
    public GameObject binocularView;
    public GameObject intro2Background;

    public GameObject equalityRuntimeObjsParent;
    public GameObject equityRuntimeObjsParent;
    public GameObject liberationRuntimeObjsParent;

    public SitSite sitSite1, sitSite2, sitSite3;
    public GameObject finalBoxes;
    [SerializeField]
    public List<FencePoint> fencePoints; 
    public GameObject fencePointsParent;
    public List<Transform> fenceParts;
    public GameObject jumpMeter; 


    private Sequence instroS, intro2S, mainMenuS;
    private List<Character> equityCharacters = new List<Character>();
    

    // Use this for initializat
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
        //equityCharacters.Add(equity_BoxC1);
        equityCharacters.Add(equity_BoxC2);
    }



    public void Intro1_Enter()
    {

        //Sequence quoteS = DOTween.Sequence();
        //quoteS.Append(quoteText.transform.DOScale(0.17f, 20f).From());
        //quoteS.Join(quoteText.DOFade(0f, 5f).From().SetEase(Ease.InOutQuad));
        //quoteS.Append(quoteByLineText.DOFade(0f, 2f).From().SetEase(Ease.InOutQuad));
        //quoteS.AppendInterval(4f);
        //quoteS.Append(quoteText.DOFade(0f, 2f).SetEase(Ease.InOutQuad));
        //quoteS.Join(quoteByLineText.DOFade(0f, 2f).SetEase(Ease.InOutQuad));
        //quoteS.OnPlay(() => {
        //    quoteUI.SetActive(true);
        //});
        //quoteS.OnComplete(()=> {
        blackScreen.gameObject.SetActive(true);
        intro1Scene.SetActive(true);

        quoteUI.SetActive(false);

            SoundManager.instance.musicSource.clip = SoundManager.instance.steadycheer;
            SoundManager.instance.musicSource.volume = 0.25f;
            SoundManager.instance.musicSource.DOFade(0f, 2f).From().OnPlay(()=> {
                SoundManager.instance.musicSource.Play();
            });
           
            blackScreen.DOFade(0f, 9f).SetEase(Ease.InQuad);//.SetLoops(1, LoopType.Restart);
            Debug.Log("Welcome entered");

        
        
        //welcomeUI.SetActive(true);
        instroS = DOTween.Sequence();
            instroS.Append(DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 12, 10f));
            instroS.Join(Camera.main.transform.DOMove(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 25f, Camera.main.transform.position.z), 10f, false));
            instroS.InsertCallback(2f,() => {
                noClickUI.SetActive(true);
            });
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

       // });
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
        //clickTutorialUI.SetActive(true);
        //clickTutorialUI.transform.DOLocalMoveX(clickTutorialUI.transform.localPosition.x + 300f, 0.5f).From().SetEase(Ease.OutQuad);

        binocularView.transform.DOLocalMoveY(36f, 5f, false).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear).OnComplete(() => {
            DeBlur();
            Sequence zoomToPlay = DOTween.Sequence();
            binocularView.transform.DOGoto(0, false);
            zoomToPlay.Append(intro2Background.transform.DOScale(new Vector3(3f, 3f, 3f), 1.5f));
            zoomToPlay.Join(intro2Background.transform.DOLocalMoveY(-30f - 78.6f, 1.5f));
            zoomToPlay.AppendInterval(2f);

            zoomToPlay.AppendCallback(() =>
            {
                StartCoroutine(buildFence());
            });
        });
    }

    public void HandleIntro2HomeButton() {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);
        fsm.ChangeState(States.MainMenu);
    }

    public void HandleIntro2NextButton()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);
        fsm.ChangeState(States.Equality);
    }

    private bool intro2Clicked = false; 
    public void Intro2_Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //Debug.Log(binocularView.transform.position.y);
        //    clickTutorialUI.SetActive(false);
        //    if (newFence1.activeInHierarchy == true)
        //    {
        //        blackScreen.DOFade(0f, 1f).SetEase(Ease.InOutQuad).SetLoops(1, LoopType.Restart).OnComplete(() =>
        //        {
        //            fsm.ChangeState(States.Equality);
        //        });
        //    }
        //    else if(intro2Clicked == false)
        //    {
        //        intro2Clicked = true; 
        //        DeBlur();
        //        if (binocularView.transform.localPosition.y < 5f)
        //        {
        //            Sequence zoomToPlay = DOTween.Sequence();
        //            binocularView.transform.DOGoto(0, false);
        //            zoomToPlay.Append(intro2Background.transform.DOScale(new Vector3(3f, 3f, 3f), 1.5f));
        //            zoomToPlay.Join(intro2Background.transform.DOLocalMoveY(-30f - 78.6f, 1.5f));
        //            zoomToPlay.AppendInterval(2f);

        //            zoomToPlay.AppendCallback(() =>
        //            {
        //                StartCoroutine(buildFence());
        //            });

        //        }
        //        else
        //        {
        //            //zoom in on the action and go back to yo yo.
        //            Sequence zoomToPlay = DOTween.Sequence();
        //            binocularView.transform.DOGoto(0, false);
        //            zoomToPlay.Append(intro2Background.transform.DOScale(new Vector3(3f, 3f, 3f), 1.5f));
        //            zoomToPlay.Join(intro2Background.transform.DOLocalMoveY(-30f + 86.3f, 1.5f));
        //            zoomToPlay.AppendInterval(3f);
        //            zoomToPlay.InsertCallback(1.5f, () =>
        //            {
        //                pitcher.GetComponent<Animator>().SetTrigger("inAction");
        //            });
        //            zoomToPlay.InsertCallback(1.75f, () =>
        //            {
        //                batter.GetComponent<Animator>().SetTrigger("inAction");
        //            });

        //            zoomToPlay.AppendCallback(() =>
        //            {
        //                intro2Background.transform.SetScaleXYZ(1.3424f, 1.3424f, 1.3424f);
        //                intro2Background.transform.SetLocalY(-30f);
        //                Blur();
        //                binocularView.transform.DOLocalMoveY(36f, 5f, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        //                intro2Clicked = false;
        //                //binocularView.transform.DOLocalMoveY(20f, 3f, false).SetLoops(-1, LoopType.Yoyo);
        //            });

        //        }
        //    }

        //}
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
        yield return new WaitForSeconds(1.5f);
        dust.GetComponent<Animator>().SetTrigger("triggerBigPoof");
        yield return new WaitForSeconds(1f);
        newFence2.SetActive(true);
        yield return new WaitForSeconds(.25f);
        builder.transform.localPosition = new Vector3(builder.transform.localPosition.x + 14f, builder.transform.localPosition.y, builder.transform.localPosition.z);
        
        yield return new WaitForSeconds(1f);
        introUI.SetActive(true);
        
        //clickTutorialUIText.text = "Tap anywhere to \ngo to the fence";
        //clickTutorialUI.SetActive(true);
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
        
        intro2S.Join(builder2Blurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fan1Blurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fan2Blurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fan3Blurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fan4Blurred.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f).SetEase(Ease.InQuad));

        intro2S.Join(fence.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(builder.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(dust.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(pitcher.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(batter.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(catcher.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));

        intro2S.Join(builder2.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fan1.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fan2.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fan3.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        intro2S.Join(fan4.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InQuad));
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

        builder2Blurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        fan1Blurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        fan2Blurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        fan3Blurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        fan4Blurred.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        
        fence.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        builder.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        dust.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        pitcher.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        batter.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        catcher.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);

        builder2.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        fan1.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        fan2.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        fan3.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        fan4.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
    }

    public void Intro2_Exit()
    {
        Debug.Log("Intro2 exited");


        introScene2.SetActive(false);
        clickTutorialUI.SetActive(false);
        noClickUI.SetActive(false);
        introUI.SetActive(false);

    }


    private bool boxTutTextShow;
    public void Equality_Enter()
    {
        Debug.Log("Welcome to Equality");
        failedOnce = false;

        SoundManager.instance.musicSource.clip = SoundManager.instance.ballparkBkg;
        SoundManager.instance.musicSource.Play();
        if (SoundManager.instance.efxSource.isPlaying)
        {
            SoundManager.instance.efxSource.Stop();
        }
        
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

        clickTutorialUIText.GetComponent<LeanLocalizedText>().TranslationName = "Tap each heart to collect it";
        //clickTutorialUIText.text = "Tap each heart\n to collect it";
        clickTutorialUI.transform.DOLocalMoveX(clickTutorialUI.transform.localPosition.x + 350f, 0.5f).From().SetEase(Ease.OutQuad).SetDelay(5f).OnPlay(() =>
        {
            clickTutorialUI.SetActive(true);
        });
    }

    public IEnumerator BoxPlacementTutorial()
    {
        yield return new WaitForSeconds(0.25f);
        //clickTutorialUIText.text = "Tap anywhere to\nto drop the box";
        clickTutorialUIText.GetComponent<LeanLocalizedText>().TranslationName = "Tap anywhere to drop the box";

        //clickTutorialUI.transform.localPosition = new Vector3(Screen.width * 0.7f, Screen.height * 0.12f, 0);
        clickTutorialUI.SetActive(true);
        clickTutorialUI.transform.DOLocalMoveY(clickTutorialUI.transform.localPosition.y - 300f, 0.5f).From().SetEase(Ease.OutQuad);
    }

    public void Equality_Update()
    {
        //Debug.Log("heart num: " + HeartCounter.Instance.HeartsNumber);
        if (HeartCounter.Instance.HeartsNumber == 3 && boxTutTextShow == false)
        {
            boxTutTextShow = true;
            Debug.Log("3 hearts are there");
            //clickTutorialUIText.text = "Tap the icon\nto make a box";
            clickTutorialUIText.GetComponent<LeanLocalizedText>().TranslationName = "Tap the icon to make a box";

            //Screen.height * 0.12f
            // Screen.width * 0.7f
            clickTutorialUI.transform.localPosition = new Vector3(221f, -229f, 0f);
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
        Character tempShortC = equality_ShortC.GetComponent<Character>();
        tempShortC.meter.SetActive(false);
        tempShortC.transform.GetChild(tempShortC.transform.childCount - 1).gameObject.SetActive(true);
        foreach (Transform child in equalityRuntimeObjsParent.transform)
        {
            if (child.name.Contains("heart"))
            {
                Destroy(child.gameObject);
            }
        }
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
        textTutorialUI.SetActive(false);
        clickTutorialUI.SetActive(false);
        wideTutorialUI.SetActive(false);
        SuccessScreen mySuccessScreen = successScene.GetComponent<SuccessScreen>();
        mySuccessScreen.percentText.text = percent.ToString() + "%";
        Debug.Log("in turnonResultsScreen: " + percent);
        //if (fsm.CurrentStateMap.state.ToString() != States.Liberation.ToString())
        //{
            if (percent < 100f)
            {
                mySuccessScreen.nextButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                mySuccessScreen.nextButton.GetComponent<Button>().interactable = true;

            }
        //}
        //else
        //{
        //    Debug.Log("Current state nOT Liberation apparently");
        //    mySuccessScreen.nextButton.GetComponent<Button>().interactable = false;
        //}

        mySuccessScreen.AnimateStars(percent);
        if (fsm.CurrentStateMap.state.ToString() == States.Equality.ToString())
        {
            successScene.transform.SetParent(equalityUI.transform);
            successScene.transform.SetAsLastSibling();
            equalityBlackScreen.transform.SetAsLastSibling();
            mySuccessScreen.infoText.GetComponent<LeanLocalizedText>().TranslationName = "Equality Achieved";
            mySuccessScreen.questionLeanText.TranslationName = "Have you ever claimed a victory even if it wasn't a victory for everyone? Why or why not?";
            //mySuccessScreen.infoText.text = "Equality Achieved";
        }
        else if (fsm.CurrentStateMap.state.ToString() == States.Equity.ToString())
        {
            successScene.transform.SetParent(equityUI.transform);
            successScene.transform.SetAsLastSibling();
            equityBlackScreen.transform.SetAsLastSibling();
            mySuccessScreen.infoText.GetComponent<LeanLocalizedText>().TranslationName = "Equity Achieved";
            mySuccessScreen.questionLeanText.TranslationName = "How does a sense of urgency effect equity work?";
        }
        else if (fsm.CurrentStateMap.state.ToString() == States.Liberation.ToString())
        {
            Debug.Log("Success is parented Liberation now"); 
            successScene.transform.SetParent(liberationUI.transform);
            successScene.transform.SetAsLastSibling();
            liberationBlackScreen.transform.SetAsLastSibling();
            mySuccessScreen.infoText.GetComponent<LeanLocalizedText>().TranslationName = "Liberation Achieved";
            mySuccessScreen.questionLeanText.TranslationName = "Do you ever consider getting to know those responsible for building barriers to liberation? Why or why not?";
        }

        successScene.SetActive(true);
    }

    private bool failedOnce = false;
    public IEnumerator Failed()
    {
        if (failedOnce == false)
        {
            failedOnce = true; 
            clickTutorialUI.SetActive(false);
            yield return new WaitForSeconds(2f);
            if (fsm.CurrentStateMap.state.ToString() == States.Equality.ToString())
            {
                TurnOnResultsScreen(67f);
            }
            else if (fsm.CurrentStateMap.state.ToString() == States.Equity.ToString())
            {
                Debug.Log("should show Equity fail screen");
                float totalPassed = 3f;
                for (int i = 0; i < equityCharacters.Count; i++)
                {
                    if (equityCharacters[i].fsm.CurrentStateMap.state.ToString() != Character.States.Watching.ToString())
                    {
                        totalPassed = totalPassed - 1f;
                    }
                }

                TurnOnResultsScreen((float)Mathf.RoundToInt((totalPassed / 3f) * 100f));
            }
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
        Box sceneBox; 
        foreach (Transform child in equalityRuntimeObjsParent.transform)
        {
            sceneBox = child.GetComponent<Box>();
            if(sceneBox != null)
            {
                GridWorld.RegisterObstacle(sceneBox.transform, true);
            }
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
        failedOnce = false;

        SoundManager.instance.musicSource.clip = SoundManager.instance.ballparkBkg;
        SoundManager.instance.musicSource.Play();
        if (SoundManager.instance.efxSource.isPlaying)
        {
            SoundManager.instance.efxSource.Stop();
        }
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
        foreach (Transform child in equityRuntimeObjsParent.transform)
        {
            if (child.name.Contains("heart"))
            {
                Destroy(child.gameObject);
            }
        }

        if (wideTutorialUI.activeInHierarchy)
        {
            wideTutorialUI.SetActive(false);
        }

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
        if (wideTutorialUI.activeInHierarchy)
        {
            wideTutorialUI.SetActive(false);
        }

        equityBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeToBlackTween = equityBlackScreen.DOFade(1f, 2f).SetEase(Ease.OutQuad);
        yield return fadeToBlackTween.WaitForCompletion();
        //delete all instantiated boxes and hearts
        Box sceneBox;
        foreach (Transform child in equityRuntimeObjsParent.transform)
        {
            sceneBox = child.GetComponent<Box>();
            if (sceneBox != null)
            {
                GridWorld.RegisterObstacle(sceneBox.transform, true);
            }
            Destroy(child.gameObject);
        }

        EquityScene.SetActive(false);
        equityUI.SetActive(false);
        DOTween.KillAll();
    }


    public IEnumerator Liberation_Enter()
    {
        Debug.Log("Welcome to Liberation");
        

        SoundManager.instance.musicSource.clip = SoundManager.instance.ballparkBkg;
        SoundManager.instance.musicSource.Play();
        if (SoundManager.instance.efxSource.isPlaying)
        {
            SoundManager.instance.efxSource.Stop();
        }
        //reset heart counter, character Heart_StartPoint
        panScript.enabled = true; 
        successScene.SetActive(false);
        liberationTextAnimator.gameObject.SetActive(false);
        LiberationScene.SetActive(true);
        liberationUI.SetActive(true);
        heartCounterButton.transform.SetParent(liberationUI.transform);
        heartCounterButton.GetComponent<HeartCounter>().HeartsNumber = 0;
        boxButton.transform.SetParent(liberationUI.transform);
        liberationBlackScreen.transform.SetAsLastSibling();
        liberationBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeFromBlack = liberationBlackScreen.DOFade(1f, 3f).SetEase(Ease.InOutQuad).From();
        fadeFromBlack.OnUpdate(() => {
            if (fadeFromBlack.ElapsedPercentage() > 0.25f)
            {
                if (!liberationIntroScreen.activeInHierarchy)
                {
                    liberationIntroScreen.SetActive(true);
                }

            }
        });
        Camera.main.orthographicSize = 30;
        Camera.main.transform.position = INIT_LIBERATION_POS;

        yield return new WaitForSeconds(5f);
        //if (panScript.numOfPans == 0)
        //{
        clickTutorialUIText.GetComponent<LeanLocalizedText>().TranslationName = "Hold down and drag to move sideways";
        //    clickTutorialUIText.text = "Hold down & drag\n to pan sideways";

        clickTutorialUI.transform.localPosition = new Vector3(0f, -236f, 0f);//  new Vector3(0f, clickTutorialUI.transform.localPosition.y, clickTutorialUI.transform.position.z);
            Tween showTextTutorial = clickTutorialUI.transform.DOLocalMoveY(clickTutorialUI.transform.localPosition.y - 100f, 0.5f).From().SetEase(Ease.OutQuad);
            showTextTutorial.OnPlay(() =>
            {
                clickTutorialUI.SetActive(true);
                clickTutorialAnimator.SetBool("isDrag", true);
                StartCoroutine(TurnOffFirstLibTutText());
            });
        //}
        
    }

    public IEnumerator TurnOffFirstLibTutText()
    {
        yield return new WaitForSeconds(10f);
        if (clickTutorialUI.activeInHierarchy)
        {
            turnedOffClickTutorial = true;
            clickTutorialUI.SetActive(false);
            clickTutorialAnimator.SetBool("isDrag", false);
        }
    }

    public void CalculateScore_Liberation()
    {

        Debug.Log("should show Liberation fail screen");
        float totalCharacters = 0f;
        float passedCharacters = 0f; 

        foreach (Transform child in LiberationScene.transform)
        {
            Character c = child.GetComponent<Character>();
            if (c != null)
            {
                totalCharacters = totalCharacters + 1f;
                if (c.fsm != null)
                {
                    if (c.fsm.CurrentStateMap.state.ToString() == Character.States.Watching.ToString())
                    {
                        passedCharacters = passedCharacters + 1f;
                    }
                }
            }
        }

        TurnOnResultsScreen((float)Mathf.RoundToInt((passedCharacters / totalCharacters) * 100f));
    }


    private bool triggerJumpMeter = false;
    public int currenFencePower = 0;
    private int maxFencePower = 12;
    private bool initiateBreakFence = false;
    private bool turnedOffClickTutorial = false;
    public void Liberation_Update()
    {

        if (panScript.numOfPans > 1)
        {
            if (turnedOffClickTutorial == false)
            {

                if (clickTutorialUI.activeInHierarchy)
                {
                    turnedOffClickTutorial = true;
                    clickTutorialUI.SetActive(false);
                    clickTutorialAnimator.SetBool("isDrag", false);
                }
            }
        }
            //Debug.Log("howMany: " + howManyOnFence + " allChars: " + allFenceChars.Count); 
            if (howManyOnFence == allFenceChars.Count && howManyOnFence > 0)
        {
            if(triggerJumpMeter == false)
            {
                jumpMeter.SetActive(true);
                noClickUI.SetActive(false);
                triggerJumpMeter = true;
            }
        }

        if(currenFencePower >= maxFencePower)
        {
            //Debug.Log("FENCE IS BROKEN");
            if(initiateBreakFence == false)
            {
                initiateBreakFence = true;
                jumpMeter.SetActive(false);
                noClickUI.transform.SetParent(liberationUI.transform);
                noClickUI.transform.SetAsLastSibling();
                liberationBlackScreen.transform.SetAsLastSibling();
                noClickUI.SetActive(true);
                FenceParent.Instance.DestroyFence();

                for(int j=0; j< allNonFenceChars.Count; j++)
                {
                    allNonFenceChars[j].myAnimator.SetBool("isAltCheering", true);

                }
                for (int i =0; i < allFenceChars.Count; i++)
                {
                    allFenceChars[i].FallToGround();
                    
                }
                //noCaneBertha falls to ground and starts cheering
                noCaneBerthaAnimator.gameObject.transform.DOLocalMoveY(-15.1f, 10f).SetSpeedBased().SetDelay(2.25f).SetEase(Ease.InExpo).OnComplete(() => {
                    noCaneBerthaAnimator.SetBool("isCheering", true);
                });
                StartCoroutine(InitiateLiberationSuccess());
            }
        }
    }

    IEnumerator InitiateLiberationSuccess()
    {
        yield return new WaitForSeconds(6f);
        Sequence successS = DOTween.Sequence();
        successS.Append(DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 54, 5f));
        successS.Join(Camera.main.transform.DOMove(new Vector3(23f, 5f, Camera.main.transform.position.z), 5f, false));
        successS.InsertCallback(2f, () =>
        {
            SoundManager.instance.musicSource.clip = SoundManager.instance.bigcheer;
            SoundManager.instance.musicSource.Play();
            //bottomBkgObject.transform.localPosition = new Vector3(10f, 45f, 80f);
            //topBkgObject.transform.localPosition = new Vector3(0f, 0f, 0f);

        });
        successS.InsertCallback(5f, () =>
        {
            liberationTextAnimator.gameObject.SetActive(true);
            liberationTextAnimator.Play("Expand");
        });
        successS.InsertCallback(6f, () =>
        {
            fireworks.SetActive(true);
            SoundManager.instance.PlaySingle(SoundManager.instance.fireworks);
        });
        successS.InsertCallback(17f, () =>
        {
            ///fsm.ChangeState(States.EndScene);
            //instead show scorecard
            TurnOnResultsScreen(100f);
        });
        


    }

    private Tween backButtonCutSceneTween;
    private Tween heartCounterCutSceneTween;
    private Tween countDownCutSceneTween;
    private Tween boxButtonCutSceneTween;
    public void BeginCutScene()
    {

        Camera.main.GetComponent<NewDrag>().enabled = false;
        CountDownTimer.Instance.countDownTween.TogglePause();

        Transform backButton = liberationUI.transform.Find("BackButton");

        //back button
        backButtonCutSceneTween = backButton.DOLocalMoveY(400f, 1f).SetLoops(2, LoopType.Yoyo);
        backButtonCutSceneTween.OnStepComplete(() =>
        {
            if (backButtonCutSceneTween.CompletedLoops() == 1)
            {
                backButtonCutSceneTween.TogglePause();
            }
        });

        //heart counter
        heartCounterCutSceneTween = HeartCounter.Instance.transform.DOLocalMoveY(400f, 1f).SetLoops(2, LoopType.Yoyo);
        heartCounterCutSceneTween.OnStepComplete(() =>
        {
            if(heartCounterCutSceneTween.CompletedLoops() == 1)
            {
                heartCounterCutSceneTween.TogglePause(); 
            }
        });
        //count down timer
        countDownCutSceneTween = CountDownTimer.Instance.transform.DOLocalMoveY(600f, 1f).SetLoops(2, LoopType.Yoyo);
        countDownCutSceneTween.OnStepComplete(() =>
        {
            if (countDownCutSceneTween.CompletedLoops() == 1)
            {
                countDownCutSceneTween.TogglePause();
            }
        });
        //box button
        boxButtonCutSceneTween = boxButton.transform.DOLocalMoveY(-400f, 1f).SetLoops(2, LoopType.Yoyo);
        boxButtonCutSceneTween.OnStepComplete(() =>
        {
            if (boxButtonCutSceneTween.CompletedLoops() == 1)
            {
                boxButtonCutSceneTween.TogglePause();
            }
        });
    }


    public void EndCutScene()
    {

        Camera.main.GetComponent<NewDrag>().enabled = true;
        CountDownTimer.Instance.countDownTween.TogglePause();

        backButtonCutSceneTween.TogglePause();
        heartCounterCutSceneTween.TogglePause();
        countDownCutSceneTween.TogglePause();
        boxButtonCutSceneTween.TogglePause();

    }

    private bool triggeredIB = false;
    public void TriggerIdeaBubble(SitPoint point1, SitPoint point2)
    {
        if(triggeredIB == false)
        {
            triggeredIB = true;
            Vector3 idea1LocalPos = new Vector3(point1.occupant.speechBubble.transform.localPosition.x, point1.occupant.speechBubble.transform.localPosition.y, Mathf.Abs(point1.occupant.speechBubble.transform.localPosition.z));
            Vector3 idea2LocalPos = new Vector3(point2.occupant.speechBubble.transform.localPosition.x, point2.occupant.speechBubble.transform.localPosition.y, Mathf.Abs(point2.occupant.speechBubble.transform.localPosition.z));

            BeginCutScene();

            Camera.main.transform.DOMoveX(point1.transformPoint.parent.position.x + 12f, 20f).SetSpeedBased().SetDelay(1f).SetEase(Ease.Linear).OnComplete(()=> {

                ideaBubble.gameObject.transform.SetParent(point1.transformPoint);
                ideaBubble2.gameObject.transform.SetParent(point2.transformPoint);
                ideaBubble2.transform.SetScaleXY(5f, 4f);
                ideaBubble.transform.SetScaleXY(5f, 4f);
                //Debug.Log("should have parented by now");

                //ideaBubble.transform.localPosition = Vector3.zero;
                //ideaBubble2.transform.localPosition = Vector3.zero ;
                //space out the bubbles a bit
                //if (point1.yAngle > 1f)
                //{
                //    ideaBubble.transform.SetLocalX(ideaBubble.transform.localPosition.x + 15f);
                //    ideaBubble2.transform.SetLocalX(ideaBubble.transform.localPosition.x - 5f);
                //}
                //else
                //{
                //    ideaBubble.transform.SetLocalX(ideaBubble.transform.localPosition.x - 5f);
                //    ideaBubble2.transform.SetLocalX(ideaBubble.transform.localPosition.x + 15f);
                //}
                ideaBubble2.transform.SetLocalX(-4.5f);
                ideaBubble.transform.SetLocalX(6.5f);

                Sequence ideaBubbleS = DOTween.Sequence();
                
                ideaBubbleS.Append(ideaBubble.transform.DOLocalMoveY(7.5f, 2f).SetSpeedBased().SetEase(Ease.Linear));
                ideaBubbleS.Join(ideaBubble2.transform.DOLocalMoveY(7.5f, 2f).SetSpeedBased().SetEase(Ease.Linear));
                ideaBubbleS.OnPlay(() =>
                {
                    ideaBubble.SetActive(true);
                    ideaBubble2.SetActive(true);
                });

                bool fadeInOnce = false;
                ideaBubbleS.OnUpdate(() => {
                    if(ideaBubbleS.ElapsedPercentage() > 0.5f)
                    {
                        if (fadeInOnce == false)
                        {
                            fadeInOnce = true;
                            ideaBubble.GetComponent<SpriteRenderer>().DOFade(1f, 1f).From();
                            ideaBubble2.GetComponent<SpriteRenderer>().DOFade(1f, 1f).From();
                        }
                    }
                });

                ideaBubbleS.OnComplete(() => {
                    ideaBubble.transform.DOScale(new Vector3(ideaBubble.transform.localScale.x - 0.1f, ideaBubble.transform.localScale.y-0.1f, ideaBubble.transform.localScale.z), .8f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);
                    ideaBubble2.transform.DOScale(new Vector3(ideaBubble2.transform.localScale.x - 0.1f, ideaBubble2.transform.localScale.y - 0.1f, ideaBubble2.transform.localScale.z), .8f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);

                    //make them interactive
                    ideaBubble.GetComponent<BoxCollider2D>().enabled = true;
                    ideaBubble2.GetComponent<BoxCollider2D>().enabled = true;
                    //click tutorial
                    clickTutorialUIText.GetComponent<LeanLocalizedText>().TranslationName = "Tap on one of the ideas to try it";
                    //clickTutorialUIText.text = "Tap on one of the\n ideas to try it";
                    clickTutorialUI.transform.localPosition = new Vector3(0f, -236f, 0f); 
                    //clickTutorialUI.transform.localPosition = new Vector3(0f, clickTutorialUI.transform.localPosition.y, clickTutorialUI.transform.position.z);
                    //Tween showTextTutorial = clickTutorialUI.transform.DOLocalMoveY(clickTutorialUI.transform.localPosition.y - 100f, 0.5f).From().SetEase(Ease.OutQuad);
                    //showTextTutorial.OnPlay(() =>
                    //{
                    clickTutorialUI.SetActive(true);
                    //});
                });           
            });
        }
    }


    private bool clickIdeaBubble2 = false; 
    public void HandleIdeaBubble2()
    {
        if (clickIdeaBubble2 == false)
        {
            clickIdeaBubble2 = true; 
            clickTutorialUI.SetActive(false);
            SoundManager.instance.PlaySingle(SoundManager.instance.select, 0.75f);

            Vector3 endPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height * 0.5f, Camera.main.nearClipPlane));
            ideaBubble2.transform.DOKill();
            Sequence ideaBubble2S = DOTween.Sequence();
            ideaBubble2S.SetDelay(0.5f);
            ideaBubble2S.Append(ideaBubble.transform.GetComponent<SpriteRenderer>().DOFade(0f, 1f));
            ideaBubble2S.Join(ideaBubble.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0f, 1f));
            ideaBubble2S.Join(ideaBubble2.transform.DOMove(new Vector3(endPoint.x, endPoint.y, ideaBubble2.transform.position.z), 1f).SetEase(Ease.OutQuad));
            ideaBubble2S.Join(ideaBubble2.transform.DOScale(new Vector3(8f, 6.6f, ideaBubble2.transform.localScale.z), 1f).SetEase(Ease.OutQuad));

            ideaBubble2S.AppendInterval(5f);
            ideaBubble2S.OnComplete(() =>
            {
                ideaBubble.SetActive(false);
                ideaBubble2.SetActive(false);
            //Camera.main.GetComponent<NewDrag>().enabled = true;
            //CountDownTimer.Instance.countDownTween.TogglePause();
                EndCutScene();
                textTutorialText.GetComponent<LeanLocalizedText>().TranslationName = "Continue to create boxes to achieve perfect equity";
                //textTutorialText.text = "Continue to create boxes\n to achieve perfect equity";
                textTutorialUI.transform.localPosition = new Vector3(0f, -236f, 0f);
                //textTutorialUI.transform.localPosition = new Vector3(0f, textTutorialUI.transform.localPosition.y, textTutorialUI.transform.position.z);
                //Tween showTextTutorial = textTutorialUI.transform.DOLocalMoveY(textTutorialUI.transform.localPosition.y - 100f, 0.5f).From().SetEase(Ease.OutQuad);
                //showTextTutorial.OnPlay(() =>
                //{
                textTutorialUI.SetActive(true);
                StartCoroutine(TurnOffIdeaBubble2HelpText());
            //});
            });

        }
    }

    public IEnumerator TurnOffIdeaBubble2HelpText()
    {
        yield return new WaitForSeconds(4f);
        textTutorialUI.SetActive(false);
    }

    //track if character is going to stand on fence
    public List<Character> allFenceChars = new List<Character>();
    public List<Character> allNonFenceChars = new List<Character>();
    private bool clickIdeaBubble = false;
    public int howManyOnFence = 0; 
    public void HandleIdeaBubble()
    {
        if (clickIdeaBubble == false)
        {

            clickIdeaBubble = true;

            //Debug.Log("idea bubble clicked");
            //disable camera drag
            clickTutorialUI.SetActive(false);
            SoundManager.instance.PlaySingle(SoundManager.instance.select, 0.75f);
            ideaBubble2.SetActive(false);
            Camera.main.GetComponent<NewDrag>().enabled = false;
            Vector3 endPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height * 0.5f, Camera.main.nearClipPlane));
            ideaBubble.transform.DOKill();
            Sequence ideaBubbleS = DOTween.Sequence();
            ideaBubbleS.SetDelay(0.5f);
            ideaBubbleS.Append(ideaBubble2.transform.GetComponent<SpriteRenderer>().DOFade(0f, 1f));
            ideaBubbleS.Join(ideaBubble2.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0f, 1f));
            ideaBubbleS.Join(ideaBubble.transform.DOMove(new Vector3(endPoint.x, endPoint.y, ideaBubble.transform.position.z), 1f).SetEase(Ease.OutQuad));
            ideaBubbleS.Join(ideaBubble.transform.DOScale(new Vector3(8f, 6.6f, ideaBubble.transform.localScale.z), 1f).SetEase(Ease.OutQuad));
            ideaBubbleS.AppendCallback(() =>
            {
                finalBoxes.SetActive(true);
                CountDownTimer.Instance.countDownTween.Kill();
                CountDownTimer.Instance.transform.gameObject.SetActive(false);
                HeartCounter.Instance.transform.gameObject.SetActive(false);
                boxButton.gameObject.SetActive(false);
                noCaneBerthaAnimator.gameObject.SetActive(true);
            //turn off back button
            liberationUI.transform.GetChild(0).gameObject.SetActive(false);
            });
            ideaBubbleS.AppendInterval(5f);
            ideaBubbleS.OnComplete(() =>
            {
                ideaBubble.SetActive(false);
                noClickUI.SetActive(true);
                foreach (Transform child in LiberationScene.transform)
                {
                    Character c = child.GetComponent<Character>();
                    if (c != null)
                    {
                        //c.fsm.CurrentStateMap.state.ToString() == Character.States.GaveUp.ToString() &&
                        if (!c.transform.name.Contains("CharC"))
                        {
                            allFenceChars.Add(c);
                            Debug.Log("little shit: " + c.fsm.CurrentStateMap.state.ToString());
                            if (c.fsm.CurrentStateMap.state.ToString() != Character.States.GaveUp.ToString())
                            {
                                if (c.myBoxes.Count > 0)
                                {
                                    //for Char B & D who might be standing on boxes, first get down from them
                                    c.GetDownBox_ThenFence();
                                }
                                else
                                {
                                    //for Char A who's watching and for Char B & D who might be waiting around with some resolve left
                                    c.skipSittingDown = true;
                                    c.fsm.ChangeState(Character.States.GaveUp);
                                    StartCoroutine(c.GetOnFence());
                                }

                            }
                            else
                            {
                                StartCoroutine(c.GetOnFence());
                            }

                        }
                        else
                        {
                            allNonFenceChars.Add(c);
                            c.StandUp();
                        }
                    }
                }
                //move camera final Boxes
                Camera.main.transform.DOMoveX(finalBoxes.transform.position.x, 20f).SetSpeedBased().SetEase(Ease.Linear).SetDelay(2f);
            });
        }
        
    }

    public IEnumerator Liberation_Exit()
    {
        Debug.Log("Exiting Liberation");
        Camera.main.GetComponent<NewDrag>().enabled = false;
        liberationBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeToBlackTween = liberationBlackScreen.DOFade(1f, 5f).SetEase(Ease.OutQuad);
        yield return fadeToBlackTween.WaitForCompletion();
        //delete all instantiated boxes and hearts
        foreach (Transform child in liberationRuntimeObjsParent.transform)
        {
            Destroy(child.gameObject);
        }
        LiberationScene.SetActive(false);
        liberationUI.SetActive(false);
        foreach (PolygonCollider2D po in Resources.FindObjectsOfTypeAll(typeof(PolygonCollider2D)) as PolygonCollider2D[])
        {
            Destroy(po.gameObject);
        }
        DOTween.KillAll();
    }


    public IEnumerator EndScene_Enter()
    {
        Debug.Log("Welcome to EndScene");


        successScene.SetActive(false);
        SoundManager.instance.musicSource.clip = SoundManager.instance.birds;
        SoundManager.instance.musicSource.volume = 0.4f; 
        SoundManager.instance.musicSource.DOFade(0f, 1f).From().OnPlay(()=> {
            SoundManager.instance.musicSource.Play();
        });
        SoundManager.instance.musicSource.volume = 0.4f; 
        //SoundManager.instance.PlaySingle(SoundManager.instance.saw, 0.2f);


        EndScene.SetActive(true);
        endSceneUI.SetActive(true);
        endSceneBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeFromBlack = endSceneBlackScreen.DOFade(1f, 5f).SetEase(Ease.InOutQuad).From();
        fadeFromBlack.OnUpdate(() => {
            if (fadeFromBlack.ElapsedPercentage() > 0.25f)
            {
                if (!endSceneIntroScreen.activeInHierarchy)
                {
                    endSceneIntroScreen.SetActive(true);
                    noClickUI.transform.SetParent(endSceneUI.transform);
                    noClickUI.transform.SetAsLastSibling();
                    endSceneBlackScreen.transform.SetAsLastSibling();
                    noClickUI.SetActive(true);
                }

            }
        });

        Camera.main.transform.position = INIT_ENDSCENE_POS;
        Camera.main.orthographicSize = 65;

        yield return new WaitForSeconds(18f);

        Sequence fadingS = DOTween.Sequence();
        fadingS.Append(endSceneBlackScreen.DOFade(1f, 4f).SetEase(Ease.InOutQuad));
        fadingS.Join(SoundManager.instance.efxSource.DOFade(0f, 2f));
        fadingS.AppendCallback(() =>
        {
            endSceneChars.SetActive(false);
            sawer.SetActive(false);
            //show a finished garden bed
            unfinishedGardenBox.SetActive(false);
            finishedGardenBox.SetActive(true);
            //SoundManager.instance.efxSource.volume = 1f; 
            
        });
        //fadingS.AppendInterval(1f);
        fadingS.Append(endSceneBlackScreen.DOFade(1f, 2f).SetEase(Ease.InOutQuad).From());

        
        SoundManager.instance.musicSource.clip = SoundManager.instance.hammer;
        SoundManager.instance.musicSource.volume = 0.25f;
        SoundManager.instance.musicSource.DOFade(0f, 2f).From().OnPlay(() => {
            SoundManager.instance.musicSource.Play();
        });
        StartCoroutine(buildEndFence());
        

    }

    public IEnumerator buildEndFence()
    {
        endBuilderDust.GetComponent<Animator>().SetTrigger("triggerBigPoof");
        yield return new WaitForSeconds(1.25f);
        lastFence1.SetActive(true);
        yield return new WaitForSeconds(.25f);
        endFenceBuilder.transform.localPosition = new Vector3(endFenceBuilder.transform.localPosition.x + 14f, endFenceBuilder.transform.localPosition.y, endFenceBuilder.transform.localPosition.z);
        yield return new WaitForSeconds(3f);
        endBuilderDust.GetComponent<Animator>().SetTrigger("triggerBigPoof");
        yield return new WaitForSeconds(1.25f);
        lastFence2.SetActive(true);
        yield return new WaitForSeconds(.25f);
        endFenceBuilder.transform.localPosition = new Vector3(endFenceBuilder.transform.localPosition.x + 16f, endFenceBuilder.transform.localPosition.y, endFenceBuilder.transform.localPosition.z);
        yield return new WaitForSeconds(3f);
        endBuilderDust.GetComponent<Animator>().SetTrigger("triggerBigPoof");
        yield return new WaitForSeconds(1.25f);
        lastFence3.SetActive(true);
        yield return new WaitForSeconds(.25f);
        endFenceBuilder.transform.localPosition = new Vector3(endFenceBuilder.transform.localPosition.x + 16f, endFenceBuilder.transform.localPosition.y, endFenceBuilder.transform.localPosition.z);
        yield return new WaitForSeconds(3f);
        endBuilderDust.GetComponent<Animator>().SetTrigger("triggerBigPoof");
        yield return new WaitForSeconds(1.25f);
        lastFence4.SetActive(true);
        yield return new WaitForSeconds(.25f);
        endFenceBuilder.transform.localPosition = new Vector3(endFenceBuilder.transform.localPosition.x + 16f, endFenceBuilder.transform.localPosition.y, endFenceBuilder.transform.localPosition.z);
        yield return new WaitForSeconds(3f);
        fourthBoxQuestionsUI.SetActive(true);
        noClickUI.SetActive(false);

    }

    public void HandleFourthBoxCreditsButton()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);
        fsm.ChangeState(States.Credits);
    }

    public void HandleFourthBoxMainMenuButton()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);
        fsm.ChangeState(States.MainMenu);
    }


    public IEnumerator EndScene_Exit()
    {
        Debug.Log("Exiting EndScene");
        liberationBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeToBlackTween = endSceneBlackScreen.DOFade(1f, 3f).SetEase(Ease.InOutQuad);
        fourthBoxQuestionsUI.SetActive(false);
        yield return fadeToBlackTween.WaitForCompletion();

        
        EndScene.SetActive(false);
        endSceneUI.SetActive(false);
        DOTween.KillAll();
    }



    public void Credits_Enter()
    {
        Debug.Log("Welcome to Credits");

        creditsUI.SetActive(true);
        creditsBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        creditsBlackScreen.DOFade(1f, 3f).SetEase(Ease.InOutQuad).From();
    }


    public void HandleCreditsFinished()
    {
        
        fsm.ChangeState(States.MainMenu);
    }


    public IEnumerator Credits_Exit()
    {
        Debug.Log("Exiting Credits");
        creditsBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeToBlackTween = creditsBlackScreen.DOFade(1f, 2f).SetEase(Ease.OutQuad);
        yield return fadeToBlackTween.WaitForCompletion();

        creditsUI.SetActive(false);
        DOTween.KillAll();
    }

    public void Questions_Enter()
    {
        Debug.Log("Welcome to Credits");

        questionsUI.SetActive(true);
        questionsBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        questionsBlackScreen.DOFade(1f, 3f).SetEase(Ease.InOutQuad).From();
    }


    public IEnumerator Questions_Exit()
    {
        Debug.Log("Exiting Questions");
        questionsBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeToBlackTween = questionsBlackScreen.DOFade(1f, 2f).SetEase(Ease.OutQuad);
        yield return fadeToBlackTween.WaitForCompletion();

        questionsUI.SetActive(false);
        DOTween.KillAll();
    }


    public void LanguageMenu_Enter()
    {
        SoundManager.instance.musicSource.clip = SoundManager.instance.menuBkg;
        SoundManager.instance.musicSource.volume = 0.15f;
        SoundManager.instance.musicSource.DOFade(0f, 2f).From().OnPlay(() => {
            SoundManager.instance.musicSource.Play();
        });

        languageMenuUI.SetActive(true);
        languageBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        languageBlackScreen.DOFade(1f, 3f).SetEase(Ease.InOutQuad).From();

        //blackScreen.gameObject.SetActive(true);
        //blackScreen.DOFade(0f, 1f).SetEase(Ease.InOutQuad).SetLoops(1, LoopType.Restart);

        Sequence languageMenuS = DOTween.Sequence();
        languageMenuS.Append(langEnglishButton.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        languageMenuS.Append(langSpanishButton.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        languageMenuS.AppendCallback(() =>
        {
            langcssLogo.SetActive(true);
            mainMenuS.Append(langcssLogo.transform.DOLocalMoveY(cssLogo.transform.localPosition.y - 150f, 0.5f).From().SetEase(Ease.OutBounce));
        });

    }

    public void LangEnglishButtonPressed()
    {
        EnglishButtonPressed();
        fsm.ChangeState(States.Intro1);
    }

    public void LangSpanishButtonPressed()
    {
        //SoundManager.instance.PlaySingle(SoundManager.instance.take);
        SpanishButtonPressed();
        fsm.ChangeState(States.Intro1);
    }

    public void LanguageMenu_Update()
    {
    }

    public IEnumerator LanguageMenu_Exit()
    {
        Debug.Log("Exiting Language Menu");
        languageBlackScreen.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeToBlackTween = languageBlackScreen.DOFade(1f, 2f).SetEase(Ease.OutQuad);
        yield return fadeToBlackTween.WaitForCompletion();

        languageMenuUI.SetActive(false);
    }


    public void MainMenu_Enter()
    {
        Debug.Log("Welcome to MainMenu");


        SoundManager.instance.musicSource.clip = SoundManager.instance.menuBkg;
        SoundManager.instance.musicSource.volume = 0.15f;
        SoundManager.instance.musicSource.DOFade(0f, 2f).From().OnPlay(()=> {
            SoundManager.instance.musicSource.Play();
        });

        SoundManager.instance.efxSource.volume = 1f; 
        if (SoundManager.instance.efxSource.isPlaying)
        {
            SoundManager.instance.efxSource.Stop();
        }
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
            //questionsButton.GetComponent<Button>().interactable = true;
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

        nextSceneState = States.Liberation;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuestionsButtonPressed()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.error);
        fsm.ChangeState(States.Questions);
    }

    public void CreditsButtonPressed()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);
        fsm.ChangeState(States.Credits);
    }
    public void EnglishButtonPressed()
    {
        //SoundManager.instance.PlaySingle(SoundManager.instance.take);
        if (englishButton.GetComponent<Image>().sprite != englishButtonSelectedSprite)
        {
            SoundManager.instance.PlaySingle(SoundManager.instance.take);
            englishButton.GetComponent<Image>().sprite = englishButtonSelectedSprite;
            spanishButton.GetComponent<Image>().sprite = spanishButtonUnselectedSprite;
            titleImage.sprite = engTitleSprite; 
        }
    }

    public void SpanishButtonPressed()
    {
        
        //change color of spanish button
        //change sprite of english button
        if(spanishButton.GetComponent<Image>().sprite != spanishButtonSelectedSprite) {
            SoundManager.instance.PlaySingle(SoundManager.instance.take);
            spanishButton.GetComponent<Image>().sprite = spanishButtonSelectedSprite;
            englishButton.GetComponent<Image>().sprite = englishButtonUnselectedSprite;
            titleImage.sprite = spnTitleSprite;
        }

    }

    //public void MainMenu_Update()
    //{

    //}

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
            return liberationRuntimeObjsParent.transform;
        }
        else
        {
            Debug.Log("something is wrong");
            return equityRuntimeObjsParent.transform;
        }
    }

}
