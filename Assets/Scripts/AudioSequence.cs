using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSequence : MonoBehaviour
{
    public AudioClip introBgm;
    public AudioClip ghostNormalBgm;
    AudioSource music;
    void Awake()
    {
        music = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        music.loop = false;
        music.clip = introBgm;
        music.Play();
        StartCoroutine(SwitchBGM(3f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator SwitchBGM(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        music.loop = true;
        music.clip = ghostNormalBgm;
        music.Play();
    }
}
