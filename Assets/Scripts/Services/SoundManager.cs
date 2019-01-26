using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public struct SoundBank
    {
        [SerializeField]
        public AudioClip playerAttack;
        
        [SerializeField]
        public AudioClip playerDamage;
        
        [SerializeField]
        public AudioClip doorClosing;
    }
    
    [SerializeField]
    private SoundBank soundBank;
    
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
            
        EventBus.OnPlayerAttack.evt += () => { PlaySingle(soundBank.playerAttack); };
        EventBus.OnPlayerDamage.evt += () => { PlaySingle(soundBank.playerDamage); };
        EventBus.OnExitHouse.evt    += () => { PlaySingle(soundBank.doorClosing); };
    }
    
    public void PlaySingle(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play ();
    }

}
