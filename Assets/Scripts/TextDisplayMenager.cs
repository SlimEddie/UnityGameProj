using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextDisplayMenager : MonoBehaviour {

    public Text TextSlave;

    public string[] TextArr;
    private string CurrentWordString = "";
    private bool CurrentlyRunning = false;
    private List<int> DisplayQueue = new List<int>();

    // Use this for initialization
    void Start () {
        //ToggleText(0); // This is how it worx
        //ToggleText(1); // This is how it worx
        //ToggleText(2); // This is how it worx
    }
	


    //StartCoroutine(Example());
    IEnumerator DisplayText(int index)
    {
        for (int i = 0; i < TextArr[index].Length; i++)
        {
            CurrentWordString += TextArr[index][i];
            TextSlave.text = CurrentWordString;
            yield return new WaitForSecondsRealtime(0.03f);
        }
        Invoke("RemoveText", 2 + (TextArr[index].Length * 0.03f));
    }



    public void ToggleText(int index)
    {
        if (!CurrentlyRunning) // If there is nothing running launch the request
        {
            CurrentlyRunning = true;
            CurrentWordString = "";
            StartCoroutine(DisplayText(index));
        }
        else // Queue the text request...
        {
            DisplayQueue.Add(index);
        }

    }


    public void RemoveText()
    {
        TextSlave.text = "";
        CurrentlyRunning = false;
        CurrentWordString = "";
        if (DisplayQueue.Count > 0)
        {
            ToggleText(DisplayQueue[0]);
            DisplayQueue.RemoveAt(0);
        }
    }





}
