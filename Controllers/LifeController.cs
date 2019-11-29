using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
public class LifeController : MonoBehaviour
{
    //Controla la vida del los personajes

    void Start()
    {
        _hitPoints = 10;
        _animator = this.gameObject.GetComponent<Animator>();
    }


    void Update()
    {
            //Comprueba la vida de los personajes en cada frame si esta llega a 0 se ejecuta la
            //animacion de muerte de ese personaje.
        if (_hitPoints <= 0)
        {
            _animator.SetBool("isDead", true);

        }


    }
//Se ejecuta en un envento al final de la animación de muerte de un gameobject con lifecontroller.
    public void DestroyGameObject()
    {

        Destroy(this.gameObject);
    }


//Comprueba las pulsaciones de la tecla F de el playerdentro de un triguer,
//si este gamobject tiene el componente lifecontroller resta su vida por cada pulsación.

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && _Character.FisPressed)
        {
            _hitPoints--;
        }

    }






    [SerializeField]
    private ThirdPersonCharacter _Character;
    public int _hitPoints;
    public bool _isDead;
    Animator _animator;
}
