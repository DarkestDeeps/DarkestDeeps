using UnityEngine;
using System.Collections;

public class ObstacleHandler : MonoBehaviour
{

    public bool potentialVault;
    public Vector3 beamPosition;

    //init variables
    void Start()
    {
        potentialVault = false;
        beamPosition = new Vector3(0, 0, 0);
    }

    //if a beam is in the collider, can vault
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Beam")
        {
            potentialVault = true;
            beamPosition = other.gameObject.transform.position;
        }
    }

    //when beams leam, cant vault
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Beam")
        {
            potentialVault = false;
            beamPosition = beamPosition = new Vector3(0, 0, 0); ;
        }
    }
}
