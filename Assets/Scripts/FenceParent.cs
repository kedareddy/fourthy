using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.MyExtensions;
using DG.Tweening;

public class ExplodablePart
{
    public Explodable explodablePart;
}


public class FenceParent : MonoBehaviour
{
    private static FenceParent _instance;
    public static FenceParent Instance { get { return _instance; } }
    public GameObject debris;
    public List<SpriteRenderer> fenceParts;
    public List<ExplodablePart> fenceExplodables;
    public GameObject fenceCrusher;
    //public List<Explodable> fenceExplodables;
    public Sprite brokenFenceSprite;
    private int previousFencePower;

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
    void Start()
    {
        previousFencePower = GameManager2.instance.currenFencePower;
        Component[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spR in spriteRenderers)
        {
            fenceParts.Add(spR);
            //ExplodablePart eP; 
            //eP.explodablePart = spR.GetComponent<Explodable>();
            //if (eP.explodablePart != null)
            //{
            //    fenceExplodables.Add(eP);
            //}
        }

       // Debug.Log("How many fence Parts" + fenceParts.Count);
        //Debug.Log("How many fence Explodables" + fenceExplodables.Count);
    }

    public void ApplyDamage(bool fullDamage)
    {
        fenceParts.Shuffle();
        int damageToApply; 
        if (fullDamage)
        {
            damageToApply = 7;
            Debug.Log("How many times is the fenceJump Sound playing");
            SoundManager.instance.PlaySingle(SoundManager.instance.jumpFence, 0.75f);
        }
        else
        {
            damageToApply = 2;
            
        }
        int numOfFlips = 0; 
        for(int i = 0; i < fenceParts.Count; i++)
        {
            if(fenceParts[i].sprite != brokenFenceSprite && numOfFlips < damageToApply)
            {
                fenceParts[i].sprite = brokenFenceSprite;
                numOfFlips++;
                if(numOfFlips == damageToApply)
                {
                    break;
                }
            }
        }        
    }

    private bool deleteSomeFenceParts = false; 
    public void DestroyFence()
    {
        List<SpriteRenderer> partsForDeletion = new List<SpriteRenderer>(); 
        for (int i = 0; i < fenceParts.Count; i++)
        {
            fenceParts[i].GetComponent<PolygonCollider2D>().enabled = true; 
           if (i % 2 == 0)
           {
                Explodable eP = fenceParts[i].GetComponent<Explodable>();
                if (eP != null)
                {
                    eP.explode();
                }
           }
           else
           {
              partsForDeletion.Add(fenceParts[i]);
           }
            //fenceExplodables[i].explodablePart.explode();
        }

        Tween fenceCrushTween = fenceCrusher.transform.DOLocalMoveY(-55f, 10f).SetSpeedBased().SetEase(Ease.InExpo);

        fenceCrushTween.OnUpdate(() =>
        {
            
            if (fenceCrushTween.ElapsedPercentage() > 0.55f)
            {
                if(deleteSomeFenceParts == false)
                {
                    deleteSomeFenceParts = true;
                    for (int j = 0; j < partsForDeletion.Count; j++)
                    {
                        Destroy(partsForDeletion[j].gameObject);
                    }
                    debris.SetActive(true);
                    GameManager2.instance.gamePlayers.SetActive(false);
                    GameManager2.instance.winningTeam.SetActive(true);
                    GameManager2.instance.liberationFourthy.SetActive(true);
                    SoundManager.instance.PlaySingle(SoundManager.instance.fenceCrash, 0.6f);
                }
                

            }
            
        });
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        int powerDiff = GameManager2.instance.currenFencePower - previousFencePower;
        if ( powerDiff !=0)
        {
            if(powerDiff == 1)
            {
                ApplyDamage(false);
            }
            else
            {
                ApplyDamage(true);
            }
            previousFencePower = GameManager2.instance.currenFencePower; 
        }
    }
}
