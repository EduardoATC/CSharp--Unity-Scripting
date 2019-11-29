using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

// Se encarga de la lógica del inventario, almacenando los objetos que el player recoge.

    void Start()
    {
        _Objects = new List<int>();
        _Objects.Insert(0, 0);
        _Objects.Insert(1, 0);

    }

//Propiedad de tipo "get" que devuelve la list de objetos (Inventario), asi desde el 
// ThirdPlayerController se puede actualizar el inventario.
    public List<int> GetItemsList
    {
        get { return _Objects; }
    }

    // public List<int> myProperty
    // {

    //     set { _Objects = value; }
    // }

    private List<int> _Objects;
    
    //InteractiveObject interactiveObject;


}
