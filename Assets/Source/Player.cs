﻿using System.Collections.Generic;
using UnityEngine;

public class Player : Ship 
{
    [SerializeField] float _acceleration = 1f;
    [SerializeField] float _deceleration = 1f;
    [SerializeField] float _topSpeed = 2f;

    [SerializeField] Collider _mouseCollider;

    [SerializeField] float _captureRadius = 10f;

    [SerializeField] float _minFov = 25f;
    [SerializeField] float _maxFov = 100f;
    [SerializeField] float _fovPerAlly = 2.5f;
    [SerializeField] float _fovSpeed = 1f;

    [SerializeField] List<Weapon> _weapons;

    float _normalizedFov;

    Vector3 _mousePos;

    int _allies;

    Camera _mainCamera;
    readonly Collider[] _captureBuffer = new Collider[100];

    float Speed { get; set; }

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        UpdateTransform();
        UpdateCaptures();

        foreach (Weapon weapon in _weapons)
        {
            weapon.Shoot();
        }
    }

    void UpdateTransform()
    {
        // Get mouse position
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (_mouseCollider.Raycast(ray, out RaycastHit hitData, 1000))
        {
            _mousePos = hitData.point;
        }

        // Move ship
        Vector3 flatMousePos = _mousePos;
        flatMousePos.y = 0f;
        Vector3 direction = (flatMousePos - transform.position).normalized;

        Speed += (_acceleration * Time.deltaTime * Input.GetAxis("Fire2"));
        Speed -= _deceleration * Time.deltaTime;
        Speed = Mathf.Clamp(Speed, 0f, _topSpeed);

        transform.Translate(direction * Speed, Space.World);

        // Rotate ship
        transform.LookAt(flatMousePos, Vector3.up);
    }

    void UpdateCaptures()
    {
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, _captureRadius, _captureBuffer);

        for (var i = 0; i < colliderCount; i++)
        {
            var ally = _captureBuffer[i].GetComponent<Ally>();
            if (ally != null)
            {
                if (ally.Capture())
                {
                    _allies += 1;
                }
            }
        }

        float currentNormalizedFov = (_fovPerAlly / _maxFov) * _allies;
        _normalizedFov = Mathf.MoveTowards(_normalizedFov, currentNormalizedFov, _fovSpeed * Time.deltaTime);
        _mainCamera.fieldOfView = Mathf.Lerp(_minFov, _maxFov, _normalizedFov);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _captureRadius);
    }
}