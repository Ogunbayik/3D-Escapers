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
    public class PhysicsDefaults
    {
        public const float GROUNDED_GRAVITY = -2f;
        public const float GRAVITY_COEFFICIENT = -2f;
    }
}
