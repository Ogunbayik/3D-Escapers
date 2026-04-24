using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    public class PlayerInput
    {
        public const string HORIZONTAL_INPUT = "Horizontal";
        public const string VERTICAL_INPUT = "Vertical";
    }
    public class PlayerAnimation
    {
        public const string PLAYER_IDLE = "Player_Idle_Breathing";
        public const string PLAYER_MOVE = "Player_Move";
        public const string PLAYER_JUMP = "Player_Jump";

        public static readonly int IDLE_HASH = Animator.StringToHash(PLAYER_IDLE);
        public static readonly int MOVE_HASH = Animator.StringToHash(PLAYER_MOVE);
        public static readonly int JUMP_HASH = Animator.StringToHash(PLAYER_JUMP);
    }
    public class AnimationDuration
    {
        public const float QUICK_TRANSITION = 0.1f;
        public const float SMOOTH_TRANSITION = 0.2f;
    }
    public class PhysicsDefaults
    {
        public const float GROUNDED_GRAVITY = -2f;
        public const float GRAVITY_COEFFICIENT = -2f;
    }
}
