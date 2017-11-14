using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour {

    public bool Enemy;
    public LevelBehavior LB;
    public ScoreController Score_Controller;
    public UnitDataController UDC;
    public AudioController AC;
    public bool BonusDmgArmored = false;
    public bool bonusDmgUnArmored = false;
   
    

    public float maxHealth;
    public float health;
    public int armor = 0;
    public int Dmg;
    public int DmgToBase = 3;
    public int DmgBonus = 1;
    public float Cost;
    public bool Dead = false;
    public string AttackName = "MeeleDmg";
    public string AttackAnimName = "Attack";
    public string IdleAnimName = "PeasantAttack";
    public string DeadAnimName = "PeasantAttack";
    public string WalkAnimName = "PeasantAttack";
    public string AttackSoundEffectName = "Cling";

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
        AC = gameObject.GetComponent<AudioController>(); //Getting audio controller
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
        if (Enemy) // Reducing enemy count
        {
            LB.EnemyCount--;
        }
        else
        {
            if (health == maxHealth)
            {
                Score_Controller.IncrementGold(Cost); // ---------------------------------------------------------------------- This has to be redone.
            }
            else if ((maxHealth - health) / maxHealth * Cost > 0)
                Score_Controller.IncrementGold(Mathf.Round((maxHealth - health) / maxHealth * Cost));

            if(!ByWin) UDC.RemoveUnit(this.gameObject);
        }
        Dead = true;
        this.GetComponent<Animator>().Play(DeadAnimName);
    }
    public void ParseAttack(bool BonusArmored, bool BonusUnArmored, int Dmg, int BonusDmg)
    {
        if(armor > 0) // If I am armored do this
        {
            if(BonusArmored) Dmg = Mathf.Clamp(Dmg + BonusDmg - armor,0,9999);
            else Dmg = Mathf.Clamp(Dmg - armor, 0, 9999);
            IncrementHealth(-Dmg);
        }
        else // If I am not armored do this
        {
            if (BonusUnArmored) Dmg = Mathf.Clamp(Dmg + BonusDmg, 0, 9999);
            else Dmg = Mathf.Clamp(Dmg, 0, 9999);
            IncrementHealth(-Dmg);
        }


    }

}
