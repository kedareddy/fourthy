using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BoxButton : MonoBehaviour
{
    public GameObject boxPrefab;
    private static BoxButton _instance;

    public static BoxButton Instance { get { return _instance; } }

    private Toggle boxToggle;

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
    // Start is called before the first frame update
    void OnEnable()
    {
        boxToggle = transform.GetComponent<Toggle>();
    }


    public void OnToggled()
    {
        //Debug.Log("clicke on box butotn");
        if (boxToggle.IsInteractable())
        {
            SoundManager.instance.PlaySingle(SoundManager.instance.take, 0.5f);
            //Debug.Log("it is interactive");
            //boxToggle.isOn = true;
            if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
            {
                if (GameManager2.instance.clickTutorialUI.activeInHierarchy)
                {
                    GameManager2.instance.clickTutorialUI.SetActive(false);
                    StartCoroutine(GameManager2.instance.BoxPlacementTutorial());
                }
            }

            if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equity.ToString() && GameManager2.instance.equity_ShortC.myBoxes.Count == 1 && HeartCounter.Instance.HeartsNumber == 3)
            {
                GameManager2.instance.textTutorialUI.SetActive(false);
                Tween stackTutorialTween = GameManager2.instance.wideTutorialUI.transform.DOLocalMoveX(GameManager2.instance.wideTutorialUI.transform.localPosition.x - 100f, 0.5f).From().SetEase(Ease.OutQuad);
                stackTutorialTween.OnPlay(() =>
                {
                    GameManager2.instance.wideTutorialUI.SetActive(true);
                    StartCoroutine(TurnOffStackTutorial());
                });
            }
        }
        else
        {
            SoundManager.instance.PlaySingle(SoundManager.instance.error);
        }
    }

    IEnumerator TurnOffStackTutorial()
    {
        yield return new WaitForSeconds(10f);
        GameManager2.instance.wideTutorialUI.SetActive(false);
    }


    private Vector3 mouseDownPos;
    private float timeOfLastDrop = 0f;
    // Update is called once per frame
    void Update()
    {
        //drop box into the world
        if (boxToggle.IsInteractable() && boxToggle.isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                // Check if the mouse was clicked over a UI element
                //if (!EventSystem.current.IsPointerOverGameObject())
                //{
                if(IsPointerOverUIElement() == false) { 
                    //Debug.Log("Dropped a box");
                    mouseDownPos = Input.mousePosition;

                }
            }


            if (Input.GetMouseButtonUp(0) )
            {
                //Debug.Log("time: " + (Time.time - timeOfLastDrop).ToString());
                //Vector3.SqrMagnitude(a - b) < 0.0001
                //if (mouseDownPos == Input.mousePosition && (Time.time - timeOfLastDrop) > 2f)
                if (Vector3.SqrMagnitude(mouseDownPos - Input.mousePosition) < 20f && (Time.time - timeOfLastDrop) > 2f)
                {
                    timeOfLastDrop = Time.time;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 worldTopPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
                    Vector3 gridWorldPos = GridWorld.GetGridPointCenter(worldPos);


                    //SoundManager.instance.PlaySingle(SoundManager.instance.drop, 0.2f);
                   
                    //trigger box stacking tutorial in Equity

                    GameObject instantiatedBox = Instantiate(boxPrefab, new Vector3(gridWorldPos.x, worldTopPos.y, boxPrefab.transform.position.z), boxPrefab.transform.rotation) as GameObject;
                    instantiatedBox.transform.SetParent(GameManager2.instance.GetCurrentRuntimeObjsParent());
                    HeartCounter.Instance.DecrementBoxHearts();

                    if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
                    {
                        if (GameManager2.instance.clickTutorialUI.activeInHierarchy)
                        {
                            GameManager2.instance.clickTutorialUI.SetActive(false);
                        }
                    }

                    //if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equity.ToString() && GameManager2.instance.equity_ShortC.myBoxes.Count == 1)
                    //{
                    //    if (GameManager2.instance.wideTutorialUI.activeInHierarchy)
                    //    {
                    //        GameManager2.instance.wideTutorialUI.SetActive(false);
                    //    }
                    //}
                }

            }

        }

    }



    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}

