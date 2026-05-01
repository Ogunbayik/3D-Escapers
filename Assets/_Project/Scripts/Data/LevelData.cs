using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Scriptable Object/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Board Dimensions")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _scale;

    [Header("Gameplay Settings")]
    [SerializeField] private float _lethalDuration;
    [SerializeField] private int _scorePerGoal;

    [Header("Lethal Groups")]
    [SerializeField] private List<CellGroup> _lethalGroups;

    public int Width => _width;
    public int Height => _height;
    public float Scale => _scale;
    public float LethalDuration => _lethalDuration;
    public int ScorePerGoal => _scorePerGoal;
    public List<CellGroup> LethalGroups => _lethalGroups;
    
}
