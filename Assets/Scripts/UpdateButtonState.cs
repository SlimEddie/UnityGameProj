using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpdateButtonState : MonoBehaviour {


    public int LevelId;
 
    
	void Start () {
        this.GetComponent<Button>().interactable = GameObject.Find("_ScriptSlave").GetComponent<DataManager>().FetchLevelState(LevelId);
	}

    public void LoadLevel()
    {
        SceneManager.LoadScene(LevelId + 1);
    }
	
}
