using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController
{
    private Animator _animator;

    public AnimationController(Animator animator) => _animator = animator;
    public void PlayAnimation(int animationHash, float transitionTime)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animationHash && !_animator.IsInTransition(0))
            return;

        _animator.CrossFade(animationHash, transitionTime);
    }
}
