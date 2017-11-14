using UnityEngine;
using System.Collections;

[System.Serializable]
public class Unit  {
    public GameObject UnitPrefab;
    public float Cost;
    public int Units_Awailable;
    public int Units_Max_Awailable;


    public float FetchCost()
    {
        return Cost;
    }
    public int Fetch_Units_Awailable()
    {
        return Units_Awailable;
    }




	
}
