using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Heart : MonoBehaviour, IPointerDownHandler
{
    public float LIFE = 10f;
    public bool floatDown = true;
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer myBkgSpriteRenderer;
    private float FLOOR_Y = -16.9f;
    private bool pickedUp = false;

    private void OnEnable()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myBkgSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        pickedUp = false;

        if (floatDown)
        {    
            float randomX = Random.Range(transform.localPosition.x - 5f, transform.localPosition.x + 5f);
            Vector3 secondPoint = new Vector3(randomX, transform.localPosition.y + 0.5f, transform.localPosition.z);
            Vector3 thirdPoint = new Vector3(randomX, FLOOR_Y, transform.localPosition.z);
            Vector3[] points = { transform.localPosition, secondPoint, thirdPoint };
            Sequence heartS = DOTween.Sequence();
            heartS.Append(transform.DOLocalPath(points, 15f, PathType.CatmullRom).SetSpeedBased().SetEase(Ease.Linear));
            heartS.Join(transform.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-40f, 40f)), 15f).SetSpeedBased().SetEase(Ease.OutQuad));
            heartS.AppendInterval(10f);
            heartS.OnComplete(() =>
            {
                Destroy(gameObject);
            });
            SoundManager.instance.PlaySingle(SoundManager.instance.born);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pickedUp == false)
        {
            //Debug.Log("heart clicked");
            if (eventData.pointerCurrentRaycast.gameObject == gameObject)
            {
                    pickedUp = true;
                    if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
                    {
                        if (GameManager2.instance.clickTutorialUI.activeInHierarchy)
                        {
                            GameManager2.instance.clickTutorialUI.SetActive(false);
                        }
                    }

                   // Debug.Log("picked up a heart");
                    SoundManager.instance.PlaySingle(SoundManager.instance.gulp);
                    Sequence collectS = DOTween.Sequence();
                    collectS.Append(transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutQuad));
                    collectS.Append(transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.2f, Screen.height*0.9f, Camera.main.nearClipPlane)), 1f).SetSpeedBased().SetEase(Ease.OutQuad));
                    collectS.Join(mySpriteRenderer.DOFade(0f, 1f).SetSpeedBased().SetEase(Ease.OutQuad));
                    collectS.Join(myBkgSpriteRenderer.DOFade(0f, 1f).SetSpeedBased().SetEase(Ease.OutQuad));
                    collectS.Join(transform.DOScale(new Vector3(1f, 1f, 1f), 1f).SetSpeedBased().SetEase(Ease.InQuad));
                    collectS.AppendCallback(() =>
                    {
                        HeartCounter.Instance.IncrementHearts();
                    });

            }

        }
    }
    
}
