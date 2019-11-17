using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using MyTools.MyExtensions;
using DG.Tweening;
using MonsterLove.StateMachine;

public class Character : MonoBehaviour
{

    public Vector3 startPoint, offscreenPos;
    public float WP_WAIT_TIME = 1f;
    public float WALK_SPEED = 20f;
    public float JUMP_SPEED = 30f;
    public float RESOLVE = 30f;
    public float HEART = 30f;
    public float HEART_STARTPOINT = 0f;
    public GameObject meter, resolveBar, heartBar;


    public enum HeightType
    {
        Tall,
        Middle,
        Short,
    }
    public HeightType myHeightType;

    public enum States
    {
        Watching,
        Waiting,
        GaveUp,
        Offscreen,
        Restart,
    }
    public States startingState;
    public StateMachine<States> fsm;

    public List<Box> myBoxes = new List<Box>();
    private Tween myPathTween, myBoxWalkTween;
    private Vector3 boxPosition;
    private List<Vector3> myPathPoints = new List<Vector3>();
    private int currentPos;
    private string prevDir = "right";
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    public Tween resolveBarTween, heartBarTween;
    private Vector3[] drawPoints;
    private Vector3 originalPos; 
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        meter.SetActive(true);
        fsm = StateMachine<States>.Initialize(this);
        Tween myVisualPathTween = GetComponent<DOTweenPath>().GetTween();
        drawPoints = myVisualPathTween.PathGetDrawPoints();
     

    }

    private void OnEnable()
    {
        StartCoroutine(AssignState());
        Box.OnBoxEmpty += HandleEmptyBox;
    }

    IEnumerator AssignState()
    {
        yield return new WaitForSeconds(0.25f);
        fsm.ChangeState(startingState);
    }

    private void OnDisable()
    {
        Box.OnBoxEmpty -= HandleEmptyBox;
    }


    public void HandleEmptyBox(GameObject box = null)
    {
        //Debug.Log("Handle Empty Box");
        if (fsm.CurrentStateMap.state != null)
        {
            if (fsm.CurrentStateMap.state.ToString() == States.Waiting.ToString() || fsm.CurrentStateMap.state.ToString() == States.Offscreen.ToString())
            {
                if (!(myHeightType == HeightType.Short && mySpriteRenderer.sortingOrder == 2))
                {
                    myPathTween.Kill();
                    myPathTween = null;
                    boxPosition = box.transform.position;
                    myBoxWalkTween = transform.DOLocalMoveX(box.transform.position.x, WALK_SPEED).SetSpeedBased().SetEase(Ease.Linear);
                    myBoxWalkTween.OnComplete(() =>
                    {
                        Box targetBox = box.GetComponent<Box>();
                    //Debug.Log("box is already occupied");
                    if (targetBox.boxOccupiedState == BoxOccupiedState.Unoccupied)
                        {
                            Debug.Log("added box to character");
                            myBoxes.Add(targetBox);
                            targetBox.boxOccupiedState = BoxOccupiedState.Occupied;

                            Vector3 firstPoint = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                            Vector3 secondPoint = new Vector3(transform.localPosition.x, transform.localPosition.y + 6.5f + 1f, transform.localPosition.z);
                            Vector3 thirdPoint = new Vector3(transform.localPosition.x, transform.localPosition.y + 6.5f, transform.localPosition.z);

                            SoundManager.instance.PlaySingle(SoundManager.instance.grunt);

                            transform.DOLocalPath(new Vector3[] { firstPoint, secondPoint, thirdPoint }, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InOutQuad).OnComplete(() =>
                            {
                                if (myHeightType == HeightType.Short)
                                {
                                //fsm.ChangeState(States.WaitingOnBox);
                                mySpriteRenderer.sortingOrder = 2;
                                }
                                else
                                {
                                    fsm.ChangeState(States.Watching);
                                }
                            });
                        }
                        else
                        {
                            fsm.ChangeState(States.Restart);
                        }
                    });
                }
            }
        }
    }

    IEnumerator WalkIntoScene()
    {
        prevDir = "right";
        currentPos = 0;
        Tween initWalkTween = transform.DOLocalMove(startPoint, WALK_SPEED).SetEase(Ease.Linear).SetSpeedBased(true);
        myAnimator.SetBool("isMoving", true);
        yield return initWalkTween.WaitForCompletion();
        myAnimator.SetBool("isMoving", false);
        yield return new WaitForSeconds(WP_WAIT_TIME);
        //Debug.Log("how many draw points: " + drawPoints.Length);
        myPathPoints.Add(transform.localPosition);
        myPathPoints.AddRange(drawPoints);
        myPathPoints.RemoveAt(1);
        //myPathPoints = new Vector3[] { transform.localPosition, drawPoints[2] };
        myPathTween = transform.DOLocalPath(myPathPoints.ToArray(), WALK_SPEED).SetSpeedBased(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        yield return myPathTween.WaitForPosition(0.1f);
        fsm.ChangeState(States.Waiting);
        myPathTween.OnWaypointChange((int index) =>
        {
            if (myPathTween.IsPlaying())
            {
                currentPos = index;
                myPathTween.Pause();
                StartCoroutine(AtWayPoint(index));
            }
        });
    }

    IEnumerator AtWayPoint(int index)
    {
        yield return new WaitForSeconds(WP_WAIT_TIME);
        myPathTween.Play();
    }

    public IEnumerator Waiting_Enter()
    {
        Debug.Log("Welcome to Waiting!!!!!!!!!");
        mySpriteRenderer.sortingOrder = 3; 
        meter.SetActive(true);
        resolveBar.SetActive(true);
        heartBar.SetActive(false);
        myAnimator.SetBool("isMoving", false);
        myAnimator.SetBool("isCheering", false);
        if (fsm.LastState != States.Restart)
        {
            resolveBar.transform.localPosition = Vector3.zero;
        }
        //subscribe to box occupancy opened event
        resolveBarTween = resolveBar.transform.DOLocalMoveX(-1.35f, RESOLVE).OnComplete(() =>
        {
            fsm.ChangeState(States.GaveUp);
        });

        if(myPathTween == null)
        {
            currentPos = 0;
            myPathPoints.Clear();
            myPathPoints.Add(transform.localPosition);
            myPathPoints.AddRange(drawPoints);
            myPathPoints.RemoveAt(1);
            myPathTween = transform.DOLocalPath(myPathPoints.ToArray(), WALK_SPEED).SetDelay(WP_WAIT_TIME).SetSpeedBased(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            yield return myPathTween.WaitForStart();
            
            myPathTween.OnWaypointChange((int index) =>
            {
                if (myPathTween.IsPlaying())
                {
                    currentPos = index;
                    myPathTween.Pause();
                    StartCoroutine(AtWayPoint(index));
                }
            });
        }
    }

    private string movingPath = "none";
    private Tween jumpUpAgainTween;
    public void Waiting_Update()
    {
        movingPath = "none";
        if (myPathTween != null) {
            if(myPathTween.IsActive() && myPathTween.IsPlaying())
            {
                movingPath = "path";
            }
        }

        if(myBoxWalkTween != null)
        {
            if(myBoxWalkTween.IsActive() && myBoxWalkTween.IsPlaying())
            {
                movingPath = "box";
            }
        }

        if(movingPath != "none")
        {
            //Debug.Log(transform.name + " is Moving");
            myAnimator.SetBool("isMoving", true);

            float direction;
            //orienting when patrolling points or going to box
            if (movingPath == "path")
            {
                direction = myPathPoints[currentPos].x - transform.localPosition.x;
            }
            else
            {
                direction = -boxPosition.x + transform.localPosition.x;
            }


            if (direction > 0f)
            {
                //Debug.Log("Left");
                if (prevDir != "left")
                {
                    transform.Rotate(0, 180f, 0);
                    prevDir = "left";
                }
            }
            else
            {
                //Debug.Log("Right");
                if (prevDir != "right")
                {
                    transform.Rotate(0, 180f, 0);
                    prevDir = "right";
                }
            }
        }
        else
        {
            myAnimator.SetBool("isMoving", false);
        }

        if (myHeightType == HeightType.Short)
        {
            if (myBoxes.Count > 0)
            {
                //Debug.Log("should jump up again 111: " + myBoxes[0].boxOnTop + " :");
                if (myBoxes[0].boxOnTop != null)
                {
                    //Debug.Log("should jump up again 222");
                    if (jumpUpAgainTween == null)
                    {
                        //Debug.Log("should jump up again 333");
                        jumpUpAgainTween = transform.DOLocalMoveY(-1.8f, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f);

                        jumpUpAgainTween.OnComplete(() => {
                            myBoxes.Add(myBoxes[0].boxOnTop.GetComponent<Box>());
                            jumpUpAgainTween.Kill();
                            SoundManager.instance.PlaySingle(SoundManager.instance.grunt);
                            fsm.ChangeState(States.Watching);
                        });
                    }
                }
            }
        }

    }

    public void Waiting_Exit()
    {
        resolveBarTween.Kill();
    }

    private Tween meterTween;
    public void Watching_Enter()
    {
        mySpriteRenderer.sortingOrder = 2;
        
        Debug.Log("Watching game!");

        //if (myHeightType != HeightType.Short || (myHeightType == HeightType.Short && myBoxes.Count > 1 && transform.localPosition.y > -2f))
        //{
            meter.SetActive(true);
            resolveBar.SetActive(true);
            resolveBar.transform.localPosition = Vector3.zero;
            heartBar.SetActive(true);
            heartBar.transform.localPosition = new Vector3(-0.114f, 0.058f, 0f); 
            heartBarTween = heartBar.transform.DOLocalMoveY(-0.22f, HEART).From().SetLoops(-1, LoopType.Restart);
            heartBarTween.Goto(HEART_STARTPOINT, true);

            heartBarTween.OnStepComplete(() =>
            {
                //Debug.Log("should release heart");
                meterTween = meter.transform.DOScale(1.2f, 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InQuad);
                meterTween.OnComplete(() =>
                {
                    //if(meterTween.CompletedLoops() == 1)
                    //{
                        Instantiate(Resources.Load("Prefabs/heart", typeof(GameObject)), new Vector3(meter.transform.position.x, meter.transform.position.y, 0f), Quaternion.identity, GameManager2.instance.GetCurrentRuntimeObjsParent());
                    //} 
                });
            });

            myAnimator.SetBool("isCheering", true);
       // }


        if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
        {
            if (transform == GameManager2.instance.equality_ShortC.transform)
            {
                GameManager2.instance.Equality_Success();
                meter.SetActive(false);
            }
        }
        
        
    }

    private Tween jumpDownTween;
    public void Watching_Update()
    {
        if (myBoxes.Count > 0)
        {
            if (myBoxes[0].boxHealthState == BoxHealthState.Broken)
            {
                if (jumpDownTween == null)
                {
                    jumpDownTween = transform.DOLocalMoveY(-15.1f, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f);
                    
                    jumpDownTween.OnComplete(()=>{
                        fsm.ChangeState(States.Waiting);
                        jumpDownTween.Kill();
                        //SoundManager.instance.PlaySingle(SoundManager.instance.grunt);
                    });
                }
            }
        }

        
    }
    public void Watching_Exit()
    {
        heartBarTween.Kill();

    }


    public void GaveUp_Enter()
    {
        mySpriteRenderer.sortingOrder = 4;
        meter.SetActive(false);
        transform.DOKill();
        myAnimator.SetBool("isSitting", true);
        transform.localPosition = new Vector3(transform.localPosition.x, originalPos.y, transform.localPosition.z); 
        //talk to people if there are more than 2 people next to each other
        if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString() || GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equity.ToString())
        {
            Debug.Log("should show fail SCREEN");
            StartCoroutine(GameManager2.instance.Failed());
        }

    }
    public void GaveUp_Update()
    {

    }
    public void GaveUp_Exit()
    {

    }



    public void Offscreen_Enter()
    {
        meter.SetActive(true);
        resolveBar.SetActive(true);
        heartBar.SetActive(false);
        resolveBar.transform.localPosition = Vector3.zero;
        StartCoroutine(WalkIntoScene());
    }
    public void Offscreen_Update()
    {

    }
    public void Offscreen_Exit()
    {

    }

    public void Restart_Enter()
    {
        fsm.ChangeState(fsm.LastState);
    }

    public void Restart_Update()
    {

    }

    public void Restart_Exit()
    {

    }

    bool isEqual(float a, float b)
    {
        if (a >= b - Mathf.Epsilon && a <= b + Mathf.Epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
