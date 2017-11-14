using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLib : MonoBehaviour {

    public List<AudioClip> SoundEffects = new List<AudioClip>();


    public AudioClip FetchClip(string ClipName)
    { 
        int i = 0;
        foreach (AudioClip a in SoundEffects)
        {
            if (a.name == ClipName)
            {
                return SoundEffects[i];    
            }
            i++;
        }
        Debug.Log("Something went wrong when fetching Audio Clip from library.");
        return SoundEffects[0];   
    }
}
