using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDataController : MonoBehaviour {

    //public UI_TextController UI_Text_Controller;
    public SliderController S_Controller;
    public ScoreController Score_Controller;
    public List<Unit> Units = new List<Unit>();
    public int CurrentUnit = 9;
    public Vector2[] SpawnLocations;
    public List<GameObject> ActiveUnits;
    


    /*Unit UI button called functions reside here*/
    public void SetThisUnit(int myInt)
    {
        //Check if not clicking the same unit
        if (S_Controller.FetchCurrentSliderNumber() != 1)
        {
            CurrentUnit = myInt;
            //If all unit selection conditions are met
            if (CheckGold())
            {
                //Change zone width 
                S_Controller.ReorderSliderBackground(Units[CurrentUnit].FetchSuccessPerecentage());
                S_Controller.SetSliderSpeed(Units[CurrentUnit].FetchSliderSpeed());
                //Start slider
                S_Controller.StartSlider(0);

            }
        }
    }
    //Check if enough gold
    private bool CheckGold()
    {
       if(Score_Controller.FetchGold() >= Units[CurrentUnit].FetchCost())
        {
            return true;
        }
        return false;
    }
    //In case of successful slider stop call this
    public void SliderFeedBack(Vector2 SuccesZoneInterval, float SliderResult)
    {
        if((SliderResult >= SuccesZoneInterval.x && SliderResult <= SuccesZoneInterval.y) || S_Controller.FetchCurrentSliderNumber() == 1)
        {
            // Do stuff that happens when success
            // LAUNCH SECOND SLIDER
            switch (S_Controller.FetchCurrentSliderNumber())
            {
                case 0:
                    {
                        Score_Controller.IncrementGold(-Units[CurrentUnit].FetchCost());
                        S_Controller.StartSlider(1);
                        //Launch second slider.
                        //Enable then launch second slider
                        break;
                    }

                case 1:
                    {

                        int TempPathNumber = CalculatePathNumber(SliderResult);
                        GameObject UnitRef = Instantiate(Units[CurrentUnit].UnitPrefab, SpawnLocations[TempPathNumber], Quaternion.identity) as GameObject;
                        ActiveUnits.Add(UnitRef);
                        //UnitRef.GetComponent<SpriteRenderer>().sortingOrder = (TempPathNumber+1)* (TempPathNumber + 1);
                        S_Controller.CurrentSlider = 0;
                        //Spawn mob
                        break;
                    }
            }
        }
        else
        {
            //In case of failure do this...
            Score_Controller.IncrementGold(-Units[CurrentUnit].FetchCost()/2);

        }
        
       
        // add bool to see if succeeded etc .. remove gold on success
        // Remove half of a gold on fail
       // Debug.Log(S_Controller.FetchSliderValue());

    }

    private int CalculatePathNumber(float SliderResults)
    {
        if (SliderResults <= 0.333f)
        {
            return 2;
        }
        else if (SliderResults <= 0.666f)
        {

            return 1;
        }
        else {

            return 0;
        }

    }

    public void RemoveUnit(GameObject UnitRef)
    {
        ActiveUnits.Remove(UnitRef);
    }
    public void RemoveAllRemainingUnits()
    {
        for(int i= 0; i < ActiveUnits.Count; i++)
        {
            ActiveUnits[i].GetComponent<UnitStats>().Kill(true,true);
        }

    }





}
