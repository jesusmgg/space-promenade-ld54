using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] ShipStance _stance = ShipStance.Enemy;
    [SerializeField] EnemyBehavior _behavior;

    /// <summary>
    /// Follow this one.
    /// </summary>
    [SerializeField] Enemy _leaderEnemy;

    [SerializeField] List<Weapon> _weapons;

    Vector3 _formationOffset;

    Vector3 _targetPositionFluctuation;
    Vector3 _currentPositionFluctuation;

    Vector3 TargetPosition => _leaderEnemy.transform.position + _formationOffset;
    Quaternion TargetRotation => _leaderEnemy.transform.rotation;

    void Update()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.Shoot(Vector3.zero);
        }

        if (_behavior == EnemyBehavior.Moving)
        {
            if (_leaderEnemy != null)
            {
                Transform tr = transform;

                if (Random.value > .99f)
                {
                    _targetPositionFluctuation = new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
                }

                _currentPositionFluctuation =
                    Vector3.MoveTowards(_currentPositionFluctuation, _targetPositionFluctuation, Time.deltaTime * 0.1f);

                tr.position = TargetPosition + _currentPositionFluctuation;
                tr.rotation = TargetRotation;
            }

            else
            {
                // Move to random position.
            }
        }
    }
}

public enum EnemyBehavior
{
    Static,
    Moving,
}