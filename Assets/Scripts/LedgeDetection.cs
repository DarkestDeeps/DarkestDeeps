using UnityEngine;
using System.Collections;

public class LedgeDetection : MonoBehaviour {

    private bool ledgeDetected;

    float ledgeY;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ledge")
        {
            ledgeDetected = true;
            ledgeY = other.transform.position.y;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ledge")
        {
            ledgeDetected = false;
        }
    }

    public bool canGrabLedge()
    {
        return ledgeDetected;
    }

    public float getLedgeY()
    {
        return ledgeY;
    }
}
