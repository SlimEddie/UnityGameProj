using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

    private Camera MyCam;
    public float Scale = 4.21875f;

	void Awake () {

        MyCam = GetComponent<Camera>();
        Camera.main.orthographicSize = Scale / Screen.width * Screen.height;
    }
	
	
}
