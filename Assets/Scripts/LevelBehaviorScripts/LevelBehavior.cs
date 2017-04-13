using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelBehavior : MonoBehaviour {
    /*
     Events are invoked one after another OR by a trigger of some kind... 
     */
    public UnitDataController UDC;
    public ScoreController Score_Controller;
    public int EnemyCount = 0;

    public GameObject GameOverScreen;
    public GameObject VictoryScreen;

    public Animator[] Indicators;
    
    public Vector2[] SpawnLocations;

    public TextDisplayMenager TDM;
    public LevelDatabase[] L_D_Base;
    private int CurrentEventIndex = 0;

    public bool GameOver = false;
   

    void Start()
    {
        if ((L_D_Base.Length > 0) && (L_D_Base.Length > CurrentEventIndex))
        {
            Invoke("InvokeEvent", L_D_Base[CurrentEventIndex].Event_Timing);
            if (L_D_Base[CurrentEventIndex].Event_Timing > 2 && L_D_Base[CurrentEventIndex].EnemyUnits.Length > 0) Invoke("flashIndicators", L_D_Base[CurrentEventIndex].Event_Timing - 2);
        }

    }









    
     private void InvokeEvent()
     {
        if (!GameOver)
        {
            TextDisplay(); // Will check internally if there is a need to display text.
            SpawnEnemyUnits(); // Will check internally if there are any units to spawn.
            if (L_D_Base[CurrentEventIndex].Invoke_Functionality) Invoke(L_D_Base[CurrentEventIndex].Function_Name, 0f);



            if (CurrentEventIndex + 1 < L_D_Base.Length)  // ALSO ADD CHECK IF IT IS ALSO TIMED EVENT OR CONDITIONAL
            {
                Invoke("InvokeEvent", L_D_Base[CurrentEventIndex].Event_Timing);
                if (L_D_Base[CurrentEventIndex].Event_Timing > 2 && L_D_Base[CurrentEventIndex].EnemyUnits.Length > 0) Invoke("flashIndicators", L_D_Base[CurrentEventIndex].Event_Timing - 2);
            }
            else
            {


            }
            CurrentEventIndex++;
        }
     }
     
    void TextDisplay()
    {
        if (L_D_Base[CurrentEventIndex].Acompany_TextEvent)
            TDM.ToggleText(L_D_Base[CurrentEventIndex].Event_TextEventIndex);
    }
    void SpawnEnemyUnits()
    {
        //CHECK IF THEY HAVE TARGET ON SOME LIKE OR SMTH - DO THIS LATER WHEN THIS AT LEAST WORKS
        for(int i = 0; i < L_D_Base[CurrentEventIndex].EnemyUnits.Length;i++)
        {
            EnemyCount++;
            Instantiate(L_D_Base[CurrentEventIndex].EnemyUnits[i], SpawnLocations[L_D_Base[CurrentEventIndex].lanes[i]], Quaternion.Euler(new Vector3(0,180,0)));

        }
    }



    public void Finished()
    {
        if(EnemyCount != 0 && !GameOver)
        {
            Invoke("Finished", 1f);
        }
        else if (Score_Controller.YourHp < 1)
        {
            UDC.RemoveAllRemainingUnits();
            // trigger victory
            GameOverScreen.SetActive(true);
        }
        else
        {
            UDC.RemoveAllRemainingUnits();
            VictoryScreen.SetActive(true);
        }


    }
    private void flashIndicators()
    {
        for (int i = 0; i < L_D_Base[CurrentEventIndex].EnemyUnits.Length; i++)
        {
            Indicators[L_D_Base[CurrentEventIndex].lanes[i]].Play("Indicator");
        }

    }

}
