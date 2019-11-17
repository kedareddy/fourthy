using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CountDownTimer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    float currCountdownValue;
    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            //Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        //LevelManager.Instance.KillPlayer(LevelManager.Instance.PlayerPrefabs[0]);
        //StartCoroutine(StartCountdown());

    }


}

