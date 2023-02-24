using UnityEngine;

public class LinePoints : MonoBehaviour
{
    [SerializeField] private Vector3 _pos = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 _oldPos;
    [SerializeField] private Vector3 _acceleration = new Vector3(0, -5f, 0);

    public Vector3 Pos
    {
        get => _pos;
        set => _pos = value;
    }

    public Vector3 OldPos
    {
        get => _oldPos;
        set => _oldPos = value;
    }

    public Vector3 Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }
}