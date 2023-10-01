﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Ship
{
    [SerializeField] EnemyBehavior _behavior;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _rotationSpeed = 10f;
    [SerializeField] Collider _targetAreaCollider;

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
                _targetPosition = Util.GetRandomPointInBounds(_targetAreaCollider);
                _targetPosition.y = 0f;
                StartCoroutine(CheckForWallsInTargetDirection());
                break;
            }
            case EnemyBehavior.SeekPlayer:
            {
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