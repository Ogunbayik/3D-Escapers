using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private Transform _playerVisual;

    private Vector3 _movementDirection;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        _movementDirection.Set(horizontal, 0f, vertical);

        if (_movementDirection.magnitude > 1f)
            _movementDirection.Normalize();

        if (IsMoving())
            _characterController.Move(_movementDirection * _movementSpeed * Time.deltaTime);

        HandleRotation();
    }

    private void HandleRotation()
    {
        if (IsMoving())
        {
            var rotation = Quaternion.LookRotation(_movementDirection);
            _playerVisual.transform.rotation = Quaternion.Slerp(_playerVisual.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private bool IsMoving() => _movementDirection != Vector3.zero;
}
