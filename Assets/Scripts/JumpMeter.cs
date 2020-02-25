using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lean.Localization;

public class JumpMeter : MonoBehaviour
{
    public delegate void InitiateJump(bool sweetSpot);
    public static event InitiateJump OnInitiateJump;
    public Image marker;
    private float meterLength = 490f; 
    private float meterSpeed = 700f;
    private List<float> sweetSpotPoints = new List<float> { -542f, -382f};
    private float orgMarkerX; 

    void Awake()
    {
        orgMarkerX = transform.localPosition.x; 
    }

    private Tween markerTween = null;
    void OnEnable()
    {
        markerTween = marker.transform.DOLocalMoveX(marker.transform.localPosition.x + meterLength, meterSpeed).SetSpeedBased().SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        GameManager2.instance.wideTutorialText.GetComponent<LeanLocalizedText>().TranslationName = "Tap anywhere while the marker is in the green to jump together";

        //GameManager2.instance.wideTutorialText.text = "Tap anywhere when the\n marker overlaps the green";
        
        GameManager2.instance.wideTutorialUI.transform.localPosition = new Vector3(0f, -236f, 0f);// new Vector3(0f, GameManager2.instance.clickTutorialUI.transform.localPosition.y, GameManager2.instance.wideTutorialUI.transform.localPosition.z);
        GameManager2.instance.wideTutorialUI.SetActive(true);
        //GameManager2.instance.wideTutorialUI.transform.DOLocalMoveY(GameManager2.instance.wideTutorialUI.transform.localPosition.y - 100f, 0.5f).From().SetEase(Ease.InOutQuad).SetDelay(0.5f).OnPlay(() =>
        //{
        
        //});
    }

    private bool listenToInput = true; 
    void Update()
    {
        if (listenToInput)
        {
            if (Input.GetMouseButtonUp(0))
            {
                bool sweetSpot; 
                if (marker.transform.localPosition.x > sweetSpotPoints[0] && marker.transform.localPosition.x < sweetSpotPoints[1])
                {
                    Debug.Log("in the sweetspot");
                    OnInitiateJump?.Invoke(true);
                    sweetSpot = true;
                    //GameManager2.instance.currenFencePower = GameManager2.instance.currenFencePower + 3; 
                }
                else
                {
                    Debug.Log("NOT in the sweetspot");
                    OnInitiateJump?.Invoke(false);
                    sweetSpot = false; 
                    //GameManager2.instance.currenFencePower = GameManager2.instance.currenFencePower + 1;
                }
                //jump noCaneBertha
                GameManager2.instance.noCaneBerthaAnimator.gameObject.transform.DOLocalMoveY(13.7f, 0.75f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutCirc);
                SoundManager.instance.PlaySingle(SoundManager.instance.buzzStop, 0.75f);
                markerTween.TogglePause();
                listenToInput = false;
                StartCoroutine(ResetListenToInput(sweetSpot));
                if (GameManager2.instance.wideTutorialUI.activeInHierarchy)
                {
                    GameManager2.instance.wideTutorialUI.SetActive(false);
                }
            }
        }
    }

    IEnumerator ResetListenToInput(bool sweetSpot)
    {
        float waitTime; 
        if (sweetSpot) {
            waitTime = 0.75f * 2f;
        } else
        {
            waitTime = 2.5f;
        }
        yield return new WaitForSeconds(waitTime);
        if (sweetSpot)
        {
            GameManager2.instance.currenFencePower = GameManager2.instance.currenFencePower + 3;
        }
        else
        {
            GameManager2.instance.currenFencePower = GameManager2.instance.currenFencePower + 1;
        }
        listenToInput = true;
        markerTween.TogglePause();
    }
}
