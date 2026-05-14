using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera _gameCamera;
    [SerializeField] private CinemachineVirtualCamera _deadCamera;
    [SerializeField] private CinemachineVirtualCamera _menuCamera;
    [SerializeField] private CinemachineVirtualCamera _transitionCamera;
    [SerializeField] private CinemachineVirtualCamera _victoryCamera;
    [Header("Source References")]
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private void Start() => Initialize();
    private void Initialize() => SwitchCamera(CameraType.Menu);
    public void GenerateHitShake() => _impulseSource.GenerateImpulse();
    public void OnPlayerDead() => SwitchCamera(CameraType.Dead);
    public void OnPlayerVictory() => SwitchCamera(CameraType.Victory);
    public void SwitchCamera(CameraType cameraType)
    {
        _gameCamera.Priority = 10;
        _deadCamera.Priority = 10;
        _menuCamera.Priority = 10;
        _transitionCamera.Priority = 10;
        _victoryCamera.Priority = 10;

        switch(cameraType)
        {
            case CameraType.Game: _gameCamera.Priority = 20; break;
            case CameraType.Dead:_deadCamera.Priority = 20; break;
            case CameraType.Menu: _menuCamera.Priority = 20; break;
            case CameraType.Transition: _transitionCamera.Priority = 20; break;
            case CameraType.Victory: _victoryCamera.Priority =20; break;
        }
    }
    public async UniTask PlayPathTransition(bool forward, float duration = 3f)
    {
        var dolly = _transitionCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        var path = dolly.m_Path;

        // Hedef: Eðer forward true ise MaxPos (son), false ise 0 (baþlangýç)
        float targetPos = forward ? path.MaxPos : 0f;

        // Mevcut pozisyondan hedef pozisyona git
        var tween = DOTween.To(() => dolly.m_PathPosition,
                   x => dolly.m_PathPosition = x,
                   targetPos,
                   duration)
               .SetEase(Ease.InOutSine);

        await tween.AsyncWaitForCompletion();
    }
}
