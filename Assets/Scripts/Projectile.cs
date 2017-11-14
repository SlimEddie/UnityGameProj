using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public string TargetTag;
    public int ProjectileDmg = 2;
    public float MoveSpeed = 4.5f;
    public bool PhysicalDmg = true;


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == TargetTag)
        {
            UnitStats Target = other.GetComponent<UnitStats>();//.IncrementHealth(-ProjectileDmg)
            if (PhysicalDmg && Target.armor > 0)
            {
                Target.IncrementHealth(-Mathf.Clamp(ProjectileDmg - Target.armor, 0, 9999)); 
            }
            else
            {
                Target.IncrementHealth(-ProjectileDmg);
            }
            /*
                  Any other type of dmg should be recalculated here.
            */

            DestroyProjectile();


           
        }
    }
    void FixedUpdate()
    {
        transform.Translate(MoveSpeed * Time.deltaTime * new Vector2(0, 1));
    }


    void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }



}
