using UnityEngine;
using System.Collections;

//Ensure raycast do no start in object
//Edit -> Project Settings -> Physics2D -> Uncheck box labeled "Queries start in Collider"

//Using ray cast means if player clicks over a block itll connect to it. Do we keep or only connect at mouse click location?

//Object with this script will need to have distance joint2d component

//Change mask to default manually in unity

//All lines followed by "//" should be moved to a management script for entire game to better seperate script functionality

//Manually drag in LineRenderer object to attach to gameobject with this script in unity

public class HookMechanic : MonoBehaviour {
    public LineRenderer line;

    private DistanceJoint2D joint;
    private Vector3 targetPos;
    private RaycastHit2D hit;
    public LayerMask mask;

    private Rigidbody2D rb; 

    public AudioClip hookSound;
    public AudioSource audioSource;

    public PlayerManager pm;    //
    public HookMovement hm;     //

    //Serves as maximum length for hook
    public float maxHookLength;

    //Will determine how fast hero climbs up rope
    public float reelSpeed;

    //The minimum length the hook will be while connected
    public float minimumHookLength;

    //Boolean value to determine if hero has to climb rope still
    private bool climb;

    //Boolean value stating if hero has hooked to object;
    private bool hooked;

    // Z Offset for line render
    public float lineRendererZOffset;

	// Use this for initialization
	void Start () {
        setRepeatFunctions();
        initDistanceJoint2d();
        initLineRenderer();
        initScriptVariables();
        

        //All below should be handeled in different script when possible
        pm = GetComponent<PlayerManager>(); //
        hm = GetComponent<HookMovement>(); //

        pm.enabled = true; //
        hm.enabled = false; //
	}

    //Initialize all variables used in this script
    void initScriptVariables()
    {
        climb = false;
        hooked = false;
        maxHookLength = 15;
        minimumHookLength = .5f;
        reelSpeed = .5f;
        lineRendererZOffset = -1;

    }

    //Set repeat on all functions that will need to be called at set time frames
    void setRepeatFunctions()
    {
        InvokeRepeating("reelIn", 0.0f, .05f);
    }

    //Initialization for all values connected to DistanceJoint2d component
    void initDistanceJoint2d()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        joint.maxDistanceOnly = true;
        joint.enableCollision = true;
    }

    //Initialization for all value connected to LineRenderer component
    void initLineRenderer()
    {
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (getRayCastHit())
            {
                pm.enabled = false; //
                hm.enabled = true;  //
                
                setLine();
                setHook();
            }
        }

        if (Input.GetKeyDown(KeyCode.W)) { climb = true; }

        if(Input.GetKeyUp(KeyCode.W)) { climb = false; }

        //While hook is attached, update line(rope) to move with player
        if (joint.enabled == true){ line.SetPosition(0, transform.position); }

        //Space will disconnect hook;
        if (joint.enabled && Input.GetKeyDown(KeyCode.Space)) { disconnectHook();  }

    }

    void disconnectHook()
    {
        climb = false;
        joint.enabled = false;
        line.enabled = false;

        pm.enabled = true;  //
        hm.enabled = false; //

        pm.jump();
        
    }

    //Cast ray for hook and return true only ifS it collides with a collider type component
    bool getRayCastHit()
    {
        //Get world position of mouse click
        targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

        //Shoots ray to where mouse clicked and detects if it hits an object along the way
        hit = Physics2D.Raycast(transform.position, targetPos - transform.position, maxHookLength, mask);

        return hit.collider != null && hit.collider.gameObject.GetComponent<Collider2D>() != null;
    }

    //Set LineRenderer component with hit point from raycast
    void setLine()
    {
        float distance = Vector3.Distance(transform.position, hit.point);

        line.enabled = true;
        setLinePosition(transform.position, hit.point);
        line.material.mainTextureScale = new Vector2(distance * 2, 1);
    }

    //Set DistanceJoint2D component(hook) with hit point from raycast
    void setHook()
    {
        joint.enabled = true;
        joint.connectedAnchor = new Vector2(hit.point.x, hit.point.y);
        setHookLength();

        playHookSound();
    }

    //Play audio clip for hook
    void playHookSound()
    {
        audioSource.PlayOneShot(hookSound, 0.7f);
    }

    //Set hook length to ensure it does not go farther than maximum hook length
    void setHookLength()
    {
        float newHookLength = Vector2.Distance(transform.position, targetPos);
        if (newHookLength < minimumHookLength)
            joint.distance = minimumHookLength;
        else
            joint.distance = newHookLength - .5f ;
    }

    //Reel in the hero till minimumHookLength is reached
    void reelIn()
    {
        if (climb && joint.distance > minimumHookLength)
        {
            if (joint.distance - reelSpeed < minimumHookLength)
                joint.distance = minimumHookLength;
            joint.distance -= reelSpeed;
        }
    }

    //Sets the end points of the LineRenderer component
    void setLinePosition(Vector3 pos0, Vector3 pos1)
    {
        Vector3 a = new Vector3(pos0.x, pos0.y, pos0.z + lineRendererZOffset);
        Vector3 b = new Vector3(pos1.x, pos1.y, pos1.z + lineRendererZOffset);
        line.SetPosition(0, a);
        line.SetPosition(1, b);
    }
}
