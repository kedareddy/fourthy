using UnityEngine;
using System.Collections;


public class NewDrag : MonoBehaviour
{
    //
    // VARIABLES
    //

    public float panSpeed = 360.0f;         // Speed of the camera when being panned
    public float panDrag = 3.5f;            // RigidBody Drag when panning camera

    public float cameraMaxY = 50;
    public float cameraMinY = -50;
    public float cameraMaxX = 50;
    public float cameraMinX = -50;

    public int numOfPans = 0; 


    private Vector3 bl;
    private Vector3 tr;


    private Vector3 mouseOrigin;            // Position of cursor when mouse dragging starts
    private bool isPanning;             // Is the camera being panned?

    private Rigidbody2D rigidbody;
    private float timeInitClick;
    private bool timeInitialized = false;
    //
    // AWAKE
    //

    void Awake()
    {
        // Setup camera physics properties
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        Physics2D.gravity = Vector2.zero;
        //rigidbody.useGravity = false;
    }

    //
    // UPDATE: For input
    //

    void LateUpdate()
    {

        // Get the right mouse button
        if (Input.GetMouseButtonDown(0))
        {

            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isPanning = true;
            numOfPans++;
            //Debug.Log("num of pans: " + numOfPans);
            if (timeInitialized == false)
            {
                timeInitialized = true;
                timeInitClick = Time.time;
            }

        }


        // == Disable movements on Input Release ==
        if (!Input.GetMouseButton(0))
        {
            isPanning = false;
            timeInitialized = false;
          
            

        }

        if ((Time.time - timeInitClick) > 0.5f)
        {
            isPanning = false;
        }

    }

    
    //
    // Fixed Update: For Physics
    //

    void FixedUpdate()
    {

        // Move (pan) the camera on it's XY plane
        if (isPanning)
        {
            
            // Get mouse displacement vector from original to current position
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
            Vector3 move = new Vector3(pos.x * panSpeed, pos.y, 0);

            // Apply the pan's move vector in the orientation of the camera's front
            Quaternion forwardRotation = Quaternion.LookRotation(transform.forward, transform.up);
            move = forwardRotation * move;

            // Set Drag
            rigidbody.drag = panDrag;

            // Pan
            rigidbody.AddForce(move, ForceMode2D.Force);
        }

    }

}