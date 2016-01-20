using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	GameObject player;
	Rigidbody rigid;

	// Use this for initialization
	void Start () {
	
		player = GameObject.FindGameObjectWithTag ("Player");
		rigid = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

	if ((transform.position - player.transform.position).sqrMagnitude < 50) {
        if ((transform.position - player.transform.position).sqrMagnitude > 4) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 2f * Time.deltaTime);
        }
    }
	}

	void OnTriggerEnter(Collider hitObject) {

		if (hitObject.tag == "PlayerWeapon") {
			rigid.AddForce(player.transform.forward * 100);
			rigid.AddForce(Vector3.up * 200);
		}
	}
}
