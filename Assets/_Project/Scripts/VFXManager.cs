using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _goalVFX;

    public void PlayGoalEffect(Vector3 spawnPosition)
    {
        var goalEffect = Instantiate(_goalVFX);
        goalEffect.transform.position = spawnPosition;
    }
}
