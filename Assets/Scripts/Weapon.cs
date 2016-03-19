using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    private Collider col;
    public string weaponName;

    public int baseDamage;

    void Start()
    {
        col = gameObject.GetComponent<BoxCollider>();
        col.enabled = false;
    }

    public void toggleCollider(int toggle)
    {
        switch (toggle)
        {
            case 0:
                col.enabled = false;
                break;
            case 1:
                col.enabled = true;
                break;
        }
    }

    public int getDamage()
    {
        return baseDamage;
    }
}
