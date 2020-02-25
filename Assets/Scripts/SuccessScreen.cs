using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lean.Localization;

public class SuccessScreen : MonoBehaviour
{
    public static SuccessScreen Instance { get; private set; }


    public GameObject retryButton, homeButton, nextButton, textParent, questionsButton;
    public Text infoText, percentText;
    public LeanLocalizedText questionLeanText; 
    public GameObject star1, star1Fill, star2, star2Fill, star3, star3Fill;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        Sequence resultsS = DOTween.Sequence();
        resultsS.Append(textParent.transform.DOLocalMoveY(infoText.transform.localPosition.y + 50f, 0.5f).From().SetEase(Ease.OutBounce));
        resultsS.Append(questionsButton.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        resultsS.Append(retryButton.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        resultsS.Join(homeButton.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        resultsS.Join(nextButton.transform.DOScale(Vector3.zero, 0.25f).From().SetEase(Ease.OutQuad));
        resultsS.Append(percentText.transform.DOScale(0f, 0.25f).From().SetEase(Ease.OutBounce));
        Debug.Log("Success was turned on!"); 
    }

    void OnDisable()
    {
        star1Fill.SetActive(false);
        star2Fill.SetActive(false);
        star3Fill.SetActive(false);
    }

    public void AnimateStars(float percent)
    {
        int numStars = 0;
        if(percent > 0f && percent <= 50f)
        {
            numStars = 1; 
        }else if (percent > 50f && percent <= 75f)
        {
            numStars = 2;
        }
        else if (percent > 75f && percent <= 101f)
        {
            numStars = 3;
        }

        if (numStars > 0)
        {
            Sequence starS = DOTween.Sequence();
            starS.SetDelay(1.5f);
            starS.Append(star1Fill.transform.DOLocalMoveX(-116f, 0.2f).From().SetEase(Ease.OutQuad).OnPlay(()=> {
                star1Fill.SetActive(true);
                SoundManager.instance.PlaySingle(SoundManager.instance.coincollected, 0.15f);
            }));
            starS.Append(star1.transform.DOScale(1.2f, 0.2f).SetLoops(2,LoopType.Yoyo).SetEase(Ease.OutQuad));

            if (numStars > 1)
            {
                starS.Append(star2Fill.transform.DOLocalMoveX(-116f, 0.2f).From().SetEase(Ease.OutQuad).SetDelay(0.01f).OnPlay(() =>
                {
                    star2Fill.SetActive(true);
                    SoundManager.instance.PlaySingle(SoundManager.instance.coincollected, 0.15f);
                }));
                starS.Append(star2.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad));

                if (numStars > 2)
                {
                    starS.Append(star3Fill.transform.DOLocalMoveX(-116f, 0.2f).From().SetEase(Ease.OutQuad).SetDelay(0.05f).OnPlay(() =>
                    {
                        star3Fill.SetActive(true);
                        SoundManager.instance.PlaySingle(SoundManager.instance.coincollected, 0.15f);
                    }));
                    starS.Append(star3.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad));
                }
            }
        }
    }

    public void HandleHomeButton()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);

        //if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
        //{
            GameManager2.instance.fsm.ChangeState(GameManager2.States.MainMenu);
        //}
    }

    public void HandleRetryButton()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);

        //if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
        //{
            GameManager2.instance.fsm.ChangeState(GameManager2.States.Restart);
        //}
    }

    public void HandleNextButton()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);

        if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
        {
            //GameManager2.unlockGameNum = GameManager2.unlockGameNum + 1;
            Debug.Log("NEXT pushed"); 
            GameManager2.instance.fsm.ChangeState(GameManager2.States.Equity);
        }else if(GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equity.ToString())
        {
            //GameManager2.unlockGameNum = GameManager2.unlockGameNum + 1; 
            GameManager2.instance.fsm.ChangeState(GameManager2.States.Liberation);
        }
    }

    public void HandleQuestionsButton()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.take);

        if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
        {
            //GameManager2.unlockGameNum = GameManager2.unlockGameNum + 1;
            Debug.Log("Questions pushed");
            GameManager2.instance.equalityQuestionsUI.SetActive(true);
        }
        else if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equity.ToString())
        {
            //GameManager2.unlockGameNum = GameManager2.unlockGameNum + 1; 
            GameManager2.instance.equityQuestionsUI.SetActive(true);
        }
        else if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Liberation.ToString())
        {
            //GameManager2.unlockGameNum = GameManager2.unlockGameNum + 1; 
            GameManager2.instance.liberationQuestionsUI.SetActive(true);
        }
    }


}
