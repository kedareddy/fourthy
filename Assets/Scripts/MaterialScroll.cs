using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialScroll : MonoBehaviour
{
    private Material myMat;
    private float speed = 10f;
    private float currentscroll;
	void Start()
	{
        myMat = GetComponent<SpriteRenderer>().material;
	}

	void Update()
	{
		currentscroll += speed * Time.deltaTime;
        myMat.mainTextureOffset = new Vector2(currentscroll, 0);
	}
}
