using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class Bubble : MonoBehaviour
{
    public List<SpriteRenderer> availableSpriteRenderers;
    public List<Sprite> sadEmojis;
    public List<Sprite> happyEmojis;
    public Sprite thoughtSprite;
    public Sprite speakSprite;
    public Sprite rightSpeakSprite; 

    private Character myCharacter;
    private SpriteRenderer mySpriteRenderer;
    private float seatedHeight;
    private Vector3 orgLocalPos; 
    // Start is called before the first frame update
    void Awake()
    {
        myCharacter = transform.parent.GetComponent<Character>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        seatedHeight = transform.localPosition.y - (mySpriteRenderer.bounds.size.y * 0.4f);
        orgLocalPos = transform.localPosition; 
    }

    private void OnEnable()
    {
        //Debug.Log("BUBBLE ON!" + transform.name);
        if (myCharacter.fsm.CurrentStateMap.state.ToString() == Character.States.GaveUp.ToString())
        {
            //if(myCharacter.closestSitSite.sitPoints[myCharacter.sitPointIndex].yAngle > 0f)
            //{
            //    Debug.Log("BUBBLE ON!" + transform.parent.name);
            //    //mySpriteRenderer.sprite = rightSpeakSprite;
            //    mySpriteRenderer.flipX = false;
            //}
            //else
            //{
            //    //mySpriteRenderer.sprite = speakSprite;
            //    mySpriteRenderer.flipX = true;
            //}
            mySpriteRenderer.sprite = speakSprite;
            OpenBubble(true);
        }
        else
        {
            mySpriteRenderer.sprite = thoughtSprite;
            OpenBubble(false); 
        }

    }

    public void OpenBubble(bool r)
    {
        bool recursive = r;
        if(mySpriteRenderer.enabled == false)
        {
            mySpriteRenderer.enabled = true;
            //Debug.Log("should be called a few time"); 
        }
        if (recursive)
        {
            mySpriteRenderer.sortingLayerName = "GaveUp";
            transform.localPosition = new Vector3(transform.localPosition.x, seatedHeight, transform.localPosition.z);
            //Debug.Log("should set localpos to be half height");
        }
        else
        {
            transform.localPosition = orgLocalPos;  
        }
        transform.DOScale(0f, 20f).From().SetSpeedBased().SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            int numOfEmojis = Random.Range(3, 6);
            for (int i = 0; i < availableSpriteRenderers.Count; i++)
            {
                if (i < numOfEmojis)
                {
                    if (recursive)
                    {
                        availableSpriteRenderers[i].sortingLayerName = "GaveUp";
                    }
                    availableSpriteRenderers[i].sprite = sadEmojis[Random.Range(0, sadEmojis.Count)];
                    availableSpriteRenderers[i].transform.gameObject.SetActive(true);
                }
                else
                {
                    availableSpriteRenderers[i].transform.gameObject.SetActive(false);
                }
            }
            if (recursive)
            {
                myCharacter.closestSitSite.numConversations++;
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(CloseBubble());
                }
            }
        });
    }

    IEnumerator CloseBubble()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        mySpriteRenderer.enabled = false;
        for (int i = 0; i < availableSpriteRenderers.Count; i++)
        {
            availableSpriteRenderers[i].transform.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(Random.Range(1.5f, 6f));
        OpenBubble(true); 
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
