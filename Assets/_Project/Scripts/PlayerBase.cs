using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private CharacterController _characterController;

    [Header("Visual")]
    [SerializeField] private Transform _playerVisual;
    [Header("Movement")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [Header("Jump")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    [SerializeField] private float _gravityMultiplier;
    [Header("Checking")]
    [SerializeField] private Transform _checkTransform;
    [SerializeField] private float _checkDistance;
    [SerializeField] private LayerMask _checkLayer;


    private Vector3 _movementDirection;

    private Vector3 _velocity;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (PressedJump() && IsGround())
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            HandleMovement();
        HandleRotation();

        _velocity.y += _gravity * _gravityMultiplier * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        var isGround = Physics.CheckSphere(_checkTransform.position, _checkDistance, _checkLayer);
    }

    private void HandleMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        _movementDirection.Set(horizontal, 0f, vertical);

        if (_movementDirection.magnitude > 1f)
            _movementDirection.Normalize();

        if (IsMoving())
            _characterController.Move(_movementDirection * _movementSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        if (IsMoving())
        {
            var rotation = Quaternion.LookRotation(_movementDirection);
            _playerVisual.transform.rotation = Quaternion.Slerp(_playerVisual.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
        }
    }
    private bool IsGround() => Physics.CheckSphere(_checkTransform.position, _checkDistance, _checkLayer);
    private bool PressedJump() => Input.GetKeyDown(KeyCode.Space);
    private bool IsMoving() => _movementDirection != Vector3.zero;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(_checkTransform.transform.position, _checkDistance);
    }
}
