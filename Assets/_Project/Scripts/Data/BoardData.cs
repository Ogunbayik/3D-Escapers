using UnityEngine;

[CreateAssetMenu(fileName = "New Board Data", menuName = "Scriptable Object/Board Data")]
public class BoardData : ScriptableObject
{
    [Header("Board Settings")]
    [SerializeField] private float _goalEffectDelay;
    [SerializeField] private float _nextGoalDelay;
    [Header("Spawn Settings")]
    [SerializeField] private float _spawnPerDuration;
    [SerializeField] private float _spawnY;
    [SerializeField] private float _targetY;

    public float GoalEffectDelay => _goalEffectDelay;
    public float NextGoalDelay => _nextGoalDelay;
    public float SpawnPerDuration => _spawnPerDuration;
    public float SpawnY => _spawnY;
    public float TargetY => _targetY;
}
