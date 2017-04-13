using UnityEngine;
using System.Collections;

[System.Serializable]
public class Unit  {
    public GameObject UnitPrefab;
    public float Cost;
    public float SuccesPercentage;
    public float SliderSpeed;
   

    public float FetchCost()
    {
        return Cost;
    }
	
    public float FetchSuccessPerecentage()
    {
        return SuccesPercentage;
    }
    public float FetchSliderSpeed()
    {
        return SliderSpeed;
    }
}
