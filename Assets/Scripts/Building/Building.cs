using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building {

    public int MaxLevel;                                                        //Max building level
    public List<float> BuildTime = new List<float>();                           //Time it takes to build/upgrade building - seconds
    public Transform BuldingLocation;                                           //Building's location on the screen
    public List<int>  UnitsProvided_Count = new List<int>();                    //Max ammount of units provided by building
    public List<GameObject> UnitsProvided_Obj = new List<GameObject>();         //Units that building provides at the given level
    public List<int> UpgradeCost = new List<int>();                             //Bulding upgrade cost in gold
    public List<int> UpgradeCostSpecial = new List<int>();                      //Upgrade cost of special curency


    public Building()
    {

    


    }





}
