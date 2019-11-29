using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.ThirdPerson
{

    // Controlador del player

    // Código propio más abajo
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonCharacter : MonoBehaviour
    {

        [SerializeField] float m_MovingTurnSpeed = 360;
        [SerializeField] float m_StationaryTurnSpeed = 180;
        [SerializeField] float m_JumpPower = 12f;
        [SerializeField] float m_ForwardSpeed = 1f;
        [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
        [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField] float m_MoveSpeedMultiplier = 1f;
        [SerializeField] float m_AnimSpeedMultiplier = 1f;
        [SerializeField] float m_GroundCheckDistance = 0.1f;

        Rigidbody m_Rigidbody;
        Animator m_Animator;
        bool m_IsGrounded;
        float m_OrigGroundCheckDistance;
        const float k_Half = 0.5f;
        float m_TurnAmount;
        float m_ForwardAmount;
        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;
        bool m_Crouching;

        //////////////////////////////////////MY CODE///////////////////////////////////////////////////////////////////////////////////////////////////
        private State _CurrentState = State.Idle;
        public enum State // Diferentes estados del personaje.
        {
            Idle,
            Moving,
            Lifting

        }
        void Update()
        {
            _KPressed = Input.GetButtonDown("K"); //input K
            _FPressed = Input.GetButtonDown("F"); //input F


        //Evalúa en cada frame el estado del personaje y ejecuta lo correspondiente a cada estado.
            switch (_CurrentState)
            {
                case State.Lifting:

                    if (_RockInRange)
                    {
                        Lift();
                    }
                    else
                    {
                        _CurrentState = State.Idle;
                    }
                    break;

                case State.Moving:

                    if (_PillarInRange)
                    {
                        MoveElement();
                    }
                    else
                    {
                        _CurrentState = State.Idle;
                    }

                    break;
                default:
                    Idle();
                    break;
            }
        }


        public void Lift() // Función para elevar rocas.
        {
            _rockBehaviour.ReactToSkill();
        }
        public void MoveElement() //Función para mover pilares.
        {
            _pillarBehaviour.ReactToSkill();
        }


        //Evalua si el player esta cerca de algo con lo que pueda interactura
        //si no lo esta, pasa a estado idle.
        public void Idle() 
        {
            if (_KPressed)
            {
                if (_pillarBehaviour == null && _rockBehaviour == null && _NPC == null)
                {
                    Debug.Log("No hay nada con que interactuar");
                }
                _CurrentState = State.Idle;
            }
        }


// Situaciones en la que actúa el player cuando entra dentro de un trigger.
        void OnTriggerStay(Collider Collider)
        {
                    //Referencias
            _pillarBehaviour = Collider.GetComponent<PillarBehaviour>();
            _rockBehaviour = Collider.GetComponent<RockBehaviour>();
            _NPC = Collider.GetComponent<NPC>();
            _dialogueTrigger = Collider.GetComponent<DialogueTrigger>();
            _enemyBehaviour = Collider.GetComponent<EnemyBehaviour>();
            _interactiveObject = Collider.GetComponent<InteractiveObject>();


// Si el el gameobject cuyo trigger entró el personaje tiene el componente PillarBehaviour
// y la tecla para interacturar está pulsada el estado del personaje cambia a moving
//y ejecuta lo que corresponda a ese estado (Switch anterior).
            if (_pillarBehaviour)
            {

                if (_KPressed)
                {
                    _CurrentState = State.Moving;
                    _PillarInRange = true;
                }


            }

            // Si tiene un RockBehaviour cambia a estado Lifting
            else if (_rockBehaviour)
            {
                if (_KPressed)
                {
                    _CurrentState = State.Lifting;
                    _RockInRange = true;
                }
            }

            // Si tiene un componente NPC, inicia una conversación
            else if (_NPC)
            {
                if (_KPressed)
                {
                    _dialogueTrigger.TriggerDialogue();
                }

            }
            //Si tiene un componente EnemyBehaviour y se pulsa la F se activa la animación
            // de ataque.
            else if (_enemyBehaviour)
            {
                if (_FPressed)
                {
                    _animator.SetBool("Attacking", true);

                }
                else
                // si no está pulsada la F se desactiva la animación
                {
                    _animator.SetBool("Attacking", false);
                }

            }
        // Si el gameobject tiene un InteractiveObject component
            else if (_interactiveObject)
            {
         // Si la tecla para interactuar está pulsada                    
                if (_KPressed)
                {              
                    // Segun su ID lo añade al inventario y  destruye el gameobject 
                   // (simulando) que a recogido el objeto    
                    if (((int)(_interactiveObject.GetId)) == (0))
                    {                   
                        _inventory.GetItemsList[0] += 1;
                        _manaText.text = _inventory.GetItemsList[0].ToString();
                        Destroy(_interactiveObject.gameObject);
                    }

                    else if (((int)(_interactiveObject.GetId)) == (1))
                    {       
                        _inventory.GetItemsList[1] += 1;  
                        _HpText.text = _inventory.GetItemsList[1].ToString();                               
                        Destroy(_interactiveObject.gameObject);
                    }
                }
            }
        }
// Propiedad "get"  para objeter el valor de FisPressed
        public bool FisPressed
        {
            get { return _FPressed; }
        }
//Reset de los valores cuando el player sale del trigger del gameobject
        void OnTriggerExit(Collider Collider)
        {
            _pillarBehaviour = null;
            _rockBehaviour = null;

            if (_rockBehaviour == null)
            {
                _RockInRange = false;
            }

            if (_pillarBehaviour == null)
            {
                _PillarInRange = false;
            }

        }
        private PillarBehaviour _pillarBehaviour;
        private RockBehaviour _rockBehaviour;
        private bool _PillarInRange;
        private bool _RockInRange;
        private bool _KPressed;
        private bool _FPressed;
        private NPC _NPC;
        private DialogueTrigger _dialogueTrigger;
        private EnemyBehaviour _enemyBehaviour;
        Animator _animator;
        Inventory _inventory;
        InteractiveObject _interactiveObject;
        [SerializeField]
        public Text _manaText;
        [SerializeField]
         public Text _HpText;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void Start()
        {

                //
            //MY CODE
            //Referencias
            _inventory = this.GetComponent<Inventory>();
            _animator = this.GetComponent<Animator>();
            _manaText.text = "0";
            _HpText.text ="0";
            //




            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;

            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            m_OrigGroundCheckDistance = m_GroundCheckDistance;
        }


        public void Move(Vector3 move, bool crouch, bool jump)
        {

            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_TurnAmount = Mathf.Atan2(move.x, move.z);
            m_ForwardAmount = move.z;

            ApplyExtraTurnRotation();

            // control and velocity handling is different when grounded and airborne:
            if (m_IsGrounded)
            {
                HandleGroundedMovement(crouch, jump);
            }
            else
            {
                HandleAirborneMovement();
            }

            // ScaleCapsuleForCrouching(crouch);
            //PreventStandingInLowHeadroom();

            // send input and other state parameters to the animator
            UpdateAnimator(move);
        }


        void ScaleCapsuleForCrouching(bool crouch)
        {
            if (m_IsGrounded && crouch)
            {
                if (m_Crouching) return;
                m_Capsule.height = m_Capsule.height / 2f;
                m_Capsule.center = m_Capsule.center / 2f;
                m_Crouching = true;
            }
            else
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                    return;
                }
                m_Capsule.height = m_CapsuleHeight;
                m_Capsule.center = m_CapsuleCenter;
                m_Crouching = false;
            }
        }

        void PreventStandingInLowHeadroom()
        {
            // prevent standing up in crouch-only zones
            if (!m_Crouching)
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                }
            }
        }


        void UpdateAnimator(Vector3 move)
        {
            // update the animator parameters
            m_Animator.SetFloat("Forward", m_ForwardAmount * m_ForwardSpeed, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            m_Animator.SetBool("Crouch", m_Crouching);
            m_Animator.SetBool("OnGround", m_IsGrounded);
            // m_Animator.SetBool("InputDetected", m_ForwardAmount != 0f);
            if (!m_IsGrounded)
            {
                m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
            }

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle =
                Mathf.Repeat(
                    m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
            if (m_IsGrounded)
            {
                m_Animator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (m_IsGrounded && move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                m_Animator.speed = 1;
            }
        }


        void HandleAirborneMovement()
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);

            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
        }


        void HandleGroundedMovement(bool crouch, bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                m_IsGrounded = false;
                m_Animator.applyRootMotion = false;
                m_GroundCheckDistance = 0.1f;
            }
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        }


        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (m_IsGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }
        }


        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                m_GroundNormal = hitInfo.normal;
                m_IsGrounded = true;
                m_Animator.applyRootMotion = true;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundNormal = Vector3.up;
                m_Animator.applyRootMotion = false;
            }
        }
    }



}
