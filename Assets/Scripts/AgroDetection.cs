using UnityEngine;
using System.Collections;

public class AgroDetection : MonoBehaviour {

    public UnitBehavior UB;
    public string TargetTag;

	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == TargetTag) UB.SetTarget(other.transform);	
	}
}
