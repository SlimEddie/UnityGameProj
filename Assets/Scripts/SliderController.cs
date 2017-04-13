using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SliderController : MonoBehaviour {

    public Slider[] slider;
    public float Speed = 0.5f;
    private float dir = 1;
    public bool EnableSlide = false;
    public int CurrentSlider = 0;
    private float SuccessZoneWidth = 0;


    public RectTransform SuccessZone;
    private Vector2 SuccessZonePosition;



    // Use this for initialization
    void Start () {
        //Slider.interactable = false;  Changed this in the editor.
        //Debug.Log(Slider.value);
    }

    // Update is called once per frame
    void Update()
    {


    // Slider sliding mechanism
        if (EnableSlide)
        {
            slider[CurrentSlider].value = Mathf.Clamp01(slider[CurrentSlider].value + Time.deltaTime * dir * Speed);
            if ((slider[CurrentSlider].value == slider[CurrentSlider].maxValue) || (slider[CurrentSlider].value == slider[CurrentSlider].minValue)) dir = -dir;
        }

    }

  

    //Function to activate certain slider
    public void StartSlider(int index)
    {
        
      slider[index].gameObject.SetActive(true);
      CurrentSlider = index;
      EnableSlide = true;
        
    }
    //Stops and disables current slider
    public void StopCurrentSlider()
    {
       slider[CurrentSlider].gameObject.SetActive(false);
       EnableSlide = false;
    }
    //Fetch current slider state 
    public bool FetchSliderState()
    {
        return EnableSlide;
    }
    //Fetch slider number
    public int FetchCurrentSliderNumber()
    {
        return CurrentSlider;
    }
    //Fetch slider value
    public float FetchSliderValue()
    {
        return slider[CurrentSlider].value;
    }

    public void ReorderSliderBackground(float SuccessZonePercentage)
    {
        SuccessZoneWidth = 300 * SuccessZonePercentage / 100;
        SuccessZone.sizeDelta = new Vector2(SuccessZoneWidth, 20);
        SuccessZonePosition = new Vector2(Random.Range(SuccessZoneWidth / 2, 300 - SuccessZoneWidth/ 2), 0);
        SuccessZone.anchoredPosition = SuccessZonePosition;


    }
    //Set Slider speed
    public void SetSliderSpeed(float speed)
    {
        Speed = speed;
    }
    //Fetch success zone position
    public Vector2 FetchSuccesZoneInterval()
    {
    
        return new Vector2((300 - (300 - SuccessZonePosition.x) - SuccessZoneWidth / 2) / 300, (SuccessZonePosition.x + SuccessZoneWidth / 2)/300);
    }

}
