using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBehavior : MonoBehaviour {
    public int MoveDir;
    public float MoveSpeed = 20f;
    public float AttackDelay = 0.3f;
    public bool Fighting = false;
    public List<Transform> Target = new List<Transform>();
    private UnitStats TargetStats;
    private Animator Anim;
    public UnitStats MyStats;
    public GameObject AgroTrigger;

    public GameObject SpecialObjectInstance;
    public Transform SpecialObjectInstanceLocation;

    public float AttackAnimDuration;

	// Use this for initialization
	void Awake () {
        Anim = this.GetComponent<Animator>();


        #region DetectAttackAnimDuration

        //Find Attack anim length
        /*
        RuntimeAnimatorController ac = Anim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == "Archer_Anim_Shoot")        //If it has the same name as your clip
            {
                AttackAnimDuration = ac.animationClips[i].length;
            }
        }
        */
        #endregion
    }
	



	void FixedUpdate () {
       
        //IF NOT DEAD
        if (!MyStats.FetchUnitDeathState())
        {
            //Movement with target
            if (!Fighting)
            {
                Anim.Play(MyStats.WalkAnimName);//Replace with unit anim name array or smth <--------------------- TO DO
                //if (Mathf.Abs((Target[0].transform.position.x - transform.position.x)) > 0.5f)
                    transform.Translate(MoveSpeed * Time.deltaTime *  new Vector2(MoveDir,0));

                if (Target.Count != 0)
                {
                    Vector3 SqrMagVec3 = transform.position - Target[0].transform.position; // Calculate vector difference to determinate square magnitude (they will be on the same Y
                    if (SqrMagVec3.sqrMagnitude < MyStats.FetchAttackRange()) // Check if in attack range.
                    {
                        Fighting = true;
                        Anim.Play(MyStats.IdleAnimName);
                        Invoke("Attack", AttackDelay / 4);
                    }
                }
            }
        }
        //IF DEAD
        else
        {
            AgroTrigger.SetActive(false);

        }
	} // End of fixed update



    public void SetTarget(Transform TargetTransform)
    {
        Target.Add(TargetTransform);
        TargetStats = Target[0].parent.GetComponent<UnitStats>();
    }


    private void Attack()
    {
        if (!MyStats.FetchUnitDeathState() && !TargetStats.FetchUnitDeathState()) //If I am not dead and my enemy is not dead.
        {
            InvokeAttackActions(); // Invokes all actions related to attack - will also reinvoke Attack.
        }
        else if(!MyStats.FetchUnitDeathState() && TargetStats.FetchUnitDeathState()) //I f I am not dead and enemy is dead.
        {
            Target.RemoveAt(0); // Remove target.
            if (Target.Count == 0) // If no more targets left.
            {
                //Debug.Log("No one else to fight");
                Fighting = false; // Stop fighting, this will let unit move further in its lane.
            }
            else
            {
                //Debug.Log("I am in the else");
                while (Fighting)
                {
                    TargetStats = Target[0].parent.GetComponent<UnitStats>();
                    if (!TargetStats.FetchUnitDeathState()) // If there is none dead target, then set it. This will let Attack be reinvoked upon fixedUpdate.
                    {
                        Fighting = false;
                    }
                    else
                    {
                        Target.RemoveAt(0);
                        if (Target.Count == 0) Fighting = false;
                    }
                }
            }
        }
    }



    private void InvokeAttackActions() // added to reduce clutter
    {
        Anim.Play(MyStats.AttackAnimName); // Play attack anim.
        Invoke(MyStats.AttackName, 0f);  // Invoke special function related to attack - responsible for dmg too.
        Invoke("Attack", AttackDelay + AttackAnimDuration); // Reinvoke Attack function after Attack Delay(aka attack speed/frequency) + Attack Animation Duration.
    }





    private void MeeleDmg()
    {

         Invoke("InvokeableDmg", AttackAnimDuration / 2);
        // move it to target detection or smth to prevent constant calls to get component
        
        /*DO SOME WORKING HERE*/


    }

    private void RangedAttack()
    {
        Invoke("SpawnArrow", AttackAnimDuration - 0.2f);
      //  Invoke("InvokeableDmg", AttackAnimDuration);
        // move it to target detection or smth to prevent constant calls to get component

        /*DO SOME WORKING HERE*/


    }

    private void SpawnArrow()
    {
        if (!MyStats.FetchUnitDeathState())Instantiate(SpecialObjectInstance, SpecialObjectInstanceLocation.position, Quaternion.Euler(new Vector3(0, 0,-90)));

    }

    private void InvokeableDmg()
    {
        if(!MyStats.FetchUnitDeathState())Target[0].parent.GetComponent<UnitStats>().IncrementHealth(-MyStats.FetchDmg());
    }




    void Dead()
    {
        AgroTrigger.SetActive(false);

    }
    


}
