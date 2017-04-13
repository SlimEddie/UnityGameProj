using UnityEngine;
using System.Collections;

public class PlayerHpCollisionDetection : MonoBehaviour {

    public string OposingFactionTag;
    public ScoreController S_Controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D coll) {
        if(coll.tag == OposingFactionTag)
        {
            if(coll.tag == "Enemy_Collider")S_Controller.IncrementHp(-coll.gameObject.GetComponent<UnitStats>().DmgToBase);
            coll.gameObject.GetComponent<UnitStats>().Kill(true,false);
        }
	}
}
