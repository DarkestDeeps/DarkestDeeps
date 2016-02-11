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

	public GameObject getTargetEnemy() {
		return enemyList[0];
	}

	public Vector3 getEnemyPosition() {
        return getTargetEnemy().transform.position;
	}

	public int getEnemyCount() {
		return enemyList.Count;
	}

    public void broadcastIntent(Player.Intent intent)
    {
        Enemy enemy = getTargetEnemy().GetComponent<Enemy>();
        enemy.receiveIntent(intent);
    }
}