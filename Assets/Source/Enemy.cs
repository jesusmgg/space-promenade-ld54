using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : Ship
{
    [SerializeField] EnemyBehavior _behavior;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _rotationSpeed = 10f;

    [FormerlySerializedAs("_targetAreaCollider")] [SerializeField]
    public Collider TargetAreaCollider;

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
    Player _player;

    RaycastHit[] _raycastHitBuffer = new RaycastHit[100];

    protected override void Awake()
    {
        base.Awake();

        _arena = FindFirstObjectByType<Arena>();
        _player = FindFirstObjectByType<Player>();
    }

    void Start()
    {
        _formationOffset = Util.GetRandomVector3(-10f, 10f);
        _targetPosition = transform.position;
        UpdateTarget();
    }

    protected override void Update()
    {
        base.Update();

        foreach (Weapon weapon in _weapons)
        {
            weapon.Shoot();
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

    void UpdateTarget(EnemyBehavior? forcedBehavior = null)
    {
        EnemyBehavior behavior = forcedBehavior ?? _behavior;
        switch (behavior)
        {
            case EnemyBehavior.MoveRandomly:
            {
                if (TargetAreaCollider == null)
                {
                    TargetAreaCollider = _arena.GetComponent<Collider>();
                }

                _targetPosition = Util.GetRandomPointInBounds(TargetAreaCollider);
                Vector3 localScale = TargetAreaCollider.transform.localScale;
                _targetPosition -= TargetAreaCollider.bounds.center / 4f;
                _targetPosition.x /= localScale.x;
                _targetPosition.z /= localScale.z;
                _targetPosition = TargetAreaCollider.transform.TransformPoint(_targetPosition);
                _targetPosition.y = 0f;
                StartCoroutine(CheckForWallsInTargetDirection());
                break;
            }
            case EnemyBehavior.SeekPlayer:
            {
                if (_player == null)
                {
                    _behavior = EnemyBehavior.MoveRandomly;
                    break;
                }

                _targetPosition = _player.transform.position + Util.GetRandomVector3(-5f, 5f);
                StartCoroutine(CheckForWallsInTargetDirection());
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
                StartCoroutine(CheckForWallsInTargetDirection());
                break;
            }
        }
    }

    IEnumerator CheckForWallsInTargetDirection()
    {
        yield return new WaitForSeconds(.1f);
        Vector3 position = transform.position;
        Vector3 direction = _targetPosition - position;

        int hitCount = Physics.RaycastNonAlloc(position, direction.normalized, _raycastHitBuffer, direction.magnitude);
        for (var i = 0; i < hitCount; i++)
        {
            if (_raycastHitBuffer[i].collider.CompareTag("Wall"))
            {
                UpdateTarget(EnemyBehavior.MoveRandomly);
            }
        }
    }

    public override void Destroy()
    {
        ParticleManager.EmitHitLarge(transform.position);
        base.Destroy();
    }

    void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = .1f;
        Gizmos.color = color;

        Vector3 position = transform.position;
        Vector3 direction = _targetPosition - position;
        Gizmos.DrawRay(position, direction);
    }
}

public enum EnemyBehavior
{
    Static,
    MoveRandomly,
    SeekPlayer,
    FollowLeader
}