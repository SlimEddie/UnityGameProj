using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {

    public Database D_Base;
  

    void Awake () {
        try
        {
            D_Base = FileManager.Load<Database>("Database.dat");
        }
        catch //if there is no data saved make new one.
        {
            D_Base = new Database();
        }
        

    }
	


    public bool FetchLevelState(int ID)
    {
        if(D_Base.UlockedLevel <= ID)
            return true;
        return false;
    }
    
    public void SaveTheGame()
    {
        FileManager.Save<Database>(D_Base, "Database.dat");
    }



}
