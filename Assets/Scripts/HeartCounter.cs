using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartCounter : MonoBehaviour
{
    
    public Toggle boxToggle;
    private static HeartCounter _instance;

    public static HeartCounter Instance { get { return _instance; } }


    private Text myText;
    // Start is called before the first frame update

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

    void OnEnable()
    {
        myText = transform.GetChild(0).GetComponent<Text>();
        myText.text = HeartsNumber.ToString();
        if (HeartsNumber >= 3)
        {
            TurnOnBoxButton(); 
        }

    }

    public void TurnOnBoxButton()
    {
        boxToggle.transform.DOScale(0.8f, 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            boxToggle.interactable = true;
            boxToggle.isOn = false;
        });
    }

    [SerializeField]
    public int HeartsNumber
    {
        get { return heartsNumber; }
        set { heartsNumber = value; }
    }
    public int heartsNumber = 0; 

    
    public void IncrementHearts()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.taken, 0.15f);
        HeartsNumber = HeartsNumber + 1;
        transform.DOJump(transform.position, 2.5f, 1, 0.25f).OnComplete(() => {
            myText.text = HeartsNumber.ToString();
            if (HeartsNumber == 3)
            {
                TurnOnBoxButton();
            }
        });
    }



    public void DecrementBoxHearts()
    {
        HeartsNumber = HeartsNumber - 3;
        myText.text = HeartsNumber.ToString();
        if (heartsNumber < 3)
        {
            boxToggle.interactable = false;
            boxToggle.isOn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
