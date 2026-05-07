using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuIdleState : PlayerBaseState
{
    private float _minTime = 5f;
    private float _maxTime = 10f;
    private int _menuAnimationCount = 3;

    private int _currentFidgetHash;

    private bool _isPlayingFidget;

    private float _countdownTimer;
    public PlayerMenuIdleState(PlayerBase player, AnimationController animationController) : base(player, animationController) { }
    public override void EnterState()
    {
        _countdownTimer = GameUtilis.GetRandomFloat(_minTime, _maxTime);

        Player.AlignToMenuPose();

        AnimationController.PlayAnimation(GameConst.PlayerAnimation.IDLE_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);
    }
    public override void ExitState()
    {

    }
    public override void Tick()
    {
        if (_isPlayingFidget)
        {
            if (AnimationController.IsAnimationFinished(_currentFidgetHash))
            {
                AnimationController.PlayAnimation(GameConst.PlayerAnimation.IDLE_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);

                _isPlayingFidget = false;
                _countdownTimer = GameUtilis.GetRandomFloat(_minTime, _maxTime);
            }
        }
        else
            SwitchCountdown();
    }
    private void SwitchCountdown()
    {
        _countdownTimer -= Time.deltaTime;

        if(_countdownTimer <= 0)
        {
            var randomIndex = GameUtilis.GetRandomInt(0, _menuAnimationCount);
            SwitchRandomAnimation(randomIndex);

            _isPlayingFidget = true;
        }
    }
    private void SwitchRandomAnimation(int index)
    {
        switch(index)
        {
            case 0: _currentFidgetHash = GameConst.PlayerAnimation.LOOKOVERSHOULDER_HASH; break;
            case 1: _currentFidgetHash = GameConst.PlayerAnimation.SHRUGGING_HASH; break;
            case 2: _currentFidgetHash = GameConst.PlayerAnimation.SHOULDERRUBBING_HASH; break;
        }

        AnimationController.PlayAnimation(_currentFidgetHash, GameConst.AnimationTransition.QUICK_TRANSITION);
    }
    
}
