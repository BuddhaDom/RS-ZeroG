using System;
using UnityEngine;

public class Data_Saver : MonoBehaviour
{


    public GameObject last_ancor;

    public AudioSource audio_source;
    public AudioClip collision_sound;
    public AudioClip trap_sound;

    private void Awake()
    {
       audio_source = GameObject.FindGameObjectWithTag("audio_source").GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.DataSaver = this;
    }
}
