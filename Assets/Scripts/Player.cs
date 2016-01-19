using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float movementSpeed = 10;
	public float turningSpeed = 120;
	private Animator anim;
	private Camera mainCam;
	
	void Start() {
		anim = gameObject.GetComponent<Animator>();
		mainCam = Camera.main;
	}
	
	void Update() {
		
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

		if (h > 0 || h < 0 || v > 0 || v < 0) {
			anim.SetBool ("Running", true);
		} else {
			anim.SetBool ("Running", false);
		}
		
		if (Input.GetMouseButtonDown (0)) {
			anim.SetTrigger("Attack");
		}
		
	}
}
