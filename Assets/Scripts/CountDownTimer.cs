using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CountDownTimer : MonoBehaviour
{

    public Image meter;
    public Image meterBkg;
    public Sprite redMeterSprite;
    public Sprite greenMeterSprite;
    public Text meterText;
    public float secondsAlotted = 10f; 

    private static CountDownTimer _instance;
    public static CountDownTimer Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private float timeLeft;
    private bool flickerStarted = false;
    public Tween countDownTween; 
    // Start is called before the first frame update
    void Start()
    {
        meterText.text = Mathf.Floor(secondsAlotted / 60).ToString("00") + ":" + (secondsAlotted % 60).ToString("00");

        countDownTween = DOTween.To(() => meter.fillAmount, x => meter.fillAmount = x, 0f, secondsAlotted).SetEase(Ease.Linear);
        countDownTween.OnUpdate(()=> {
            timeLeft = secondsAlotted - countDownTween.position; 
            meterText.text = Mathf.Floor(timeLeft / 60).ToString("00") + ":" + (timeLeft % 60).ToString("00");
            if(timeLeft < 30f && flickerStarted == false)
            {
                //meter.DOColor(new Color(0f,0f,0f,0f), 10f).SetEase(Ease.Flash, 50, -1f);
                meter.sprite = redMeterSprite; 
                meterBkg.DOColor(new Color(0f, 0f, 0f, 0f), 30f).SetEase(Ease.Flash, 150, -1f);
                flickerStarted = true; 
            }
        });
        countDownTween.OnComplete(()=> {
            Debug.Log("END LIBERATION");
            GameManager2.instance.CalculateScore_Liberation(); 
        });
       
    }


}

