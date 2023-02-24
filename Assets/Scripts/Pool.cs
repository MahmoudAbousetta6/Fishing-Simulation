using UnityEngine;

[System.Serializable]
public class Pool
{
    [SerializeField] private string tag;
    [SerializeField] private Transform prefab;
    [SerializeField] private int size;

    public string Tag
    {
        get => tag;
        set => tag = value;
    }

    public Transform Prefab
    {
        get => prefab;
        set => prefab = value;
    }

    public int Size
    {
        get => size;
        set => size = value;
    }
}