using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Text quoteText, quoteByLineText; 
    private Vector3 initPos = new Vector3(1.599998f, -4245f, 0f);
    // Start is called before the first frame update
    void OnEnable()
    {

        Sequence scrollS = DOTween.Sequence();
        scrollS.Append(transform.DOLocalMove(new Vector3(1.599998f, 350f, 0f), 50f).SetSpeedBased().SetEase(Ease.Linear));
        scrollS.Append(quoteText.DOFade(1f, 2f));
        scrollS.Append(quoteByLineText.DOFade(1f, 2f));
        scrollS.AppendInterval(10f);
        scrollS.AppendCallback(() =>
        {
            GameManager2.instance.HandleCreditsFinished();
            transform.localPosition = initPos;
            quoteText.color = new Color(1f, 1f, 1f, 0f);
            quoteByLineText.color = new Color(1f, 1f, 1f, 0f);
        });
    }
}
