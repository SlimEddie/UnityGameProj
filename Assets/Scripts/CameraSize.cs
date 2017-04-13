using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

    private Camera MyCam;
    private float Scale = 4.21875f;

	void Awake () {

        MyCam = GetComponent<Camera>();
        Camera.main.orthographicSize = 4.21875f / Screen.width * Screen.height;
    }
	
	
}
