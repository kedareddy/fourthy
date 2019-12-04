using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.MyExtensions; 
public class FenceParent : MonoBehaviour
{
    private static FenceParent _instance;
    public static FenceParent Instance { get { return _instance; } }
    public List<SpriteRenderer> fenceParts;
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
        }
    }

    public void ApplyDamage(bool fullDamage)
    {
        fenceParts.Shuffle();
        int damageToApply; 
        if (fullDamage)
        {
            damageToApply = 7; 
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

    public void DestroyFence()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        int powerDiff = GameManager2.instance.currenFencePower - previousFencePower;
        if ( powerDiff != 0)
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
