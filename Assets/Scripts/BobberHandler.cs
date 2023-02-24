using UnityEngine;

public class BobberHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Rigidbody rb;
    private bool isActive;

    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }

    private void Start() => isActive = true;

    private void Update()
    {
        if (!isActive) return;
        rb.isKinematic = true;
        var col = Physics.OverlapSphere(transform.position, 0.5f, _layer);
        if (col.Length <= 0) return;
        isActive = false;
        var fish = col[0].gameObject.GetComponent<FishHandler>();
        fish.LurePos = transform;
        fish.IsHooked();
        UIHandler.SetTextHookedText("Hooked");
    }
}