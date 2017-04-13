using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public string TargetTag;
    public int ProjectileDmg = 2;
    public float MoveSpeed = 4.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == TargetTag)
        {
            other.GetComponent<UnitStats>().IncrementHealth(-ProjectileDmg);
            Destroy(this.gameObject);
        }
    }
    void FixedUpdate()
    {
        transform.Translate(MoveSpeed * Time.deltaTime * new Vector2(0, 1));


    }
}
