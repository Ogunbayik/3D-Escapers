using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerBase : MonoBehaviour
{
    private SignalBus _signalBus;
    private IInputService _input;

    private CharacterController _characterController;

    [Header("Visual Settings")]
    [SerializeField] private Transform _playerVisual;
    [SerializeField] private Transform _checkTransform;
    [Header("Layer Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _gridLayer;
    [Header("Data Settings")]
    [SerializeField] private PlayerData _data;

    private Collider[] _results = new Collider[5];

    private float _velocityY;
    public float VelocityY => _velocityY;

    private GridCell _grid = null;

    public bool TEST_GROUND = true;

    [Inject]
    public void Construct(SignalBus signalBus, CharacterController characterController, IInputService input)
    {
        _signalBus = signalBus;
        _characterController = characterController;
        _input = input;
    }
    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnGridChanged>(CheckPlayerGridStatus);
        _signalBus.Subscribe<GameSignal.OnGridColorChanged>(CheckPlayerGridStatus);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnGridChanged>(CheckPlayerGridStatus);
        _signalBus.Unsubscribe<GameSignal.OnGridColorChanged>(CheckPlayerGridStatus);
    }
    public void Move(Vector3 movementDirection)
    {
        if (movementDirection.magnitude > 1f)
            movementDirection.Normalize();

        Vector3 finalMovement = movementDirection * _data.MovementSpeed;
        finalMovement.y = _velocityY;

        _characterController.Move(finalMovement * Time.deltaTime);

        HandleRotation(movementDirection);
    }
    private void HandleRotation(Vector3 direction)
    {
        if (IsMoving())
        {
            var rotation = Quaternion.LookRotation(direction);
            _playerVisual.transform.rotation = Quaternion.Slerp(_playerVisual.transform.rotation, rotation, _data.RotationSpeed * Time.deltaTime);
        }
    }
    public void CheckGrid()
    {
        int gridCount = Physics.OverlapSphereNonAlloc(_checkTransform.position, _data.CheckDistance, _results, _gridLayer);

        if(gridCount > 0)
        {
            if (_results[0].TryGetComponent<GridCellView>(out GridCellView newGrid))
                SetGrid(newGrid.Grid);
        }
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

        switch (_grid.GridStatus)
        {
            case GridStatus.Goal:
                //TODO Player point is increased!
                Debug.Log("Player gain point!");
                break;
            case GridStatus.Lethal:
                //TODO Player health is decreased!
                Debug.Log("Player health is deacreased!");
                break;
            default:
                Debug.LogWarning("Unknown Grid Status detected!");
                break;
        }

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

        return direction.magnitude > 0.1f;
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
