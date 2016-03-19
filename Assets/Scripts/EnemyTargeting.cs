using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTargeting : MonoBehaviour {

	List<GameObject> enemyList = new List<GameObject>();

	void OnTriggerEnter(Collider enemy) {

		if (enemy.tag == "Enemy") {
			enemyList.Add(enemy.gameObject);
		}
	}

	void OnTriggerExit(Collider enemy) {
		
		if (enemy.tag == "Enemy") {
			enemyList.Remove(enemy.gameObject);
		}
	}

	public GameObject getTargetEnemyObject() {
		return enemyList[0];
	}

    public Enemy getTargetEnemy()
    {
        return enemyList[0].GetComponent<Enemy>();
    }

	public Vector3 getEnemyPosition() {
        return getTargetEnemy().transform.position;
	}

	public int getEnemyCount() {
		return enemyList.Count;
	}
}