using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class PillarBehaviour : SkillObject
{
    public override void ReactToSkill()
    {
        if (_targetPosition != _startPosition)
        {
            return;
        }

        _targetPosition += _Character.gameObject.transform.forward.normalized * 30f * Time.deltaTime;
        _Character.Idle();

    }

    private void Start()
    {
        _targetPosition = this.transform.position;
        _startPosition = _targetPosition;
    }

    private void Update()
    {
        if (_targetPosition != _startPosition)
        {
            _timer += Time.deltaTime / 2f;
            this.transform.position = Vector3.Lerp(this.transform.position, _targetPosition, _timer);
            if (_timer > 1f)
            {
                _timer = 0f;
                _startPosition = _targetPosition;
            }
        }
    }

    [SerializeField]
    private ThirdPersonCharacter _Character;

    private Vector3 _targetPosition;
    private Vector3 _startPosition;

    private float _timer;
    // private NavMeshAgent _agent;
}
