using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;

public class Box : MonoBehaviour
{
    public delegate void BoxEmpty(GameObject boxGameObject);
    public static event BoxEmpty OnBoxEmpty;
    public Sprite crackedSprite, brokenSprite;

    public GameObject boxOnTop; 
    public BoxHealthState boxHealthState;
    public BoxOccupiedState boxOccupiedState;
    public BoxOrigin boxOrigin = BoxOrigin.Instantiate; 
    public float HEALTH = 10f;
    private float BOX_Y = -11.4369f;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        boxHealthState = BoxHealthState.Healthy;
        boxOccupiedState = BoxOccupiedState.Unoccupied;

        if (boxOrigin == BoxOrigin.Instantiate)
        {
            if (BoxButton.Instance.obstaclesInColumn.Count > 0)
            {
                //Debug.Log("obstacles count greater than 0!");
                Box boxObstacle = BoxButton.Instance.obstaclesInColumn[0].GetComponent<Box>();
                if (boxObstacle != null)
                {
                    Debug.Log("setting height of fall");
                    BOX_Y += BoxButton.Instance.obstaclesInColumn[0].GetComponent<SpriteRenderer>().bounds.size.y;
                }
            }

            Tween boxFallTween = transform.DOLocalMoveY(BOX_Y, 25f).SetSpeedBased().SetEase(Ease.InExpo);
            yield return boxFallTween.WaitForPosition(0.8f);
            SoundManager.instance.PlaySingle(SoundManager.instance.crash);
            yield return boxFallTween.WaitForCompletion();
            //OnComplete(()=>
            //{
            if (BoxButton.Instance.obstaclesInColumn.Count > 0)
            {
                //Debug.Log("obstacles count greater than 0!");
                Box boxObstacle = BoxButton.Instance.obstaclesInColumn[0].GetComponent<Box>();
                if (boxObstacle != null)
                {
                    Debug.Log("setting boxOnTop variable");
                    boxObstacle.boxOnTop = gameObject;
                }
            }

            if (boxOccupiedState == BoxOccupiedState.Unoccupied)
            {
                //only broadcast event if first box on ground
                if (BOX_Y > - 11.5f && BOX_Y < -11f)
                {
                    OnBoxEmpty?.Invoke(gameObject);
                }
            }
        }

        StartCoroutine(CountDown());
        //});
        GridWorld.RegisterObstacle(transform, false);
        //Debug.Log("box LOC: " + GridWorld.GetSquare(transform.position)[0] + " : " + GridWorld.GetSquare(transform.position)[1]);
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.6f * HEALTH);
        //cracked box sprite
        if(crackedSprite != null)
        {
            mySpriteRenderer.sprite = crackedSprite;
            SoundManager.instance.PlaySingle(SoundManager.instance.smallCrack);
        }
        boxHealthState = BoxHealthState.Cracked;
        yield return new WaitForSeconds(0.4f * HEALTH);
        //broken box sprite

        if (crackedSprite != null)
        {
            GameObject dust = Instantiate(Resources.Load("Prefabs/dust", typeof(GameObject)), new Vector3(transform.position.x, transform.position.y - 3.35f, 0f), Quaternion.identity, transform) as GameObject;
            dust.GetComponent<ParticleSystem>().Play();
            SoundManager.instance.PlaySingle(SoundManager.instance.bigBreak);
        }
        boxHealthState = BoxHealthState.Broken;
        yield return new WaitForSeconds(0.75f);
        if (crackedSprite != null)
        {
            mySpriteRenderer.sprite = brokenSprite;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y-3.35f, transform.localPosition.z);
        }
        
        yield return new WaitForSeconds(2f);
        mySpriteRenderer.DOFade(0f, 1f).OnComplete(()=> {
            Destroy(gameObject);
        });
        
        
        //fade out
        //destroy

    }

}
