using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputService
{
    float GetHorizontal();
    float GetVertical();
    bool PressedJump();
}
