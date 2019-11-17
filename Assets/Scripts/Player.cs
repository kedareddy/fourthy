using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lightbug.Kinematic2D;
using Lightbug.CoreUtilities;


    public class Player : MonoBehaviour
    {

        public Animator myAnimator;
        public Text woodPiecesText;
        public GameObject myBoxPrefab;
        public Button myBoxButton1;
        public Button myBoxButton2;
      
        private float lastPosX = 0f;


        private void Awake()
        {
            myAnimator.SetTrigger("faceForward");
        }
        // Start is called before the first frame update
        void Start()
        {
            lastPosX = transform.position.x;

        }


        // Update is called once per frame
        void Update()
        {

            if (Mathf.Abs(transform.position.x - lastPosX) > 0.001f)
            {
                //Debug.Log("hash of state: " + myAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash + "::::" + Animator.StringToHash("Base Layer.gotBox"));
                //if (myAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("BaseLayer.gotBox"))
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.StandingBox"))
                {
                    myAnimator.SetBool("walkingBox", true);
                }
                else
                {
                    myAnimator.SetBool("isWalking", true);
                }


                lastPosX = transform.position.x;
            }
            else
            {
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.WalkingBox"))
                {
                    myAnimator.SetBool("walkingBox", false);
                }
                else
                {
                    myAnimator.SetBool("isWalking", false);
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                myAnimator.SetTrigger("faceForward");
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                MusicManager.setVolume(0.4f);
                MusicManager.play("hop");
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.StandingBox"))
                {
                    myAnimator.SetTrigger("crouchBox");
                    myAnimator.SetBool("gotBox", false);
                    myBoxButton1.interactable = false;
                    myBoxButton2.interactable = false;
                    woodPiecesText.text = "0";
                    StartCoroutine(InstantiateBox());
                }
                else
                {
                    myAnimator.SetTrigger("crouch");
                    LayerMask myMask = LayerMask.GetMask("Default");
                    Collider2D returnedCollider = Physics2D.OverlapArea(new Vector2(transform.position.x - 2f, transform.position.y), new Vector2(transform.position.x + 2f, transform.position.y));
                    Debug.Log("returned collider: " + returnedCollider.transform.name);
                    if (returnedCollider != null)
                    {
                        //collect wood
                        if (returnedCollider.transform.gameObject.activeInHierarchy && returnedCollider.transform.name == "board")
                        {
                            MusicManager.play("pickup");
                            returnedCollider.transform.gameObject.SetActive(false);
                            //GameManager1.instance.woodPieces = GameManager1.instance.woodPieces + 1;
                            woodPiecesText.transform.DOJump(woodPiecesText.transform.position, 25, 1, 1, true).OnComplete(IncrementWoodNumber);
                        }

                    }
                }
            }

            if(transform.position.y > -5f && boxDown == true)
            {
                StartCoroutine(CheckGameEnd());
            }
        }

        public void IncrementWoodNumber()
        {
            //woodPiecesText.text = (GameManager1.instance.woodPieces + 1).ToString();
            //if (GameManager1.instance.woodPieces >= 2)
            if(true)
            {
                myBoxButton1.interactable = true;
                myBoxButton2.interactable = true;
                myBoxButton2.transform.DOShakeRotation(1f, new Vector3(0f, 0f, 80f), 6, 20f, true).SetLoops(-1, LoopType.Restart);
            }
        }

        private bool boxDown = false;
        IEnumerator InstantiateBox()
        {
            yield return new WaitForSeconds(0.5f);
            MusicManager.play("drop");
            boxDown = true; 
            Instantiate(myBoxPrefab, new Vector3(transform.position.x, myBoxPrefab.transform.position.y, myBoxPrefab.transform.position.z), myBoxPrefab.transform.rotation);
        }

        public void OnMakeBoxClick()
        {
            //Debug.Log("box amker clicker!" + GameManager1.instance.woodPieces);
            //if (GameManager1.instance.woodPieces >= 2)
            if(true)
            {
                myAnimator.SetBool("gotBox", true);
                DOTween.KillAll();
                myBoxButton2.transform.eulerAngles = new Vector3(myBoxButton2.transform.eulerAngles.x, myBoxButton2.transform.eulerAngles.y, 0);
            }
        }

        IEnumerator CheckGameEnd()
        {
            yield return new WaitForSeconds(1f);
            if(transform.position.y > -5f && boxDown == true)
            {
                myAnimator.SetTrigger("watchingGame");
                //GameManager1.instance.InitiateEndSequence(); 
                MusicManager.play("cheer");
            }
        }
    }

