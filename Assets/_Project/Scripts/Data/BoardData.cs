using UnityEngine;

[CreateAssetMenu(fileName = "New Board Data", menuName = "Scriptable Object/Board Data")]
public class BoardData : ScriptableObject
{
    [Header("Board Settings")]
    [SerializeField] private float _goalEffectDelay;
    [SerializeField] private float _nextGoalDelay;
    [Header("Grid Settings")]
    [SerializeField] private float _increaseDuration;

    public float GoalEffectDelay => _goalEffectDelay;
    public float NextGoalDelay => _nextGoalDelay;
    public float IncreaseDuration => _increaseDuration;
}
