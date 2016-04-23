using UnityEngine;
using System.Collections;

public class GroundDetection : MonoBehaviour {

    public bool grounded;
    public SphereCollider grounder;

    void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    void OnTriggerExit(Collider other)
    {
        grounded = false;
    }

    public bool isGrounded()
    {
        return grounded;
    }
}
