using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera _gameCamera;
    [SerializeField] private CinemachineVirtualCamera _deadCamera;
    [SerializeField] private CinemachineVirtualCamera _menuCamera;
    [SerializeField] private CinemachineVirtualCamera _transitionCamera;
    [Header("Source References")]
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private void Start() => Initialize();
    private void Initialize() => SwitchCamera(CameraType.Menu);
    public void GenerateHitShake() => _impulseSource.GenerateImpulse();
    public void OnPlayerDead() => SwitchCamera(CameraType.Dead);
    public void SwitchCamera(CameraType cameraType)
    {
        _gameCamera.Priority = 10;
        _deadCamera.Priority = 10;
        _menuCamera.Priority = 10;
        _transitionCamera.Priority = 10;

        switch(cameraType)
        {
            case CameraType.Game: _gameCamera.Priority = 20; break;
            case CameraType.Dead:_deadCamera.Priority = 20; break;
            case CameraType.Menu: _menuCamera.Priority = 20; break;
            case CameraType.Transition: _transitionCamera.Priority = 20; break;
        }
    }
    public void OnTransitionStart()
    {
        var dolly = _transitionCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        var maxPath = _transitionCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path.MaxPos;
        var transitionDuration = 3f;

        DOTween.To(() => dolly.m_PathPosition, x => dolly.m_PathPosition = x, maxPath, transitionDuration).SetEase(Ease.InOutSine);
    }
}
