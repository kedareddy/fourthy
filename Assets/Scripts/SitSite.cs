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
            SitPoint idea1Point = null;
            SitPoint idea2Point = null;
            //Vector3 idea1Pos = Vector3.zero;
            //Vector3 idea2Pos = Vector3.zero;
            float charCAngle = -1f; 

            //check for a char C
            for (int i = 0; i < sitPoints.Count; i++)
            {
                if (sitPoints[i].occupant != null)
                {
                    if (sitPoints[i].occupant.name.Contains("CharC"))
                    {
                        idea1Point = sitPoints[i];
                        //idea1Pos = sitPoints[i].occupant.GetComponent<Character>().speechBubble.transform.position;
                        charCAngle = sitPoints[i].yAngle;
                    }
                }
            }


            for (int j = 0; j < sitPoints.Count; j++)
            {
                if (sitPoints[j].occupant != null && charCAngle >= 0f)
                {
                    if (!sitPoints[j].occupant.name.Contains("CharC") && Mathf.Abs(sitPoints[j].yAngle - charCAngle) > 1f)
                    {
                        idea2Point = sitPoints[j];
                        //idea2Pos = sitPoints[j].occupant.GetComponent<Character>().speechBubble.transform.position;
                    }
                    //turn off chats
                    if (idea1Point!= null && idea2Point != null)
                    {
                        Debug.Log("idea 1 angle: " + idea1Point.yAngle + "idea 2 angle: " + idea2Point.yAngle); 
                        sitPoints[j].occupant.speechBubble.SetActive(false);
                        GameManager2.instance.TriggerIdeaBubble(idea1Point, idea2Point);
                        numConversations = 0;
                        break;
                    }
                }
            }
            
        }
    }
}
