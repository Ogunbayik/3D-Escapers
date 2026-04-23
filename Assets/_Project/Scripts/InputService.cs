using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputService : IInputService
{
    public float GetHorizontal() => Input.GetAxis("Horizontal");
    public float GetVertical() => Input.GetAxis("Vertical");
    public bool PressedJump() => Input.GetKeyDown(KeyCode.Space);
}
