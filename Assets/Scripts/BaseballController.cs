using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballController : MonoBehaviour
{

	public Animator batter, catcher, pitcher;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayBaseball());
    }

    private IEnumerator PlayBaseball()
    {
        while (true)
        {
            //if (pitcher.GetBool("isPitching") == true)
            //{
                pitcher.SetBool("isPitching", false);
            //}
            //if (batter.GetBool("isBatting") == false)
            //{
                batter.SetBool("isBatting", false);
            batter.SetBool("isBattingLow", false);
            //}
            catcher.SetBool("isCatching", false);
            //Debug.Log("not playing");

            yield return new WaitForSeconds(Random.Range(5f, 15f));
            //Debug.Log("is playing");
            pitcher.SetBool("isPitching", true);
            yield return new WaitForSeconds(0.2f);
            int rand = Random.Range(0, 6);
            if (rand > 2)
            {
                batter.SetBool("isBatting", true);
            }
            else
            {
                batter.SetBool("isBattingLow", true);
            }
            catcher.SetBool("isCatching", true);
            yield return new WaitForSeconds(1.05f);
        }
    }

}
