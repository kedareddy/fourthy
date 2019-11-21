using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            SoundManager.instance.PlaySingle(SoundManager.instance.take);
            //Debug.Log("it is interactive");
            //boxToggle.isOn = true;
            if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == "Equality")
            {
                if (GameManager2.instance.clickTutorialUI.activeInHierarchy)
                {
                    GameManager2.instance.clickTutorialUI.SetActive(false);
                    StartCoroutine(GameManager2.instance.BoxPlacementTutorial());
                }
            }
        }
        else
        {
            SoundManager.instance.PlaySingle(SoundManager.instance.error);
        }
    }


    private Vector3 mouseDownPos; 
    // Update is called once per frame
    void Update()
    {
        //drop box into the world
        if (boxToggle.IsInteractable() && boxToggle.isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the mouse was clicked over a UI element
                if (!EventSystem.current.IsPointerOverGameObject())
                {

                    mouseDownPos = Input.mousePosition;

                }
            }


            if (Input.GetMouseButtonUp(0))
            {
                if(mouseDownPos == Input.mousePosition)
                {
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 worldTopPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
                    Vector3 gridWorldPos = GridWorld.GetGridPointCenter(worldPos);


                    SoundManager.instance.PlaySingle(SoundManager.instance.drop);
                    // Debug.Log("Dropped a box");


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
                }

            }

        }

    }
}

