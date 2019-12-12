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
    public float BOX_Y = -11.4369f;
    private float FALL_SPEED = 25f;
    private SpriteRenderer mySpriteRenderer;
    private List<GameObject> obstaclesInColumn;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        boxHealthState = BoxHealthState.Healthy;
        boxOccupiedState = BoxOccupiedState.Unoccupied;
        float heightToFallTo = BOX_Y;

        if (boxOrigin == BoxOrigin.Instantiate)
        {
            //Debug.Log("instantiated boxes");
            obstaclesInColumn = GridWorld.GetColumnObjects(transform.position);
            if (obstaclesInColumn.Count > 0)
            {
                //Debug.Log("obstacles count greater than 0! " + obstaclesInColumn[0].transform.name);
                Box boxObstacle = obstaclesInColumn[0].GetComponent<Box>();
                if (boxObstacle != null)
                {

                    heightToFallTo = BOX_Y + obstaclesInColumn[0].GetComponent<SpriteRenderer>().bounds.size.y;
                    //Debug.Log("!setting height of fall " + BOX_Y);
                }
            }

            Tween boxFallTween = transform.DOLocalMoveY(heightToFallTo, FALL_SPEED).SetSpeedBased().SetEase(Ease.InExpo);
            //yield return boxFallTween.WaitForPosition(0.8f);
            
            yield return boxFallTween.WaitForCompletion();
            SoundManager.instance.PlaySingle(SoundManager.instance.crash);
            //OnComplete(()=>
            //{
            if (obstaclesInColumn.Count > 0)
            {
                //Debug.Log("obstacles count greater than 0!");
                Box boxObstacle = obstaclesInColumn[0].GetComponent<Box>();
                if (boxObstacle != null)
                {
                    Debug.Log("setting boxOnTop variable");
                    boxObstacle.boxOnTop = gameObject;
                }
            }

            if (boxOccupiedState == BoxOccupiedState.Unoccupied)
            {
                //only broadcast event if first box on ground
                if (heightToFallTo > - 11.5f && heightToFallTo < -11f)
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


    private Tween fallDownTween;
    //fall from top of one box to the floor
    public void DropDown()
    {
        if (fallDownTween == null)
        {
            //remove from grid
            Debug.Log("remove from: " + transform.position);
            //Debug.DrawLine(transform.position, new Vector3(0f, 5f, 0f), Color.white, 2.5f);
            GridWorld.RegisterObstacle(transform, true);
            fallDownTween = transform.DOLocalMoveY(BOX_Y, FALL_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(.2f);
            

            fallDownTween.OnComplete(() =>
            {
                //add to grid
                //Debug.Log("add to: " + transform.position);
                //Debug.DrawLine(transform.position, new Vector3(0f, 5f, 0f), Color.white, 2.5f);
                GridWorld.RegisterObstacle(transform, false);
                SoundManager.instance.PlaySingle(SoundManager.instance.smallCrack);
                fallDownTween.Kill();
                fallDownTween = null;
            });
        }

    }

    private bool deRegisterObstacle = false; 
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.6f * HEALTH);
            deRegisterObstacle = false; 
            //cracked box sprite
            if(crackedSprite != null)
            {
                mySpriteRenderer.sprite = crackedSprite;
                SoundManager.instance.PlaySingle(SoundManager.instance.smallCrack);
            }
            boxHealthState = BoxHealthState.Cracked;
        yield return new WaitForSeconds(0.4f * HEALTH);
        //broken box sprite
            if (deRegisterObstacle == false)
            {
                deRegisterObstacle = true; 
                GridWorld.RegisterObstacle(transform, true);

                if (boxOnTop != null)
                {
                    boxOnTop.GetComponent<Box>().DropDown();
                }
                if (crackedSprite != null)
                {
                    GameObject dust = Instantiate(Resources.Load("Prefabs/dust", typeof(GameObject)), new Vector3(transform.position.x, transform.position.y - 3.35f, 0f), Quaternion.identity, transform) as GameObject;
                    dust.GetComponent<ParticleSystem>().Play();
                    SoundManager.instance.PlaySingle(SoundManager.instance.bigBreak);
                }
                boxHealthState = BoxHealthState.Broken;
            }
        yield return new WaitForSeconds(0.75f);
            if (crackedSprite != null && mySpriteRenderer.sprite != brokenSprite)
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
