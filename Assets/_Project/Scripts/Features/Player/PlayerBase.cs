using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerBase : MonoBehaviour
{
    private PlayerVisual _visual;
    private PlayerHealth _health;

    private SignalBus _signalBus;
    private IInputService _input;

    private CharacterController _characterController;

    [Header("Visual References")]
    [SerializeField] private Transform _checkTransform;
    [Header("Layer Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _gridLayer;
    [Header("Data References")]
    [SerializeField] private PlayerData _data;

    private Collider[] _results = new Collider[5];

    private GridCell _grid = null;

    private float _velocityY;
    public float VelocityY => _velocityY;
    [Inject]
    public void Construct(PlayerVisual visual,PlayerHealth health,SignalBus signalBus, CharacterController characterController, IInputService input)
    {
        _visual = visual;
        _health = health;
        _signalBus = signalBus;
        _characterController = characterController;
        _input = input;
    }
    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnGridChanged>(CheckPlayerGridStatus);
        _signalBus.Subscribe<GameSignal.OnGridColorChanged>(CheckPlayerGridStatus);
        _signalBus.Subscribe<GameSignal.OnPlayerLanded>(CheckPlayerGridStatus);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnGridChanged>(CheckPlayerGridStatus);
        _signalBus.Unsubscribe<GameSignal.OnGridColorChanged>(CheckPlayerGridStatus);
        _signalBus.Unsubscribe<GameSignal.OnPlayerLanded>(CheckPlayerGridStatus);
    }
    public void Move(Vector3 movementDirection)
    {
        if (movementDirection.sqrMagnitude > 1f)
            movementDirection.Normalize();

        Vector3 finalMovement = movementDirection * _data.MovementSpeed;
        finalMovement.y = _velocityY;

        _characterController.Move(finalMovement * Time.deltaTime);

        HandleRotation(movementDirection);
    }
    private void HandleRotation(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            var rotation = Quaternion.LookRotation(direction);
            _visual.Body.rotation = Quaternion.Slerp(_visual.Body.rotation, rotation, _data.RotationSpeed * Time.deltaTime);
        }
    }
    public void AlignToMenuPose() => _visual.Body.rotation = Quaternion.Euler(_data.MenuPoseRotation);
    public void CheckGrid()
    {
        int gridCount = Physics.OverlapSphereNonAlloc(_checkTransform.position, _data.CheckDistance, _results, _gridLayer);

        if (gridCount > 0 && IsGrounded())
        {
            if (_results[0].TryGetComponent<GridCellView>(out GridCellView newGrid))
                SetGrid(newGrid.Grid);
        }
        else
            SetGrid(null);
    }
    public void SetGrid(GridCell newGrid)
    {
        if (_grid == newGrid) return;

        _grid = newGrid;
        _signalBus.Fire(new GameSignal.OnGridChanged());
    }
    private void CheckPlayerGridStatus()
    {
        if (_grid == null || _grid.GridStatus == GridStatus.Safe) return;
        if (_health.IsInvulnerable || _health.IsDead) return;

        _signalBus.Fire(new GameSignal.OnPlayerGridStatus(_grid.GridStatus));
    }
    public void ApplyGravity()
    {
        if (IsGrounded() && _velocityY <= 0)
            _velocityY = GameConst.PhysicsDefaults.GROUNDED_GRAVITY;
        else
            _velocityY += Physics.gravity.y * _data.GravityMultiplier * Time.deltaTime;
    }
    public void HandleJump() => _velocityY = Mathf.Sqrt(_data.JumpHeight * GameConst.PhysicsDefaults.GRAVITY_COEFFICIENT * Physics.gravity.y);
    public bool IsMoving()
    {
        var direction = GetMovementDirection();

        return direction.sqrMagnitude > 0.01f;
    }
    public bool IsGrounded() => Physics.CheckSphere(_checkTransform.position, _data.CheckDistance, _groundLayer);
    public bool PressedJump() => _input.PressedJump();
    public Vector3 GetMovementDirection()
    {
        var horizontal = _input.GetHorizontal();
        var vertical = _input.GetVertical();
        var direction = new Vector3(horizontal, 0f, vertical);

        return direction;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(_checkTransform.transform.position, _data.CheckDistance);
    }
}
