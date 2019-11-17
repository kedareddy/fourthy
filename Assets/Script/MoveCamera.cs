using UnityEngine;
using System.Collections;


public class MoveCamera : MonoBehaviour
{
    //
    // VARIABLES
    //

    public float turnSpeed = 35.0f;         // Speed of camera turning when mouse moves in along an axis
    public float panSpeed = 360.0f;         // Speed of the camera when being panned
    public float zoomSpeed = 500.0f;        // Speed of the camera going back and forth

    public float turnDrag = 5.0f;           // RigidBody Drag when rotating camera
    public float panDrag = 3.5f;            // RigidBody Drag when panning camera
    public float zoomDrag = 3.3f;           // RigidBody Drag when zooming camera

    public float[] xBounds;

    private Vector3 mouseOrigin;            // Position of cursor when mouse dragging starts
    private bool isPanning;             // Is the camera being panned?
    private bool isRotating;            // Is the camera being rotated?
    private bool isZooming;             // Is the camera zooming?

    public float[] XBounds { get => xBounds; set => xBounds = value; }

    //
    // AWAKE
    //

    void Awake()
    {
        // Setup camera physics properties
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().useGravity = false;
    }

    //
    // UPDATE: For input
    //

    void Update()
    {
        // == Getting Input ==

        // Get the left mouse button
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // Get mouse origin
        //    mouseOrigin = Input.mousePosition;
        //    isRotating = true;
        //}

        // Get the right mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isPanning = true;
        }

        // Get the middle mouse button
        //if (Input.GetMouseButtonDown(2))
        //{
        //    // Get mouse origin
        //    mouseOrigin = Input.mousePosition;
        //    isZooming = true;
        //}


        // == Disable movements on Input Release ==

        if (!Input.GetMouseButton(1)) isRotating = false;
        if (!Input.GetMouseButton(0)) isPanning = false;
        if (!Input.GetMouseButton(2)) isZooming = false;

    }

    //
    // Fixed Update: For Physics
    //

    void FixedUpdate()
    {
        // == Movement Code ==

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            // Get mouse displacement vector from original to current position
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            // Set Drag
            GetComponent<Rigidbody>().angularDrag = turnDrag;

            // Two rotations are required, one for x-mouse movement and one for y-mouse movement
            GetComponent<Rigidbody>().AddTorque(-pos.y * turnSpeed * transform.right, ForceMode.Acceleration);
            GetComponent<Rigidbody>().AddTorque(pos.x * turnSpeed * transform.up, ForceMode.Acceleration);
        }

        // Move (pan) the camera on it's XY plane
        if (isPanning)
        {
            // Get mouse displacement vector from original to current position
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
            //pos.x = Mathf.Clamp(pos.x, minX, maxX);
            Vector3 move = new Vector3(pos.x * panSpeed, 0, 0);

            // Apply the pan's move vector in the orientation of the camera's front
            Quaternion forwardRotation = Quaternion.LookRotation(transform.forward, transform.up);
            move = forwardRotation * move;

            // Set Drag
            GetComponent<Rigidbody>().drag = panDrag;

            // Pan
            Rigidbody myRigidbody = GetComponent<Rigidbody>(); 
            myRigidbody.AddForce(move, ForceMode.Acceleration);
            
                
       
        }

        Rigidbody myRigidbody1 = GetComponent<Rigidbody>(); 
        Vector3 myPos = myRigidbody1.position;
        myPos.x = Mathf.Clamp(myPos.x, -80f, -75f);
        myRigidbody1.position = myPos;

        // Move the camera linearly along Z axis
        if (isZooming)
        {
            // Get mouse displacement vector from original to current position
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
            Vector3 move = pos.y * zoomSpeed * transform.forward;

            // Set Drag
            GetComponent<Rigidbody>().drag = zoomDrag;

            // Zoom
            GetComponent<Rigidbody>().AddForce(move, ForceMode.Acceleration);
        }
    }

}