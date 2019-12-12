using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Credits : MonoBehaviour
{
    private Vector3 initPos = new Vector3(1.599998f, -4245f, 0f);
    // Start is called before the first frame update
    void OnEnable()
    {
    
            transform.DOLocalMove(new Vector3(1.599998f, 350f, 0f), 50f).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() =>
            {
                GameManager2.instance.HandleCreditsFinished();
                transform.localPosition = initPos;
            });
        
    }
}
