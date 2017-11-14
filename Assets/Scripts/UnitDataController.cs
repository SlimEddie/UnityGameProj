using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDataController : MonoBehaviour {

    //public UI_TextController UI_Text_Controller;
    public Persistant_Data Persistant_Data_List;
    public ScoreController Score_Controller;
    public Vector2[] SpawnLocations;
    public List<GameObject> ActiveUnits;
    public List<Sprite> DragableSprites;
    
    

    void Awake()
    {
        Persistant_Data_List = GameObject.Find("_Undestructible_Obj").GetComponent<Persistant_Data>();

    }


    public void SpawnUnit(int PathNumber, int Unit_Id)
    {
       
        GameObject UnitRef = Instantiate(Persistant_Data_List.units[Unit_Id].UnitPrefab, SpawnLocations[PathNumber], Quaternion.identity) as GameObject;
        ActiveUnits.Add(UnitRef);
        Persistant_Data_List.units[Unit_Id].Units_Awailable--;

        
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

    public bool CheckGold(int UnitID)
    {
        if (Score_Controller.FetchGold() >= Persistant_Data_List.units[UnitID].FetchCost())
        {
            return true;
        }
        return false;
    }
    public bool Check_Unit_Awailability(int UnitID)
    {
        if (Persistant_Data_List.units[UnitID].Fetch_Units_Awailable() > 0)
        {
            return true;
        }
        return false;
    }



}
