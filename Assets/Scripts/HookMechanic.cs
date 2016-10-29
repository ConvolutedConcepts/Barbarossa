using UnityEngine;
using System.Collections;

//Ensure raycast do no start in object
//Edit -> Project Settings -> Physics2D -> Uncheck box labeled "Queries start in Collider"

//Using ray cast means if player clicks over a block itll connect to it. Do we keep or only connect at mouse click location?

//Object with this script will need to have distance joint2d component

//Change mask to default manually in unity

public class HookMechanic : MonoBehaviour {
    public LineRenderer line;
    private DistanceJoint2D joint;
    public Vector3 targetPos;
    public RaycastHit2D hit;
    public LayerMask mask;

    //Serves as maximum length for hook
    public float hook_length;

    //Will determine how fast hero climbs up rope
    public float hookSpeed;

    //The minimum length the hook will be while connected
    public float minimumHookLength;

    //Boolean value to determine if hero has to climb rope still
    public bool climb;

    //Boolean value to determine if hero should descend down
    public bool descend;

    //Boolean value stating if hero has hooked to object;
    public bool hooked;

	// Use this for initialization
	void Start () {
        //Call the function "reelIn" starting at time first arg and called every second arg
        InvokeRepeating("reelIn", 0.0f, .05f);

        InvokeRepeating("reelDown", 0.0f, .05f);
        
        line.enabled = false;
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        joint.maxDistanceOnly = true;
        joint.enableCollision = true;
        climb = false;
        descend = false;
        hookSpeed = .1f;
        minimumHookLength = .5f;
        hook_length = 3;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Get world position of mouse click
            targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

            //Shoots ray to where mouse clicked and detects if it hits an object along the way
            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, hook_length, mask);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<BoxCollider2D>() != null)
            {
                joint.connectedAnchor = new Vector2(hit.point.x, hit.point.y);

                //Set line for hook
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);

                float distance = Vector3.Distance(transform.position, hit.point);

                line.material.mainTextureScale = new Vector2(distance * 2, 1);
                //line.material.SetTextureScale("_MainTex", new Vector2(transform.localScale.x, transform.localScale.y));

                climb = true;
                line.enabled = true;
                joint.enabled = true;
                float newHookLength = Vector2.Distance(transform.position, targetPos);
                if (newHookLength < minimumHookLength)
                    joint.distance = minimumHookLength;
                else
                    joint.distance = newHookLength;
            }
        }

        //While hook is attached, update line(rope) to move with player
        if (joint.enabled == true)
        {
            line.SetPosition(0, transform.position);
        }

        //Left Click released. Stop climb
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            climb = false;
        }

        //Right click will start descent
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            climb = false;
            descend = true;
        }

        //Releasing right click will stop descent
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            descend = false;
        }

        //Space will disconnect hook;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            climb = false;
            descend = false;
            joint.enabled = false;
            line.enabled = false;
        }
    }

    //Reel in the hero till minimumHookLength is reached
    void reelIn()
    {
        if(climb && joint.distance > minimumHookLength)
        {
            if (joint.distance - hookSpeed < minimumHookLength)
                joint.distance = minimumHookLength;
            joint.distance -= hookSpeed;
        }
    }

    //Hero will descend down as long as descend is true and hook length doesnt extend past maximum
    void reelDown()
    {
        if (descend && joint.distance < hook_length)
        {
            if (joint.distance + hookSpeed > hook_length)
                joint.distance = hook_length;
            joint.distance += hookSpeed;
        }
    }
}
