using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JumpMeter : MonoBehaviour
{
    public delegate void InitiateJump(bool sweetSpot);
    public static event InitiateJump OnInitiateJump;
    public Image marker;
    private float meterLength = 490f; 
    private float meterSpeed = 500f;
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
                markerTween.TogglePause();
                listenToInput = false;
                StartCoroutine(ResetListenToInput(sweetSpot));
            }
        }
    }

    IEnumerator ResetListenToInput(bool sweetSpot)
    {
        
        yield return new WaitForSeconds(0.85f);
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
