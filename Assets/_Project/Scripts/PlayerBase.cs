using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerBase : MonoBehaviour
{
    private SignalBus _signalBus;

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
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _gridLayer;


    private Collider[] _results = new Collider[5];

    private Vector3 _movementDirection;

    private Vector3 _velocity;

    private GridCell _grid = null;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
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
    void Update()
    {
        CheckGrid();

        if (PressedJump() && IsGround())
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

        HandleMovement();
        HandleRotation();

        _velocity.y += _gravity * _gravityMultiplier * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
    private void CheckGrid()
    {
        int gridCount = Physics.OverlapSphereNonAlloc(_checkTransform.position, _checkDistance, _results, _gridLayer);

        if(gridCount > 0)
        {
            if (_results[0].TryGetComponent<GridCellView>(out GridCellView newGrid))
                SetGrid(newGrid.Grid);
        }
    }
    private void SetGrid(GridCell newGrid)
    {
        if (_grid == newGrid) return;

        _grid = newGrid;
        _signalBus.Fire(new GameSignal.OnGridChanged());
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
    private bool IsGround() => Physics.CheckSphere(_checkTransform.position, _checkDistance, _groundLayer);
    private bool PressedJump() => Input.GetKeyDown(KeyCode.Space);
    private bool IsMoving() => _movementDirection != Vector3.zero;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(_checkTransform.transform.position, _checkDistance);
    }
}
