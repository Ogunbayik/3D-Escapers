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
        public const string PLAYER_DEATH = "Player_Death";
        public const string PLAYER_FALL = "Player_Fall";
        //Menu Animations
        public const string PLAYER_SHRUGGING = "Player_Menu_Shrugging";
        public const string PLAYER_LOOKOVERSHOULDER = "Player_Menu_Look_Over_Shoulder";
        public const string PLAYER_SHOULDERRUBBING = "Player_Menu_Shoulder_Rubbing";

        public static readonly int IDLE_HASH = Animator.StringToHash(PLAYER_IDLE);
        public static readonly int MOVE_HASH = Animator.StringToHash(PLAYER_MOVE);
        public static readonly int JUMP_HASH = Animator.StringToHash(PLAYER_JUMP);
        public static readonly int DEATH_HASH = Animator.StringToHash(PLAYER_DEATH);
        public static readonly int FALL_HASH = Animator.StringToHash(PLAYER_FALL);
        //Menu Animations
        public static readonly int SHRUGGING_HASH = Animator.StringToHash(PLAYER_SHRUGGING);
        public static readonly int LOOKOVERSHOULDER_HASH = Animator.StringToHash(PLAYER_LOOKOVERSHOULDER);
        public static readonly int SHOULDERRUBBING_HASH = Animator.StringToHash(PLAYER_SHOULDERRUBBING);
    }
    public static class Durations
    {
        public const float INITIAL_DELAY = 1.5f;           
        public const float CAMERA_TRANSITION_TIME = 2f;  
        public const float BOARD_SETUP_DELAY = 1f;       
        public const float PLAYER_TELEPORT_DELAY = 0.5f;
        public const float GAMEPLAY_START_DELAY = 2f;
    }
    public static class ShaderProperties
    {
        public const string DISSOLVE_AMOUNT = "_DissolveAmount";
        public const string PLAYER_BASE_COLOR = "_BaseColor";

        public const float DISSOLVE_APPEAR_VALUE = -1f;
        public const float DISSOLVE_DISAPPEAR_VALUE = 1f;
    }
    public class AnimationTransition
    {
        public const float QUICK_TRANSITION = 0.1f;
        public const float SMOOTH_TRANSITION = 0.2f;
    }
    public class PhysicsDefaults
    {
        public const float GROUNDED_GRAVITY = -2f;
        public const float GRAVITY_COEFFICIENT = -2f;
    }
    public class HealthPercentage
    {
        public const float CRITICAL_PERCENTAGE = 0.25f;
        public const float UNSTABLE_PERCENTAGE = 0.5f;
        public const float STABLE_PERCENTAGE = 0.75f;
    }
}
