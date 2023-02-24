using System.Collections;
using UnityEngine;

public class FishHandler : MonoBehaviour
{
    [SerializeField] private float maxYPos;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxXPos;
    [SerializeField] private float minXPos;
    [SerializeField] private float maxZPos;
    [SerializeField] private float minZPos;
    [SerializeField] private bool hooked = false;
    [SerializeField] private float startSpeed = 5f;
    [SerializeField] private float currentSpeed = 5f;
    [SerializeField] private Transform lurePos;

    private float startTime;
    private Rigidbody rb;
    private Vector3 EAV;
    private bool turning = false;
    private float t;
    private readonly float timeToReachTarget = 3f;

    public Transform LurePos
    {
        get => lurePos;
        set => lurePos = value;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.Rotate(new Vector3(Random.Range(-20, 20), 180, 0));
    }

    private void FixedUpdate()
    {
        if (!hooked)
            Move();
        else
            GoToLure();
    }

    private void GoToLure()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(transform.position, LurePos.position, t);
    }

    public void IsHooked() => hooked = true;

    private void Move()
    {
        rb.velocity = transform.forward * -currentSpeed;
        turnScript();
        if (turning) return;
        if (Random.Range(0f, 1f) < 0.02)
            StartCoroutine(burstOfSpeed());
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Boat")
            rb.AddForce((transform.position - col.gameObject.transform.position) * 200);
    }

    private IEnumerator burstOfSpeed()
    {
        currentSpeed += 5;
        yield return new WaitForSeconds(0.2f);
        currentSpeed = startSpeed;
    }

    private void Turn(Vector3 turnVector)
    {
        var deltaRotation = Quaternion.Euler(turnVector * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
        turning = true;
    }

    private void turnScript()
    {
        var isEAVTurn = transform.position.x >= maxXPos;
        isEAVTurn |= transform.position.z >= maxZPos;
        isEAVTurn |= transform.position.x <= minXPos;
        isEAVTurn |= transform.position.z <= minZPos;
        if (isEAVTurn)
        {
            EAV = new Vector3(0, 100, 0);
            Turn(EAV);
        }
        else
        {
            EAV = new Vector3(0, 0, 0);
            turning = false;
        }

        var tempXValue = 0.0f;
        if (transform.position.y > maxYPos)
            tempXValue = Random.Range(-20, -10);
        else if (transform.position.y < minYPos)
            tempXValue = Random.Range(20, 10);
        else
        {
            turning = false;
            return;
        }

        Turn(new Vector3(tempXValue, 0, 0));
    }
}