using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameObj : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

}
