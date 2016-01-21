using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	GameObject player;
	Rigidbody rigid;
    Animator anim;

    public float health;
    public float attack;

	// Use this for initialization
	void Start () {
	
		player = GameObject.FindGameObjectWithTag ("Player");
		rigid = gameObject.GetComponent<Rigidbody> ();
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	    if ((transform.position - player.transform.position).sqrMagnitude < 50) {
            if ((transform.position - player.transform.position).sqrMagnitude > 3) {
                transform.LookAt(player.transform.position);
                anim.SetBool("Chasing", true);
            }
            else
            {
                anim.SetBool("Chasing", false);
                transform.LookAt(player.transform.position);
            }
        }
        else
        {
            anim.SetBool("Chasing", false);
        }
	}

	void OnTriggerEnter(Collider hitObject) {

		if (hitObject.tag == "PlayerWeapon") {
			rigid.AddForce(player.transform.forward * 100);
			rigid.AddForce(Vector3.up * 200);
		}
	}
}
