using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EqualityCharacter : MonoBehaviour
{

    public GameObject resolveMeter; 
    private Animator myAnimator;
    private float lastPosX = 0f; 
    // Start is called before the first frame update
    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myAnimator.SetTrigger("faceForward");
        lastPosX = transform.position.x;
    }

    void Start()
    {
        resolveMeter.transform.DOLocalMoveX(-1.35f, 30f).OnComplete(()=>
        {
            transform.DOMoveX(36f, 12f).SetSpeedBased().SetEase(Ease.Linear);
        });
    }

    // Update is called once per frame
    void Update()
    {

        //if negative x scale
        if (transform.localScale.x < 0)
        {
            //check if going left else multiplay by -1
            if ((transform.position.x - lastPosX) < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //resolveMeter.transform.localScale = new Vector3(-resolveMeter.transform.localScale.x, resolveMeter.transform.localScale.y, resolveMeter.transform.localScale.z); 
            }
        }
        else
        {
            //check if going left else multiplay by -1
            if ((transform.position.x - lastPosX) > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //resolveMeter.transform.localScale = new Vector3(-resolveMeter.transform.localScale.x, resolveMeter.transform.localScale.y, resolveMeter.transform.localScale.z);
            }
        }

        if (Mathf.Abs(transform.position.x - lastPosX) > 0.001f)
        {
            myAnimator.SetBool("isWalking", true);
            lastPosX = transform.position.x;
        }
        else
        {
            myAnimator.SetBool("isWalking", false);
        }
    }


    public void MoveToBox(GameObject myBox)
    {
        if (myBox is null)
        {
            throw new System.ArgumentNullException(nameof(myBox));
        }
        resolveMeter.transform.DOPause(); 
        transform.DOMoveX(myBox.transform.position.x, 12f).SetSpeedBased().SetEase(Ease.Linear).OnComplete(()=> {
            StartCoroutine(JumpOnBox(myBox));
        });
    }

    IEnumerator JumpOnBox(GameObject myBox)
    {
        myAnimator.SetTrigger("faceForward");
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().isKinematic = true; 
        transform.DOLocalMoveY(8f, 12f).SetSpeedBased().SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            myBox.GetComponent<BoxCollider2D>().isTrigger = false;
            myAnimator.SetTrigger("faceForward");
        });
    }
}
