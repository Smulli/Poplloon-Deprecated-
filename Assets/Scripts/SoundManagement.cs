using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagement : MonoBehaviour
{
    public static SoundManagement _sfx{get; set;}
    public AudioSource _source;
    public AudioClip[] clips = new AudioClip[3];
    public AudioClip currentSound;
    public int selection;

    void Awake(){
        if(SoundManagement._sfx == null){
            _sfx = this;
        }
        else{Destroy(gameObject);}
    }

    public void PlaySound(){
        _source.PlayOneShot(SetClip());
    }
    
    AudioClip SetClip(){
        return currentSound = clips[selection];
    }

    void Start(){
        _sfx = this;
    }
}
