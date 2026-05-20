using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardTriggerZone : MonoBehaviour
{
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;

    [Header("Environment References")]
    [SerializeField] private GameObject _walkGround;

    private bool _hasPlayerInBoard = false;

    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnLevelScoreReached>(ActivateWalkGround);
        _signalBus.Subscribe<GameSignal.OnLevelCompleted>(ResetPlayerBoardStatus);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnLevelScoreReached>(ActivateWalkGround);
        _signalBus.Unsubscribe<GameSignal.OnLevelCompleted>(ResetPlayerBoardStatus);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameConst.GameTag.PLAYER_TAG))
        {
            DeactivateWalkGround();
            _hasPlayerInBoard = true;
        }
    }
    private void ResetPlayerBoardStatus() => _hasPlayerInBoard = false;
    private void ActivateWalkGround()
    {
        _walkGround.SetActive(true);
    }
    private void DeactivateWalkGround()
    {
        if (_hasPlayerInBoard) return;

        _walkGround.SetActive(false);
    }

}
