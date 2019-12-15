using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Audience : MonoBehaviour
{

    private Animator myAnimator;
    private Vector2 originalLocalPos; 
    // Start is called before the first frame update
    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        originalLocalPos = transform.localPosition;
        StartCoroutine(GetExcited());
    }

    private IEnumerator GetExcited()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f,6f));
            //transform.DOGoto(0f);
            transform.DOKill(true);
            //transform.localPosition = new Vector3(originalLocalPos.x, originalLocalPos.y, transform.localPosition.z);
            
            var dur = Random.Range(.1f, 0.7f);

            //Audienc in Intro scene1
            if (myAnimator != null)
            {
                float height = Random.Range(.01f, 0.2f);
                //Debug.Log("previous speed " + myAnimator.speed + " nem: " + myAnimator.runtimeAnimatorController.animationClips[1].name);
                transform.DOLocalMoveY(height, dur, false).SetLoops(Random.Range(1, 3) * 2, LoopType.Yoyo).SetEase(Ease.InQuad);
                //myAnimator.speed = (dur * 2f) * (myAnimator.speed / myAnimator.runtimeAnimatorController.animationClips[1].length);
                if(height > 0.1f)
                {
                    myAnimator.SetTrigger("isExcited");
                }
                else
                {
                    myAnimator.SetTrigger("isSuperExcited");
                }
                
            }
            else
            {
                //transform.DOLocalJump(transform.localPosition, 1.5f, Random.Range(1,5), .5f);
                transform.DOScaleY(Random.Range(1.55f, 1.6f), Random.Range(.1f, 1f)).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}
