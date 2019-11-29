using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
//Se encarga de identificar mediante un ID a los objetos que caen cuando muere un enemigo, 
//para asi poder recogerlos y almacenarlos correctamente en el inventario

    public enum ID
    {
        _ManaPot = 0,
        _HpPot = 1

    }



//Cuando se intancia un objeto interactivo si dentro de su trigger hay un collider con
//el tag "player" se asigna un identificador al objeto instanciado. Este identificador 
//permite que cuando el jugador lo recoja se almacene correctamente en el inventario.
    void OnTriggerEnter(Collider PlayerCollider)
    {
     //   _InRangeOfAnObject = true;
        if (PlayerCollider.tag == "Player")
        {
            if (this.gameObject.tag == "manaPot")
            {

                _itemId = ID._ManaPot;
            }
            else if (this.gameObject.tag == "Hp_PoT")
            {

                _itemId = ID._HpPot;
            }

        }

    }


    //propiedad de tipo "get" para recoger el id de un objeto instanciado
    public ID GetId
    {
        get { return _itemId; }
    }

   
    private ID _itemId;
   
}
