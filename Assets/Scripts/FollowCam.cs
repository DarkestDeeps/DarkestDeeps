using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public GameObject target;
    public GameObject attackTarget;
	public Player player;
	public EnemyTargeting playerTargetSphere;
    public float rotateSpeed = 5;
    public float xSpeed = 115.0f;
    public float ySpeed = 50.0f;
	public float distance;
    public float distanceMin = 0.5f;
	private float x;
	private float y;
    private float xP;
    private float yP;
	private Vector3 offset;
	private bool eyy;
    private float dist = 5.0f;
    private bool collided; //For detecting if there are collisions on the camera

	void Start() {
		offset = new Vector3(target.transform.position.x, target.transform.position.y-15, target.transform.position.z+7);
        transform.LookAt(target.transform);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        eyy = true;

        xP += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        yP -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
	}

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (eyy == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                eyy = false;
            }
        }

        if (player.getCurrentState() == Player.State.Passive)
        {
            followCam();
        }
    }
	
	void FixedUpdate() {

		if (player.getCurrentState() == Player.State.Attacking) {
			attackCam ();
        }
	}

	void attackCam() {

        Vector3 velocity = Vector3.zero;
        Vector3 relativePos = attackTarget.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Vector3 newPos = attackTarget.transform.position + (-attackTarget.transform.forward * 4);
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, 0.03f);
        transform.rotation = rotation;
        transform.LookAt(new Vector3(playerTargetSphere.getEnemyPosition().x, attackTarget.transform.position.y, playerTargetSphere.getEnemyPosition().z));
		
	}
	
	void followCam() {
		
		x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
		y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		Vector3 position;
		//dist = Mathf.Clamp(dist - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distance);
        //if (checkMPos(x, y)) { 
		    Vector3 velocity = Vector3.zero;
           // if (collided == true) {
             //   position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position-Vector3.Lerp((transform.position - target.transform.position), offset, 0.5f);
            //} else {
		        position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            //}
		    transform.position = position;
		    transform.rotation = rotation;
            xP = x;
            yP = y;
        //}
				
	}

    void OnTriggerEnter(Collider coll) {//If You collide with an object
        if (coll.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Default")) {
            collided = true;
            //Debug.Log("touchy touchy");
        }
        /*
         * 
         * 
         * transform.position = Vector3.Slerp((transform.position - target.transform.position), offset, 1.0f);
         * 
         * 
         * 
         * 
        */
    }

    void OnTriggerExit(Collider coll) {//If you are no longer colliding with an object
        collided = false;
        //Debug.Log("We outty");
    }

    private bool checkMPos(float x, float y) { //Checks to see if the mouse has been moved.
        if (x == xP && y == yP) {
            return false; //Returns false if the position has not changed (I.E. mouse has not been moved.
        } else {
            return true; //Returns true if the mouse has been moved.
        }
    }
}