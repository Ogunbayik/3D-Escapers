using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Scriptable Object/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [Header("Jump Settings")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    [SerializeField] private float _gravityMultiplier;
    [Header("Check Settings")]
    [SerializeField] private float _checkDistance;
    [Header("Health Settings")]
    [SerializeField] private int _maximumHealth;

    public float MovementSpeed => _movementSpeed;
    public float RotationSpeed => _rotationSpeed;
    public float JumpHeight => _jumpHeight;
    public float Gravity => _gravity;
    public float GravityMultiplier => _gravityMultiplier;
    public float CheckDistance => _checkDistance;
    public int MaximumHealth => _maximumHealth;
}
