using UnityEngine;
using System.Collections;

[System.Serializable]
public class Database  {

    public bool[] UlockedLevel = new bool[60];
    

    public Database()
    {
        UlockedLevel[0] = true;
    }
}
