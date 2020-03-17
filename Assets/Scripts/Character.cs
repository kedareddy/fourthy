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
    public GameObject speechBubble;


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
        None,
    }
    public States startingState;
    public StateMachine<States> fsm;

    public List<Box> myBoxes = new List<Box>();
    private Tween myPathTween, myBoxWalkTween, myBoxJumpTween;
    private Vector3 boxPosition;
    private List<Vector3> myPathPoints = new List<Vector3>();
    private int currentPos;
    private string prevDir = "right";
    public Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    public Tween resolveBarTween, heartBarTween;
    private Vector3[] drawPoints;
    private Vector3 originalPos;
    private string sortingLayerName; 

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        sortingLayerName = mySpriteRenderer.sortingLayerName;
        meter.SetActive(true);
        fsm = StateMachine<States>.Initialize(this);
        Tween myVisualPathTween = GetComponent<DOTweenPath>().GetTween();
        drawPoints = myVisualPathTween.PathGetDrawPoints();


    }

    private void OnEnable()
    {
        StartCoroutine(AssignState());
        Box.OnBoxEmpty += HandleEmptyBox;
        if (!transform.name.Contains("CharC"))
        {
            JumpMeter.OnInitiateJump += HandleJump;
        }

    }

    IEnumerator AssignState()
    {
        yield return new WaitForSeconds(0.25f);
        fsm.ChangeState(States.None);
        fsm.ChangeState(startingState);
    }

    private void OnDisable()
    {
        Box.OnBoxEmpty -= HandleEmptyBox;
        if (!transform.name.Contains("CharC"))
        {
            JumpMeter.OnInitiateJump -= HandleJump;
        }
    }


    public void HandleEmptyBox(GameObject box = null)
    {
        Box targetBox = box.GetComponent<Box>();
        Debug.Log("Handle Empty Box" + targetBox.boxOccupiedState);
        //&& !transform.name.Contains("CharC")
        if (fsm.CurrentStateMap.state != null )
        {
            if (fsm.CurrentStateMap.state.ToString() == States.Waiting.ToString() || fsm.CurrentStateMap.state.ToString() == States.Offscreen.ToString())
            {
                if (!(myHeightType == HeightType.Short && myBoxes.Count > 0) && myBoxWalkTween == null && myBoxJumpTween == null)
                {
                    //Debug.Log("walk to empty box");
                    myPathTween.Kill();
                    myPathTween = null;
                    boxPosition = box.transform.position;
                    myBoxWalkTween = transform.DOLocalMoveX(box.transform.position.x, WALK_SPEED).SetSpeedBased().SetEase(Ease.Linear);
                    myBoxWalkTween.OnComplete(() =>
                    {
                        myBoxWalkTween.Kill();
                        myBoxWalkTween = null;
                        if (box != null)
                        {
                           // Box targetBox = box.GetComponent<Box>();
                            //Debug.Log("box is already occupied");
                            if (targetBox.boxOccupiedState == BoxOccupiedState.Unoccupied)
                            {
                                //char C tries to jump but can't get on box
                                if (transform.name.Contains("CharC"))
                                {
                                    transform.DOLocalMoveY(transform.localPosition.y + 2f, JUMP_SPEED/3f).SetSpeedBased().SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad).OnComplete(()=> {
                                        resolveBarTween.Pause();
                                        myAnimator.SetBool("isMoving", false);
                                        speechBubble.SetActive(true);
                                        fsm.ChangeState(States.Restart);
                                    });
                                }
                                else
                                {
                                    // Debug.Log("added box to character");
                                    myBoxes.Add(targetBox);
                                    targetBox.boxOccupiedState = BoxOccupiedState.Occupied;

                                    Vector3 firstPoint = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                                    Vector3 secondPoint = new Vector3(transform.localPosition.x, transform.localPosition.y + 6.5f + 1f, transform.localPosition.z);
                                    Vector3 thirdPoint = new Vector3(transform.localPosition.x, transform.localPosition.y + 6.5f, transform.localPosition.z);

                                    /////SoundManager.instance.PlaySingle(SoundManager.instance.grunt, 0.5f);

                                    myBoxJumpTween = transform.DOLocalPath(new Vector3[] { firstPoint, secondPoint, thirdPoint }, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InOutQuad);
                                    myBoxJumpTween.OnComplete(() =>
                                    {
                                        myBoxJumpTween = null;
                                        if (myHeightType == HeightType.Short)
                                        {
                                        //fsm.ChangeState(States.WaitingOnBox);
                                        mySpriteRenderer.sortingLayerName = "Watching";
                                            mySpriteRenderer.sortingOrder = 2;
                                        }
                                        else
                                        {
                                            resolveBarTween.Kill();
                                            fsm.ChangeState(States.Watching);
                                        }

                                    //For Equality success
                                    if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
                                        {
                                            if (transform == GameManager2.instance.equality_ShortC.transform)
                                            {
                                                GameManager2.instance.Equality_Success();
                                                meter.SetActive(false);
                                            }
                                        }

                                    //Tutorial in Equity
                                    if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equity.ToString())
                                        {
                                            Vector2 localPoint;
                                            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager2.instance.textTutorialUI.transform.parent as RectTransform, new Vector2(Screen.width * .5f, Screen.height * 0.1f), GameManager2.instance.UICamera, out localPoint);
                                            GameManager2.instance.textTutorialUI.transform.localPosition = localPoint;


                                            Tween showTextTutorial = GameManager2.instance.textTutorialUI.transform.DOLocalMoveY(GameManager2.instance.textTutorialUI.transform.localPosition.y - 100f, 0.25f).From().SetEase(Ease.OutQuad).SetDelay(0.5f);
                                            showTextTutorial.OnPlay(() =>
                                            {
                                                GameManager2.instance.textTutorialUI.SetActive(true);
                                            });
                                        }
                                    });
                                }
                            }
                            else
                            {
                                resolveBarTween.Pause();
                                myAnimator.SetBool("isMoving", false);
                                fsm.ChangeState(States.Restart);
                            }
                        }
                    });

                    myBoxWalkTween.OnUpdate(() => {
                        if(box != null)
                        {
                            if (targetBox.boxOccupiedState == BoxOccupiedState.Occupied)
                            {
                                myBoxWalkTween.Kill();
                                myBoxWalkTween = null;
                                resolveBarTween.Pause();
                                myAnimator.SetBool("isMoving", false);
                                fsm.ChangeState(States.Restart);
                            }
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
        if(myAnimator.GetBool("isMoving") == false)
        {
            myAnimator.SetBool("isMoving", true);
        }
        yield return initWalkTween.WaitForCompletion();
        if (myAnimator.GetBool("isMoving") == true)
        {
            myAnimator.SetBool("isMoving", false);
        }
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

    private bool checkSpeechBubleOnce = false;
    IEnumerator AtWayPoint(int index)
    {
        
        if (checkSpeechBubleOnce == false && speechBubble != null)
        {
            mySpriteRenderer.sortingLayerName = "Watching";
            checkSpeechBubleOnce = true; 
            int randInt = Random.Range(0, 11);
            if (randInt > 5)
            {
                if (speechBubble.activeInHierarchy == false)
                {
                    //Debug.Log("set active333333");
                    speechBubble.SetActive(true);
                }
            }
        }
        yield return new WaitForSeconds(WP_WAIT_TIME);
        checkSpeechBubleOnce = false;
        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }
        myPathTween.Play();
        mySpriteRenderer.sortingLayerName = sortingLayerName;
    }

    private bool resolveFlickerStarted = false; 
    public IEnumerator Waiting_Enter()
    {
        //Debug.Log("Welcome to Waiting!!!!!!!!!");
        
        meter.SetActive(true);
        resolveBar.SetActive(true);
        heartBar.SetActive(false);
        myAnimator.SetBool("isMoving", false);
        myAnimator.SetBool("isCheering", false);

        bool setupResolveBar = false; 
        if(fsm.LastState == States.None)
        {
            setupResolveBar = true;

        }
        else
        {
            if (fsm.LastState != States.Restart)
            {
                setupResolveBar = true;
            }
            else
            {
                setupResolveBar = false;
            }
        }
        if (setupResolveBar)
        {
            //Debug.Log("Resolve bar should flash");
            //resolveBar.transform.localPosition = Vector3.zero;
            resolveBar.transform.localPosition = new Vector3(-0.77f, 0.07f, 0f);
            resolveBar.transform.SetScaleX(1f);
            resolveBarTween = resolveBar.transform.DOScaleX(0f, RESOLVE);
            //resolveBarTween = resolveBar.transform.DOLocalMoveX(-1.35f, RESOLVE);
            resolveBarTween.OnUpdate(() => {
                //float resolveTweenLeft = Mathf.Abs(-1.35f - resolveBarTween.position);
                //Debug.Log("resolv: " + resolveBarTween.ElapsedPercentage());
                if (resolveBarTween.ElapsedPercentage() > 0.6f && resolveFlickerStarted == false)
                {
                    meter.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 0f, 0f, 0f), 20f).SetEase(Ease.Flash, 100, -1f);
                    resolveFlickerStarted = true;
                }
            });
            resolveBarTween.OnComplete(() =>
            {
                meter.SetActive(false);
                resolveBarTween.Kill();
                fsm.ChangeState(States.GaveUp);
            });
        }
        else
        {
            resolveBarTween.Play();
        }


        //check if short character is waiting on box, in 
        if (myBoxes.Count == 0)
        {

            mySpriteRenderer.sortingLayerName = sortingLayerName;
            mySpriteRenderer.sortingOrder = 3;

            if (myPathTween == null)
            {
                currentPos = 0;
                myPathPoints.Clear();
                myPathPoints.Add(transform.localPosition);
                myPathPoints.AddRange(drawPoints);
                myPathPoints.RemoveAt(1);
                myPathTween = transform.DOLocalPath(myPathPoints.ToArray(), WALK_SPEED).SetSpeedBased(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                //.SetDelay(WP_WAIT_TIME)
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

            //check for previously dropped, free boxes. first make sure character is on the ground
            if (myBoxes.Count == 0 && !transform.name.Contains("CharC"))
            {
                foreach (Transform child in GameManager2.instance.GetCurrentRuntimeObjsParent())
                {
                    Box b = child.GetComponent<Box>();
                    if (b != null)
                    {
                        if (b.boxOccupiedState == BoxOccupiedState.Unoccupied && b.transform.localPosition.y < (b.BOX_Y + 1f))
                        {
                            //Debug.Log("WTF??? I shouldn't get on the box");
                            HandleEmptyBox(child.gameObject);
                        }

                    }
                }
            }

        }
        else
        {
            //short chracter
            mySpriteRenderer.sortingLayerName = "Watching";
            mySpriteRenderer.sortingOrder = 3;
        }
    }

    private string movingPath = "none";
    private Tween jumpUpAgainTween, fallDownTween;
    public void Waiting_Update()
    {
        movingPath = "none";
        if (myPathTween != null)
        {
            if (myPathTween.IsActive() && myPathTween.IsPlaying())
            {
                //Debug.Log("path is playiing apparently");
                movingPath = "path";
            }
        }

        if (myBoxWalkTween != null)
        {
            if (myBoxWalkTween.IsActive() && myBoxWalkTween.IsPlaying())
            {
                movingPath = "box";
            }
        }

        if (movingPath != "none")
        {
            //Debug.Log("the animator state is: " + myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.chrD_moving") + myAnimator.GetCurrentAnimatorStateInfo(0).IsName("chrD_moving"));
            //if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.chrD_moving") == false)
                //{
            if (myAnimator.GetBool("isMoving") == false)
            {
                //Debug.Log("setting WALKING");
                myAnimator.SetBool("isMoving", true);
            }
            

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
                //check to see if should jump up
                if (myBoxes[0].boxOnTop != null)
                {
                    //Debug.Log("should jump up again 222");
                    if (jumpUpAgainTween == null)
                    {
                        // Debug.Log("should jump up again 333");
                        jumpUpAgainTween = transform.DOLocalMoveY(-1.8f, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f);

                        jumpUpAgainTween.OnComplete(() =>
                        {
                            Box topBox = myBoxes[0].boxOnTop.GetComponent<Box>();
                            topBox.boxOccupiedState = BoxOccupiedState.Occupied;
                            myBoxes.Add(topBox);
                            jumpUpAgainTween.Kill();
                            jumpUpAgainTween = null;
                            /////SoundManager.instance.PlaySingle(SoundManager.instance.grunt, 0.5f);
                            resolveBarTween.Kill();
                            fsm.ChangeState(States.Watching);
                        });
                    }
                }
                //check to see if should fall down
                if (myBoxes[0].boxHealthState == BoxHealthState.Broken)
                {
                    if (fallDownTween == null)
                    {
                        // Debug.Log("jump to ground");
                        fallDownTween = transform.DOLocalMoveY(-15.1f, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f);

                        fallDownTween.OnComplete(() =>
                        {
                            mySpriteRenderer.sortingLayerName = sortingLayerName;
                            myBoxes.RemoveAt(0);
                            resolveBarTween.Pause();
                            fallDownTween.Kill();
                            fallDownTween = null;
                            /////SoundManager.instance.PlaySingle(SoundManager.instance.grunt, 0.5f);
                            fsm.ChangeState(States.Restart);
                        });
                    }
                }
            }
        }

    }

    public void Waiting_Exit()
    {
        resolveFlickerStarted = false; 
        myAnimator.SetBool("isMoving", false);
        if (speechBubble != null)
        {
            if (speechBubble.activeInHierarchy)
            {
                speechBubble.SetActive(false);
            }
        }

        if (GameManager2.instance.textTutorialUI.activeInHierarchy)
        {
            GameManager2.instance.textTutorialUI.SetActive(false);
        }
        //resolveBarTween.Kill();
    }

    private Tween meterTween;
    public void Watching_Enter()
    {
        mySpriteRenderer.sortingLayerName = "Watching";
        mySpriteRenderer.sortingOrder = 2;

        //Debug.Log("Watching game!");

        //if (myHeightType != HeightType.Short || (myHeightType == HeightType.Short && myBoxes.Count > 1 && transform.localPosition.y > -2f))
        //{
        meter.SetActive(true);
        resolveBar.SetActive(true);
        //resolveBar.transform.localPosition = Vector3.zero;
        resolveBar.transform.localPosition = new Vector3(-0.77f, 0.07f, 0f);
        resolveBar.transform.SetScaleX(1f);
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


        //if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString())
        //{
        //    if (transform == GameManager2.instance.equality_ShortC.transform)
        //    {
        //        GameManager2.instance.Equality_Success();
        //        meter.SetActive(false);
        //    }
        //}


    }

    private Tween jumpDownTween;
    public void Watching_Update()
    {
        if(mySpriteRenderer.sortingLayerName != "Watching")
        {
            mySpriteRenderer.sortingLayerName = "Watching";
        }

        if (myBoxes.Count == 1)
        {
            if (myBoxes[0].boxHealthState == BoxHealthState.Broken)
            {
                if (jumpDownTween == null)
                {
                    // Debug.Log("jump to ground");
                    jumpDownTween = transform.DOLocalMoveY(-15.1f, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f);

                    jumpDownTween.OnComplete(() =>
                    {
                        myBoxes.RemoveAt(0);
                        fsm.ChangeState(States.Waiting);
                        jumpDownTween.Kill();
                        jumpDownTween = null;
                        //SoundManager.instance.PlaySingle(SoundManager.instance.grunt);
                    });
                }
            }

        }
        else if (myBoxes.Count > 1)
        {
            //check if bottom box is broken. Always check the bottom box, since top box becomes bottom box.
            if (myBoxes[0].boxHealthState == BoxHealthState.Broken)
            {
                if (jumpDownTween == null)
                {
                    //Debug.Log("jump to ground");
                    jumpDownTween = transform.DOLocalMoveY(-15.1f + myBoxes[0].GetComponent<SpriteRenderer>().bounds.size.y, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f);

                    jumpDownTween.OnComplete(() =>
                    {
                        myBoxes.RemoveAt(0);
                        fsm.ChangeState(States.Waiting);
                        jumpDownTween.Kill();
                        jumpDownTween = null;
                        //SoundManager.instance.PlaySingle(SoundManager.instance.grunt);
                    });
                }
            }

            //if (myBoxes[1].boxHealthState == BoxHealthState.Broken) {
            //    if (jumpDownTween == null)
            //    {
            //        Debug.Log("should jump to first box");
            //        jumpDownTween = transform.DOLocalMoveY(-15.1f + myBoxes[0].GetComponent<SpriteRenderer>().bounds.size.y, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f);

            //        jumpDownTween.OnComplete(() => {
            //            //fsm.ChangeState(States.Waiting);
            //            jumpDownTween.Kill();
            //            //SoundManager.instance.PlaySingle(SoundManager.instance.grunt);
            //        });
            //    }
            //}
        }

    }

    public void Watching_Exit()
    {
        myAnimator.SetBool("isCheering", false);
        heartBarTween.Kill();

    }


    public Tween gaveUpWalkTween;
    public SitSite closestSitSite;
    public int sitPointIndex = 0;
    public bool skipSittingDown = false; 
    public void GaveUp_Enter()
    {
        if (skipSittingDown == false)
        {
            //determin sites with open spots, determine the closest one, select one of it's sit points, turn it to occupied, and chart a path to it, and sit.
            List<SitSite> possibleSites = new List<SitSite> { GameManager2.instance.sitSite1, GameManager2.instance.sitSite2, GameManager2.instance.sitSite3 };
            closestSitSite = null;
            //figure out open and closest sites
            for (int i = 0; i < possibleSites.Count; i++)
            {
                int numOpenPos = 0;
                for (int j = 0; j < possibleSites[i].sitPoints.Count; j++)
                {

                    if (possibleSites[i].sitPoints[j].occupant == null)
                    {
                        numOpenPos++;
                    }
                }
                if (numOpenPos == 0)
                {
                    possibleSites.RemoveAt(i);
                }
                else
                {
                    if (closestSitSite == null)
                    {
                        closestSitSite = possibleSites[i];
                    }
                    else
                    {
                        if (Mathf.Abs(transform.position.x - closestSitSite.transform.position.x) < Mathf.Abs(transform.position.x - possibleSites[i].transform.position.x))
                        {
                            closestSitSite = possibleSites[i];
                        }
                    }
                }
            }

            //figure out the exact point at the site
            sitPointIndex = 0;
            for (int k = 0; k < closestSitSite.sitPoints.Count; k++)
            {
                if (closestSitSite.sitPoints[k].occupant == null)
                {
                    sitPointIndex = k;
                    closestSitSite.sitPoints[k].occupant = GetComponent<Character>();
                    break;
                }
            }

            //chart a path to sit point
            //if shortest gave up on a box, make that box unoccupied
            if (myBoxes.Count > 0)
            {
                myBoxes[0].boxOccupiedState = BoxOccupiedState.Unoccupied;
                //first jump down then move to the sit point
                transform.DOLocalMoveY(-15.1f, JUMP_SPEED).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f).OnComplete(() =>
                {
                    myBoxes.RemoveAt(0);
                    /////SoundManager.instance.PlaySingle(SoundManager.instance.grunt, 0.5f);
                    WalkToSitPoint();
                });
            }
            else
            {
                WalkToSitPoint();
            }
        }
        else
        {
            meter.SetActive(false);
            myAnimator.SetBool("isCheering", false);
        }

        //transform.localPosition = new Vector3(transform.localPosition.x, originalPos.y, transform.localPosition.z);
        //talk to people if there are more than 2 people next to each other
        if (GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equality.ToString() || GameManager2.instance.fsm.CurrentStateMap.state.ToString() == GameManager2.States.Equity.ToString())
        {
            Debug.Log("should show fail SCREEN");
            StartCoroutine(GameManager2.instance.Failed());
        }



    }

    public void WalkToSitPoint()
    {
        transform.DOKill();
        gaveUpWalkTween = transform.DOLocalMoveX(closestSitSite.sitPoints[sitPointIndex].transformPoint.position.x, WALK_SPEED).SetSpeedBased().SetEase(Ease.Linear);
        gaveUpWalkTween.OnComplete(() =>
        {
            mySpriteRenderer.sortingLayerName = "GaveUp";
            mySpriteRenderer.sortingOrder = closestSitSite.sitPoints[sitPointIndex].zOrder;
            meter.SetActive(false);
            gaveUpWalkTween = null;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y +  closestSitSite.sitPoints[sitPointIndex].transformPoint.localPosition.y , transform.localPosition.z);
            myAnimator.SetBool("isSitting", true);
            //orienting to face each other
            //Debug.Log(transform.name + " sitSite: " + closestSitSite.transform.name +  " sitPointIndex " + sitPointIndex + "yRotation: " + closestSitSite.sitPoints[sitPointIndex].yAngle);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, closestSitSite.sitPoints[sitPointIndex].yAngle, transform.rotation.eulerAngles.z);
            if(closestSitSite.sitPoints[sitPointIndex].yAngle > 0f)
            {
                prevDir = "left";
            }
            else
            {
                prevDir = "right";
            }
            StartCoroutine(WaitToStartTalking());
        });
    }

    IEnumerator WaitToStartTalking()
    {
        yield return new WaitForSeconds(1f);
        if (speechBubble != null)
        {
            speechBubble.SetActive(true);
        }
    }

    public void GetDownBox_ThenFence()
    {
        transform.DOKill();
        myAnimator.SetBool("isCheering", false);

        walkToFence = true; 
        //myAnimator.SetBool("isMoving", false);
        float randomX = Random.Range(transform.localPosition.x - 5f, transform.localPosition.x + 5f);
        Vector3 secondPoint = new Vector3(randomX, transform.localPosition.y + 0.5f, transform.localPosition.z);
        Vector3 thirdPoint = new Vector3(randomX, startPoint.y, transform.localPosition.z);
        Vector3[] points = { transform.localPosition, secondPoint, thirdPoint };

        Tween jumpOffBoxTween = transform.DOLocalPath(points, 25f, PathType.CatmullRom).SetSpeedBased().SetEase(Ease.InQuad);

        jumpOffBoxTween.OnComplete(() =>
        {
           // Debug.Log("finished jumping offf boxes"); 
            myBoxes.Clear();
            transform.DOKill();
            skipSittingDown = true;
            fsm.ChangeState(States.GaveUp);
            StartCoroutine(GetOnFence());
        });
    }

    public void StandUp()
    {
        //for CharC who just stands.
        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }

        meter.SetActive(false);
        
        transform.DOKill();
        myAnimator.SetBool("isMoving", false);
        if (fsm.CurrentStateMap.state.ToString() != States.GaveUp.ToString())
        {
            skipSittingDown = true;
            fsm.ChangeState(States.GaveUp);
        }
        myAnimator.SetBool("isSitting", false);
        myAnimator.SetBool("isMoving", false);
        transform.localPosition = new Vector3(transform.localPosition.x, startPoint.y-1f, transform.localPosition.z);
        mySpriteRenderer.sortingOrder = mySpriteRenderer.sortingOrder + 3; 
    }

    private bool walkToFence = false;
    private Sequence moveOnFenceS;
    public IEnumerator GetOnFence()
    {
        transform.DOKill();
        speechBubble.SetActive(false);
        myAnimator.SetBool("isSitting", false);
        walkToFence = true;
        transform.localPosition = new Vector3(transform.localPosition.x, startPoint.y, transform.localPosition.z);

        FencePoint fp = null;
        for (int i = 0; i < GameManager2.instance.fencePoints.Count; i++)
        {
            if (GameManager2.instance.fencePoints[i].occupant == null)
            {
                fp = GameManager2.instance.fencePoints[i];

                fp.occupant = GetComponent<Character>();
                //Vector3[] gaveUpPoints = { transform.localPosition, new Vector3(fp.transformPoint.position.x, transform.localPosition.y, transform.localPosition.z), new Vector3(GameManager2.instance.finalBoxes.transform.position.x, transform.localPosition.y, transform.localPosition.z) };
                //gaveUpWalkTween = transform.DOLocalPath(gaveUpPoints, WALK_SPEED).SetSpeedBased(true).SetEase(Ease.Linear);
                gaveUpWalkTween = transform.DOLocalMoveX(GameManager2.instance.finalBoxes.transform.position.x, 8f).SetSpeedBased(true).SetEase(Ease.Linear);


                gaveUpWalkTween.OnPlay(() =>
                {
                   // Debug.Log("walk to boxes started");
                    myAnimator.SetBool("isSitting", false);
                    if (closestSitSite != null)
                    {
                        mySpriteRenderer.sortingOrder = closestSitSite.sitPoints[sitPointIndex].zOrder;
                    }
                });

                //gaveUpWalkTween.OnWaypointChange((int index) => {
                //    if (index == 1)
                //    {
                //        gaveUpWalkTween.TogglePause();
                //        StartCoroutine(WaitInLine(i*4f));
                //    }
                //});

                gaveUpWalkTween.OnComplete(() =>
                {
                   // Debug.Log("walk to boxes finished");
                    gaveUpWalkTween = null;

                    transform.DOLocalMoveY(transform.localPosition.y + 7f, 15f).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f).OnComplete(() => {
                        mySpriteRenderer.sortingLayerName = "Watching";
                        if (mySpriteRenderer.sortingOrder > 1)
                        {
                            mySpriteRenderer.sortingOrder = mySpriteRenderer.sortingOrder - 1;
                        }
                        transform.DOLocalMoveY(transform.localPosition.y + 7f, 15f).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f).OnComplete(() => {
                            mySpriteRenderer.sortingLayerName = "Foreground";
                            if (mySpriteRenderer.sortingOrder > 1)
                            {
                                mySpriteRenderer.sortingOrder = mySpriteRenderer.sortingOrder - 1;
                            }
                            transform.DOLocalMoveY(GameManager2.instance.fencePointsParent.transform.localPosition.y, 15f).SetSpeedBased().SetEase(Ease.InExpo).SetDelay(0.25f).OnComplete(() => {
                                mySpriteRenderer.sortingLayerName = "Characters";
                                if (mySpriteRenderer.sortingOrder > 1)
                                {
                                    mySpriteRenderer.sortingOrder = mySpriteRenderer.sortingOrder - 1;
                                }
                                //iterate through available points and see which one's closest one that's still available and move to it.

                                if (i == Mathf.FloorToInt(GameManager2.instance.allFenceChars.Count / 2))
                                {
                                    Camera.main.transform.DOKill();
                                    Camera.main.transform.DOMoveX(GameManager2.instance.fencePoints[i].transformPoint.position.x, 5f).SetEase(Ease.InOutQuad);
                                }

                                
                                moveOnFenceS = DOTween.Sequence();
                                moveOnFenceS.Append(transform.DOLocalRotate(new Vector3(0f, 0f, transform.localEulerAngles.z - 5f), 0.5f).SetLoops(100, LoopType.Yoyo).SetEase(Ease.Linear));
                                moveOnFenceS.Join(transform.DOLocalMoveX(fp.transformPoint.position.x, 8f * 1.75f).SetSpeedBased().SetEase(Ease.Linear).SetDelay(0.25f).OnComplete(OnFinishMovingToFencePoint));
                            });
                        });

                    });

                });

                break;
            }
        }



        yield return new WaitForSeconds(1f);
    }


    //IEnumerator WaitInLine(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    gaveUpWalkTween.TogglePause();
    //}

    public void OnFinishMovingToFencePoint()
    {
        mySpriteRenderer.sortingLayerName = "LevelObjects";
        if (mySpriteRenderer.sortingOrder > 1)
        {
            mySpriteRenderer.sortingOrder = mySpriteRenderer.sortingOrder - 1;
        }
        GameManager2.instance.howManyOnFence++;
        moveOnFenceS.Kill(true);
    }

    private Tween fenceJumpTween = null;
    public void HandleJump(bool sweetSpot)
    {
        if (fenceJumpTween == null) {
            float delay = 0f;
            if (!sweetSpot)
            {
                delay = Random.Range(0f, 2f);
            }

            fenceJumpTween = transform.DOLocalMoveY(transform.localPosition.y + (WALK_SPEED/2f), 0.75f).SetLoops(2, LoopType.Yoyo).SetDelay(delay).SetEase(Ease.InOutCirc);
            

            fenceJumpTween.OnStepComplete(() =>
            {
                if (fenceJumpTween.CompletedLoops() == 1)
                {
                    if (!sweetSpot)
                    {
                        int jumpNow = Random.Range(0, 3);
                        if (jumpNow == 0)
                        {
                            SoundManager.instance.PlaySingle(SoundManager.instance.jumpFence, 0.25f);
                        }
                    }
                }
                if (fenceJumpTween.CompletedLoops() > 1)
                {
                    fenceJumpTween = null;
                }
                
            });
        }
    }

    public void FallToGround()
    {
        transform.DOLocalMoveY(originalPos.y, 10f).SetSpeedBased().SetDelay(2.25f).SetEase(Ease.InExpo).OnComplete(()=> {
            myAnimator.SetBool("isAltCheering", true);
        });
    }

    public void GaveUp_Update()
    {
        bool isWalkingNow = false; 
        if (gaveUpWalkTween != null )
        {
            //Debug.Log("not null");
            if (gaveUpWalkTween.IsActive() && gaveUpWalkTween.IsPlaying() )
            {
                isWalkingNow = true;
                //Debug.Log("is active");
                if (myAnimator.GetBool("isMoving") == false)
                {
                    //Debug.Log("setting WALKING");
                    myAnimator.SetBool("isSitting", false);
                    myAnimator.SetBool("isMoving", true);
                }

                //orienting towards end point
                float direction;
                if (walkToFence)
                {
                    direction = -GameManager2.instance.finalBoxes.transform.localPosition.x + transform.localPosition.x;
                }
                else
                {
                    direction = -closestSitSite.sitPoints[sitPointIndex].transformPoint.position.x + transform.localPosition.x;
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
        }

        if(isWalkingNow == false)
        {
            //Debug.Log("moving set to false");
            myAnimator.SetBool("isMoving", false);
        }
    }
    public void GaveUp_Exit()
    {
        myAnimator.SetBool("isSitting", false);
        Debug.Log("Exited GaveUp");
    }



    public void Offscreen_Enter()
    {
        meter.SetActive(true);
        resolveBar.SetActive(true);
        heartBar.SetActive(false);
        //resolveBar.transform.localPosition = Vector3.zero;
        resolveBar.transform.localPosition = new Vector3(-0.77f, 0.07f, 0f);
        resolveBar.transform.SetScaleX(1f);
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
       // Debug.Log("the animator state is: " + myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.chrD_moving") + myAnimator.GetCurrentAnimatorStateInfo(0).IsName("chrD_moving"));
        myAnimator.SetBool("isMoving", false);
       // Debug.Log("the animator state is2: " + myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.chrD_moving") + myAnimator.GetCurrentAnimatorStateInfo(0).IsName("chrD_moving"));

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
