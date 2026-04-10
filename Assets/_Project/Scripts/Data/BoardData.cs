using UnityEngine;

[CreateAssetMenu(fileName = "New Board Data", menuName = "Scriptable Object/Board Data")]
public class BoardData : ScriptableObject
{
    [Header("Board Settings")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _scale;


    public int Width => _width;
    public int Height => _height;
    public float Scale => _scale;

}
