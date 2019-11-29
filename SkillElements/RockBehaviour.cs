using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class RockBehaviour : SkillObject
{


    public override void ReactToSkill()
    {
        if (_targetPosition != _startPosition)
        {
            return;
        }

        _targetPosition += _Character.gameObject.transform.up.normalized * 115f * Time.deltaTime;
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
    private float _timer;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    [SerializeField]
    private ThirdPersonCharacter _Character;
    
}
