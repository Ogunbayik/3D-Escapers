using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputService : IInputService
{
    public float GetHorizontal() => Input.GetAxis(GameConst.PlayerInput.HORIZONTAL_INPUT);
    public float GetVertical() => Input.GetAxis(GameConst.PlayerInput.VERTICAL_INPUT);
    public bool PressedJump() => Input.GetKeyDown(KeyCode.Space);
}
