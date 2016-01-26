using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public string weaponName;
    public int damage;

    public int getDamage()
    {
        return damage;
    }
}
