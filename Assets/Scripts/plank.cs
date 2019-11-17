using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SpriteGlow
{
    public class plank : MonoBehaviour
    {
        private SpriteGlowEffect mySpriteGlowEffect;


        // Start is called before the first frame update
        void Start()
        {
            mySpriteGlowEffect = gameObject.GetComponent<SpriteGlowEffect>();
            DOTween.To(() => mySpriteGlowEffect.GlowBrightness , x => mySpriteGlowEffect.GlowBrightness = x, 8, 2.5f).SetLoops(-1, LoopType.Yoyo);
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}

        //public void MyClick()
        //{
        //    Debug.Log("clicker");
        //    juneTween.Play(); 
        //}

        void OnMouseDown()
        {
            Debug.Log("clicker");
        }
    }
}
