using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimOnEnable : MonoBehaviour {

    public Animator TargetAnimator;
         

    void OnEnable()
    {
        TargetAnimator.Play("UI_Spawner_SlideIn");
    }
}
