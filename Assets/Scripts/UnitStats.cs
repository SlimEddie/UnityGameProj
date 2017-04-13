using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour {

    public bool Enemy;
    public LevelBehavior LB;
    public ScoreController Score_Controller;
    public UnitDataController UDC;
    
    public float maxHealth;
    public float health;
    public int Dmg;
    public int DmgToBase = 3;
    public float Cost;
    public bool Dead = false;
    public string AttackName = "MeeleDmg";
    public string AttackAnimName = "Attack";
    public string IdleAnimName = "PeasantAttack";
    public string DeadAnimName = "PeasantAttack";
    public string WalkAnimName = "PeasantAttack";

    public float AttackRange = 0.25f;


	// Use this for initialization
	void Awake () {
        maxHealth = health;
        if (Enemy)
        {
            LB = GameObject.Find("_Level_Events").GetComponent<LevelBehavior>();
        }
        else
        {
            UDC = GameObject.Find("_ScriptSlave").GetComponent<UnitDataController>();
        }
        Score_Controller = GameObject.Find("_ScriptSlave").GetComponent<ScoreController>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public float FetchHealth()
    {
        return health;
    }
    public void IncrementHealth(int increment)
    {
        health += increment;
        if(health <= 0 && !Dead )
        {
            Kill();
            /*Do dead stuff here...*/
        }

    }
    public int FetchDmg()
    {
        return Dmg;
    }
    public bool FetchUnitDeathState()
    {
        return Dead;

    }
    public float FetchAttackRange()
    {
        return AttackRange;
    }
    public void Kill()
    {
        if (Enemy)
        {
            LB.EnemyCount--;
            Score_Controller.IncrementGold(1);
        }
        else
        {
            UDC.RemoveUnit(this.gameObject);
        }        
        Dead = true;
        this.GetComponent<Animator>().Play(DeadAnimName);
    }
    public void Kill(bool MapExit, bool ByWin)
    {
        if (Enemy)
        {
            LB.EnemyCount--;
        }
        else
        {
            if (health == maxHealth)
            {
                Score_Controller.IncrementGold(Cost);
            }
            else if ((maxHealth - health) / maxHealth * Cost > 0)
                Score_Controller.IncrementGold(Mathf.Round((maxHealth - health) / maxHealth * Cost));

            if(!ByWin) UDC.RemoveUnit(this.gameObject);
        }
        Dead = true;
        this.GetComponent<Animator>().Play(DeadAnimName);
    }


}
