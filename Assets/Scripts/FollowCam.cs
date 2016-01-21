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
	private Vector3 offset;
	private bool eyy;
    private float dist = 5.0f;

	void Start() {
		offset = new Vector3(target.transform.position.x, target.transform.position.y-15, target.transform.position.z+10);
        transform.LookAt(target.transform);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		playerTargetSphere = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<EnemyTargeting>();
        eyy = true;
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
        Vector3 newPos = attackTarget.transform.position + (-attackTarget.transform.forward * 2.25f);
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, 0.07f);
        transform.rotation = rotation;
        transform.LookAt(new Vector3(playerTargetSphere.getEnemyPosition(playerTargetSphere.getTargetEnemy()).position.x, attackTarget.transform.position.y, playerTargetSphere.getEnemyPosition(playerTargetSphere.getTargetEnemy()).position.z));
		
	}
	
	void followCam() {
		
		x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
		y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		
		dist = Mathf.Clamp(dist - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distance);

        Vector3 velocity = Vector3.zero;
		Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;

        transform.position = position;
		transform.rotation = rotation;
		
	}
}