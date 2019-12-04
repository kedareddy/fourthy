using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SitPoint
{
    [SerializeField]
    public Transform transformPoint;
    [SerializeField]
    public Character occupant;
    [SerializeField]
    public int zOrder;
    [SerializeField]
    public float yAngle;
    // optionally some other fields
}

public class SitSite : MonoBehaviour
{
    //public List<Dictionary<bool, Transform>> sitPoints = new List<Dictionary<bool, Transform>>(); 
    [SerializeField]
    public List<SitPoint> sitPoints = new List<SitPoint>();

    public int numConversations = 0;
    private int CONVERSATIONS_THRESHOLD = 5;
    // Update is called once per frame
    void Update()
    {
        if(numConversations >= CONVERSATIONS_THRESHOLD)
        {
            //turn off chats
            for (int i = 0; i < sitPoints.Count; i++)
            {
                if (sitPoints[i].occupant != null)
                {
                    sitPoints[i].occupant.speechBubble.SetActive(false);
                }
            }
            GameManager2.instance.TriggerIdeaBubble(transform.position);
            numConversations = 0; 
        }
    }
}
