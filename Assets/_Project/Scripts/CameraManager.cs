using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera _gameCamera;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    public void GenerateHitShake() => _impulseSource.GenerateImpulse();
}
