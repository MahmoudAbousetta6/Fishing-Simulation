using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private Transform topOfFishingLine;
    [SerializeField] private Rigidbody hook;
    [SerializeField] private Transform player;
    [SerializeField] private BobberHandler bobber;
    [Range(0f, 1f)] [SerializeField] private float lineLength = 0.1f;
    [Range(-100f, 0f)] [SerializeField] private float acceleration = -5f;
    [Range(0, 0.1f)] [SerializeField] private float dt = 0.1f;

    private LinePoints[] particles = new LinePoints[10];
    private LineRenderer lineRenderer;
    private Vector2 closestPoint;
    private Vector3 tmpHit;
    private bool hitWater = false;
    private bool doOnce = true;
    private bool throwing = false;
    private bool throwStarted = false;

    private bool isBobber;


    public float LineLength
    {
        get => lineLength;
        set => lineLength = value;
    }

    public float DT
    {
        get => dt;
        set => dt = value;
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    public void LinePointInit(int num)
    {
        if (lineRenderer.positionCount != 0) return;
        particles = new LinePoints[num];
        lineRenderer.positionCount = num;

        for (var i = 0; i < particles.Length; i++)
        {
            particles[i] = new LinePoints();
            particles[i].Acceleration = new Vector3(0, acceleration, 0);
        }

        throwing = true;
    }

    private void UpdatePos()
    {
        for (var i = 1; i < particles.Length; i++)
            Verlet(particles[i], DT);

        particles[1].Pos = topOfFishingLine.position;

        for (var j = 0; j < 5; j++)
        for (var i = 1; i < particles.Length - 1; i++)
            PoleConstraint(particles[i + 1], particles[i], LineLength);

        for (var i = 1; i < particles.Length; i++)
            lineRenderer.SetPosition(i, particles[i].Pos);

        lineRenderer.SetPosition(0, topOfFishingLine.position);
    }

    private void Update()
    {
        if (lineRenderer.positionCount == 0) return;
        isBobber = Vector3.Distance(bobber.transform.position, particles[^1].Pos) <= 0.3f;
        if (!isBobber) return;
        bobber.transform.position = particles[^1].Pos;
        bobber.IsActive = true;
    }

    private void LateUpdate()
    {
        if (lineRenderer.positionCount == 0) return;
        if (!throwing) return;
        UpdatePos();

        if (hitWater)
        {
            hook.velocity = new Vector3(0, 0, 0);
            hook.transform.position = Vector3.Lerp(tmpHit,
                new Vector3(topOfFishingLine.position.x, hook.transform.position.y,
                    topOfFishingLine.position.z), 1 - LineLength);
            var yMod = (Mathf.PerlinNoise(Time.time, 1));
            hook.transform.position += new Vector3(0, yMod, 0);
            particles[^1].Pos = hook.transform.position;
            if (LineLength <= 0.005f)
            {
                hook.gameObject.SetActive(false);
                hitWater = false;
                throwing = false;
                lineRenderer.enabled = false;
                doOnce = true;
                TransferFishes();

            }
        }

        if (!throwStarted) return;
        hook.gameObject.SetActive(true);
        if (!doOnce)
        {
            LineLength += 0.5f;
            LineLength = Mathf.Clamp(LineLength, 0, 1);

            if (lineRenderer.GetPosition(particles.Length - 1).y < 0f && !hitWater)
            {
                tmpHit = lineRenderer.GetPosition(particles.Length - 1);
                hook.transform.position = lineRenderer.GetPosition(particles.Length - 1);
                hitWater = true;
                throwStarted = false;
                doOnce = true;
            }

            particles[^ 1].Pos = hook.transform.position;

        }

        if (!doOnce) return;
        for (var i = 0; i < particles.Length; i++)
        {
            lineRenderer.SetPosition(i, topOfFishingLine.position);
        }

        hook.velocity = Vector3.zero;
        hook.transform.position = player.position + Vector3.up * 2;
        doOnce = false;
        hook.AddForce(player.forward * 500);
        particles[^1].Pos = hook.transform.position;
    }

    private void TransferFishes()
    {
        UIHandler.SetTextFinalText("Congratulations");
        doOnce = true;
    }

    private static void Verlet(LinePoints p, float dt)
    {
        var temp = p.Pos;
        p.Pos += p.Pos - p.OldPos + (-p.Acceleration * (dt * -dt));
        p.OldPos = temp;
    }

    private static void PoleConstraint(LinePoints p1, LinePoints p2, float restLength)
    {
        var delta = p2.Pos - p1.Pos;
        var deltaLength = delta.magnitude;
        var diff = (deltaLength - restLength) / deltaLength;

        p1.Pos += delta * (diff * 0.5f);
        p2.Pos -= delta * (diff * 0.5f);
    }
}