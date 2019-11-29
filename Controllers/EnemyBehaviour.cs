using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyBehaviour : MonoBehaviour
{
    //Controla el comportamiento de los enemigos del juego


    // Enumardo de los diferentes estados de un enememigo.
    public enum EnemyState
    {

        Idle,
        Chasing,

    }
    void Awake()
    {
        // Se guarda la posicion inicial del enemigo para volver a color en su posición inicial
        // si el player se sale de su rango de persecución.
        _enemyInitialPotition = this.transform.position;
    }
    void Start()
    {
        //Refencias al MeshAgent y al Animator
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimator = this.gameObject.GetComponent<Animator>();


    }

    void OnDrawGizmosSelected()
    {
        //Delimita visualmente el area en la cual un enemigo te detecta. (radius)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _radius);

    }


    void Update()
    {
        _playerPosition = _player.transform.position;// Posición del player

        //Se cualcula en cada frame la distancia entre el player y el enemigo
        _distanceToAttack = Vector3.Distance(this.transform.position, _playerPosition);
        //Si esta distancia es menor que el radio de ataque, el enemigo te persigue
        if (_distanceToAttack <= _radius)
        {

            _monsterState = EnemyState.Chasing;
        }
        // Si es mayor el enemigo vuelve a su posicion inicial y se activa la animación de reposo
        else
    
        {
            BackToInitialPosition(); // Coloca al enemigo en su posición inicial
            _monsterState = EnemyState.Idle; // asigna el  estado idle.
        }





            // Evaluando la variable _monsterState se pueden asignar diferentes estados 
            // al enemigo, en cada estado se ejecuta un función que le da un comportamiento 
            //específico al enemigo.
        switch (_monsterState)
        {

            case EnemyState.Chasing: // Persigue al enemigo y lo ataca al llegar a su rango de ataque
                Chasing();
                break;

            case EnemyState.Idle: // Enemigo en reposo
                Idle();
                break;


        }
    }



//Persigue al player si entra en el radio de detección del enemigo.
    void Chasing()
    {

        _enemyAnimator.SetFloat("Velocity", 0.2f);// Animacion Run del enemigo
        _enemyAnimator.SetBool("InRangeToAttack", false);//Desactiva animación attack enemiga.
        //Utilizando la herramienta NavMeshAgent, el enemigo se dirige a la posición del player.
        _agent.SetDestination(_playerPosition); // 
        // Si la distancia entre el enemigo es menor a la distancia de ataque (agentStopDistance)
        //se ejecuta el metodo de atacar del enemigo
        if (_distanceToAttack <= _agent.stoppingDistance)
        {

            Attacking();
        }
    }

    void Idle()
    {
        if (Vector3.Distance(this.gameObject.transform.position, _enemyInitialPotition) <= 0.8999f)

        {

            _enemyAnimator.SetFloat("Velocity", 0f);

        }

    }







//Coloca al enemigo en su posición inicial
    void BackToInitialPosition()
    {
        _agent.SetDestination(_enemyInitialPotition);
    }
    //Desactiva la animacion de Run del enemigo
    // Ejecuta la animación de ataque del enemigo
    void Attacking()
    {

        _enemyAnimator.SetFloat("Velocity", 0f);
        _enemyAnimator.SetBool("InRangeToAttack", true);
    }



//Al ejecutarse la animación de muerte de un enemigo 
//se ejecuta está función que intancia un objeto random de un listado (inventario)
    void dropItems()
    {
        random = Random.Range(0, 2);
        _RandomObject = _Objects[random];
        Instantiate(_RandomObject, this.transform.position, Quaternion.identity);
    }

    public List<GameObject> _Objects;
    private GameObject _RandomObject;
    public int random;
    Animator _enemyAnimator;
    private Vector3 _enemyInitialPotition;
    public float _radius = 4f;
    public float _distanceToAttack;
    private NavMeshAgent _agent;
    public EnemyState _monsterState;

    [SerializeField]
    private ThirdPersonCharacter _player;
    Vector3 _playerPosition;




}


