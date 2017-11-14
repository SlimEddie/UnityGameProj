using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public UI_TextController UI_Text_Controller;
    public float Gold = 20f;
    public float YourHp = 30f;
    public LevelBehavior Level_Event_System;

    public GameObject HP_Anim_Obj;
    public GameObject Gold_Anim_Obj;
    public Text HP_Anim_Text_Obj;
    public Text Gold_Anim_Text_Obj;

    private Animator Gold_Anim_Obj_Animator;
    private Animator HP_Anim_Obj_Animator;



    void Start () {
        Gold_Anim_Obj_Animator = Gold_Anim_Obj.GetComponent<Animator>();
        HP_Anim_Obj_Animator = HP_Anim_Obj.GetComponent<Animator>();
        UI_Text_Controller.UpdateScore_Gold(FetchGold());
        UI_Text_Controller.UpdateScore_YourHp(FetchYourHp());
    }
	


    public float FetchGold()
    {
        return Gold;
    }
    public float FetchYourHp()
    {
        return YourHp;
    }

    // Should only be called if there is enough gold
    public void IncrementGold(float ammount)
    {
        Gold_Anim_Text_Obj.text = Mathf.Round(ammount).ToString();
        Gold_Anim_Obj.SetActive(true);
        Gold_Anim_Obj_Animator.Play("Score_Increment");

        Gold = Mathf.Round(Gold+ammount);
        UI_Text_Controller.UpdateScore_Gold(FetchGold());
    }
    public void IncrementHp(float ammount)
    {
        if (HP_Anim_Obj_Animator.GetCurrentAnimatorStateInfo(0).IsName("Score_Increment_Hp"))
        {
            HP_Anim_Text_Obj.text = (float.Parse(HP_Anim_Text_Obj.text) + ammount).ToString();
            Debug.Log("Adding");
        }
        else
        HP_Anim_Text_Obj.text = Mathf.Round(ammount).ToString();
        HP_Anim_Obj.SetActive(true);
        HP_Anim_Obj_Animator.Play("Score_Increment_Hp");
        YourHp = Mathf.Round(YourHp + ammount);
        UI_Text_Controller.UpdateScore_YourHp(FetchYourHp());
        if(YourHp < 1)
        {
            Level_Event_System.GameOver = true;
            Level_Event_System.Finished();
        }
    }




}
