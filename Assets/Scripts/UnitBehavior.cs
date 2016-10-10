using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBehavior : MonoBehaviour {
    public int MoveDir;
    public float MoveSpeed = 20f;
    public float AttackDelay = 0.3f;
    public float AtackDuration = 0.5f;
    public bool HasTarget = false;
    public bool Fighting = false;
    public List<Transform> Target = new List<Transform>();
    private Animator Anim;

    public GameObject DmgTrigger;

	// Use this for initialization
	void Awake () {
        Anim = this.GetComponent<Animator>();


    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // Movement without target
        if (!HasTarget)
        {
            Anim.SetBool("Walking", true);
            Anim.SetBool("Idle", false);
            transform.Translate(MoveSpeed * Time.deltaTime * new Vector2(MoveDir, 0f));
        }
        //Movement with target
        else if (HasTarget && Target[0]!= null && !Fighting)
        {
            Anim.SetBool("Walking", true);
            Anim.SetBool("Idle", false);
            if (Mathf.Abs((Target[0].transform.position.x - transform.position.x)) > 0.5f)
                transform.Translate(MoveSpeed * Time.deltaTime * new Vector2(Mathf.Sign((Target[0].transform.position.x - transform.position.x)), 0f));
            if (Mathf.Abs((Target[0].transform.position.y - transform.position.y)) > 0.02f)
                transform.Translate(MoveSpeed * Time.deltaTime * new Vector2(0f, Mathf.Sign((Target[0].transform.position.y - transform.position.y))));
            Vector3 SqrMagVec3= transform.position - Target[0].transform.position;
            if (SqrMagVec3.sqrMagnitude < 0.251f)
            {
                Fighting = true;
                Invoke("Attack", 0.1f);
                Anim.SetBool("Idle", true);
                Anim.SetBool("Walking", false);
            }
           
        }

	}
    public void SetTarget(Transform TargetTransform)
    {
        Target.Add(TargetTransform);
        HasTarget = true;

    }

    private void Attack()
    {
        DmgTrigger.SetActive(true);

        Invoke("Attack", 0.1f + AtackDuration);
    }



    void OnTriggerEnter2d(Collider2D OtherColl)
    {
        //take dmg
        if (OtherColl.tag == "dmg")
        {
            //take dmg here
            Destroy(OtherColl.gameObject);
        }
    }


}
