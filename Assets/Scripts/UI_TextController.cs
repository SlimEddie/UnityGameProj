using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_TextController : MonoBehaviour {

    public Text Score_Gold;
    public Text Score_Hp;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Update Scores
    public void UpdateScore_Gold(float score)
    {
        Score_Gold.text = "" + score;
    }
    //Update Scores
    public void UpdateScore_YourHp(float increment)
    {
        Score_Hp.text = "" + increment;
    }


    /*TO DO*/
    //Animate gold ++ and --
}
