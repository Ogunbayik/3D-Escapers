using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVictoryState : PlayerBaseState
{
    private Vector3 testRotation = new Vector3(0f, 220f, 0f);
    public PlayerVictoryState(PlayerBase player, AnimationController animationController) : base(player, animationController) { }

    public override void EnterState()
    {
        VictorySequence().Forget();
    }
    public override void Tick()
    {

    }
    public override void ExitState()
    {
        
    }

    public async UniTask VictorySequence()
    {
        Player.AlignToVictoryPose();

        //2f Camera TransitionTime
        await UniTask.Delay(System.TimeSpan.FromSeconds(2f));

        AnimationController.PlayAnimation(GameConst.PlayerAnimation.VICTORY_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);

        await UniTask.WaitUntil(() => AnimationController.IsAnimationFinished(GameConst.PlayerAnimation.VICTORY_HASH));

        AnimationController.PlayAnimation(GameConst.PlayerAnimation.IDLE_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);
    }

    
}
