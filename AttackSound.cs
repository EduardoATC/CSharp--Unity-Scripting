using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSound : MonoBehaviour
{
    //Reproduce el audio de grito al final de la animación de ataque del player.

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip audioClip;


    void Shout()
    {

        audioSource.PlayOneShot(audioClip);
    }
}
