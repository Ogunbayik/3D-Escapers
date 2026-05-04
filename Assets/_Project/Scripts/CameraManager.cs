using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera _gameCamera;
    [SerializeField] private CinemachineVirtualCamera _deadCamera;
    [Header("Source References")]
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private void Start() => Initialize();
    private void Initialize() => SwitchCamera(CameraType.Game);
    public void GenerateHitShake() => _impulseSource.GenerateImpulse();
    public void OnPlayerDead() => SwitchCamera(CameraType.Dead);
    public void SwitchCamera(CameraType cameraType)
    {
        _gameCamera.Priority = 10;
        _deadCamera.Priority = 10;

        switch(cameraType)
        {
            case CameraType.Game: _gameCamera.Priority = 20; break;
            case CameraType.Dead:_deadCamera.Priority = 20; break;
        }
    }
}
