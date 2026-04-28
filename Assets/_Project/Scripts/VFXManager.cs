using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager 
{
    private GoalEffect.Pool _goalPool;

    public VFXManager(GoalEffect.Pool goalPool)
    {
        _goalPool = goalPool;
    }
    public void PlayGoalEffect(Vector3 spawnPosition)
    {
        var goalEffect = _goalPool.Spawn(_goalPool);
        goalEffect.transform.position = spawnPosition;
    }
}
