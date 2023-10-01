using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] ShipStance _stance = ShipStance.Enemy;
    [SerializeField] EnemyBehavior _behavior;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _rotationSpeed = 10f;

    /// <summary>
    /// Follow this one.
    /// </summary>
    [SerializeField] Enemy _leaderEnemy;

    [SerializeField] List<Weapon> _weapons;

    Vector3 _formationOffset;

    Vector3 _targetPositionFluctuation;
    Vector3 _currentPositionFluctuation;

    Vector3 _targetPosition;

    Arena _arena;
    Collider _arenaCollider;
    Player _player;

    void Awake()
    {
        _arena = FindFirstObjectByType<Arena>();
        _arenaCollider = _arena.GetComponent<Collider>();
        _player = FindFirstObjectByType<Player>();
    }

    void Start()
    {
        _formationOffset = Util.GetRandomVector3(-10f, 10f);
        _targetPosition = transform.position;
        UpdateTarget();
    }

    void Update()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.Shoot(Vector3.zero);
        }

        if (_behavior != EnemyBehavior.Static)
        {
            if (Vector3.Distance(transform.position, _targetPosition + _currentPositionFluctuation) <= float.Epsilon)
            {
                UpdateTarget();
            }

            Transform tr = transform;

            if (Random.value > .99f)
            {
                _targetPositionFluctuation = new Vector3(Random.Range(-.5f, .5f), 0f, Random.Range(-.5f, .5f));
            }

            _currentPositionFluctuation =
                Vector3.MoveTowards(_currentPositionFluctuation, _targetPositionFluctuation, Time.deltaTime * 0.1f);

            Vector3 adjustedTargetPos = _targetPosition + _currentPositionFluctuation;
            Vector3 position = tr.position;
            Vector3 newPos = Vector3.MoveTowards(
                position,
                adjustedTargetPos,
                Time.deltaTime * _speed);
            newPos.y = 0f;

            tr.position = newPos;

            Vector3 direction = (newPos - position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion newRot = Quaternion.RotateTowards(tr.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            tr.rotation = newRot;
        }
    }

    void UpdateTarget()
    {
        switch (_behavior)
        {
            case EnemyBehavior.MoveRandomly:
            {
                _targetPosition = Util.GetRandomPointInBounds(_arenaCollider);
                _targetPosition.y = 0f;
                break;
            }
            case EnemyBehavior.SeekPlayer:
            {
                _targetPosition = _player.transform.position + Util.GetRandomVector3(-5f, 5f);
                break;
            }
            case EnemyBehavior.FollowLeader:
            {
                if (_leaderEnemy == null)
                {
                    _behavior = EnemyBehavior.MoveRandomly;
                    break;
                }

                Transform leaderTransform = _leaderEnemy.transform;
                _targetPosition = leaderTransform.position + _formationOffset;
                break;
            }
        }
    }
}

public enum EnemyBehavior
{
    Static,
    MoveRandomly,
    SeekPlayer,
    FollowLeader
}