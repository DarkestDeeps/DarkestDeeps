using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public GameObject target;
    public float rotateSpeed = 5;
    public float xSpeed = 115.0f;
    public float ySpeed = 50.0f;
	public float distance;
    public float distanceMin = 0.5f;

    private Vector3 offset;
    private float x;
    private float y;
    private bool eyy;
    private float dist = 5.0f;
	
	void Start() {
        offset = new Vector3(target.transform.position.x, target.transform.position.y-15, target.transform.position.z+10);
        transform.LookAt(target.transform);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        eyy = true;
	}
	
	void LateUpdate() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (eyy == true) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                eyy = false;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                eyy = true;
            }
        }

        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        //y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        dist = Mathf.Clamp(dist - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distance);

        RaycastHit hit;
        if (Physics.Linecast(target.transform.position, transform.position, out hit)) {
            dist -= hit.distance;
        }
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;

		transform.position = position;
		transform.rotation = rotation;
	}
}