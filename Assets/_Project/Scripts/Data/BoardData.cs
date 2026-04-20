using UnityEngine;

[CreateAssetMenu(fileName = "New Board Data", menuName = "Scriptable Object/Board Data")]
public class BoardData : ScriptableObject
{
    [Header("Board Settings")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _scale;
    [SerializeField] private float _lethalDuration;


    public int Width => _width;
    public int Height => _height;
    public float Scale => _scale;
    public float LethalDuration => _lethalDuration;

}
