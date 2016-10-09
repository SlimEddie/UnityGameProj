using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SliderIterate : MonoBehaviour {
    public Slider Slider;
    public float Speed = 0.5f;
    private float dir = 1;
    public bool Slide = true;


    // Use this for initialization
    void Start () {
        //Slider.interactable = false;  Changed this in the editor.
        //Debug.Log(Slider.value);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
            Slide = false;

        //Debug.Log(Slider.value);
        if (Slide)
        {
            Slider.value = Mathf.Clamp01(Slider.value + Time.deltaTime * dir * Speed);
            if ((Slider.value == Slider.maxValue) || (Slider.value == Slider.minValue)) dir = -dir;
        }
        //if ((Slider.value == Slider.maxValue) || (dir == 1)) dir = -dir;
        //else if ((Slider.value == Slider.minValue) && (dir == -1)) dir = -dir;
    }
}
