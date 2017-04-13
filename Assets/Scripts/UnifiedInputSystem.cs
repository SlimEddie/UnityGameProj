using UnityEngine;
using System.Collections;

public class UnifiedInputSystem : MonoBehaviour {

    public SliderController S_Controller;
    public UnitDataController Unit_Data_Controller;
    private bool UI_Button_Clicked = false;
    public bool SliderIsIterating = false;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonUp(0))
        {
            SliderIsIterating = S_Controller.FetchSliderState();
            if (UI_Button_Clicked)
            {
                // Button will do whatever button does.
            }
            else if (SliderIsIterating)
            {
                S_Controller.StopCurrentSlider();
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! CALCULATE SUCCESS HERE AND GIVE FEEDBACK
                Unit_Data_Controller.SliderFeedBack(S_Controller.FetchSuccesZoneInterval(), S_Controller.FetchSliderValue());
                

                // Send stop signal to slider controller

            }
            UI_Button_Clicked = false;
        }
	}

 

    public void UI_Button_Trigger()
    {
        UI_Button_Clicked = true;
    }

 
}
