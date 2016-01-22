using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float movementSpeed = 10;
	public float turningSpeed = 120;
	private Animator anim;
	private Camera mainCam;
	private EnemyTargeting targetTracker;

    private bool dashing;

	public enum State {Passive, Attacking};

	public State currentState;

    //MY CODE REMOVE HERE IF YOU WANT
        public AudioClip hyah1;
        public AudioClip hyah2;
        public AudioClip hyah3;

        private AudioSource source;
    //

	void Start() {
		anim = gameObject.GetComponent<Animator>();
		mainCam = Camera.main;
		targetTracker = gameObject.GetComponentInChildren<EnemyTargeting> ();
		currentState = State.Passive;
        dashing = false;

        //MY CODE
            source = GetComponent<AudioSource>();
        //
	}
	
	void Update() {

		if (currentState == State.Passive) {
			passiveStateBehaviour ();
		} else {
			attackStateBehaviour();
		}

		if ((Input.GetMouseButton (1)) && (targetTracker.getEnemyCount() != 0)) {
			currentState = State.Attacking;
		} else {
			currentState = State.Passive;
		}
	}

	public State getCurrentState() {
		return currentState;
	}

	private void attackStateBehaviour() {

        anim.SetBool("AttackState", true);

		GameObject enemy = targetTracker.getTargetEnemy();

		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Vector3 targetPostition = new Vector3( enemy.transform.position.x, this.transform.position.y, enemy.transform.position.z ) ;
		transform.LookAt( targetPostition ) ;

        transform.LookAt(targetPostition);

        anim.SetInteger("H_Dir", (int)h);
        anim.SetInteger("V_Dir", (int)v);

		if (Input.GetMouseButtonDown (0)) {
            if ((transform.position - enemy.transform.position).sqrMagnitude > 4) {
                dashing = true;
                anim.SetBool("Attacking", true);
            }
            else
            {
                dashing = false;

                //MY CODE REMOVE IT HERE IF YOU WANT TO GET RID OF IT
                    float eyy; //eyyyyy
                    eyy = Random.Range(0,3);
                    if (eyy < 1) {
                        source.PlayOneShot(hyah1, (Random.Range(2.0f,3.0f)));
                    } else if (eyy >= 1 && eyy < 2) {
                        source.PlayOneShot(hyah2, (Random.Range(2.0f, 3.0f)));
                    } else {
                        source.PlayOneShot(hyah2, (Random.Range(2.0f, 3.0f)));
                    }
                //AHAHAHAHAHHAHA HYAHHH HYAH HYAH HYAH HAAAH HYAAAH

                anim.SetBool("Attacking", true);
                anim.SetInteger("AttackChoice", Random.Range(0, 2));
                anim.SetTrigger("Attack");
                anim.SetBool("Attacking", false);
            }
		}

        if (dashing)
        {
            anim.SetBool("Dashing", true);
            if ((transform.position - enemy.transform.position).sqrMagnitude <= 4) {
                dashing = false;
                anim.SetBool("Dashing", false);
                anim.SetInteger("AttackChoice", Random.Range(0, 2));
                anim.SetTrigger("Attack");
                anim.SetBool("Attacking", false);
            }
        }
		
	}

	private void passiveStateBehaviour() {

        anim.SetBool("AttackState", false);

		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		
		Vector3 cameraForward = mainCam.transform.TransformDirection(Vector3.forward);
		cameraForward.y = 0;    
	
		Vector3 cameraRight = mainCam.transform.TransformDirection(Vector3.right);
		Vector3 targetDirection = h * cameraRight + v * cameraForward;
		Vector3 lookDirection = targetDirection.normalized;
		
		if (lookDirection != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation (lookDirection);
		}
		
		if (h != 0 || v != 0) {
			anim.SetBool ("Running", true);
		} else {
			anim.SetBool ("Running", false);
		}

	}
}
