using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    public AudioSource AudioSrc;
    public AudioLib AudioL;

    void Awake () {
        AudioL = GameObject.Find("_Audio_Lib").GetComponent<AudioLib>();
    }

  
    public void PlayEffect(string ClipName)
    {
        AudioSrc.clip = AudioL.FetchClip(ClipName);
        AudioSrc.Play();
    }
   

    


}
