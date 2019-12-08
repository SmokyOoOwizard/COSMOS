using UnityEngine;
using COSMOS.Prototype;
using COSMOS.Space;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float distanceToTarget = 5;
    public float Height = 2f;
    public bool DrawDebug = false;
    float radius;
    float disTarget;
    float h;
    [Header("PlanetZoom")]
    public AnimationCurve HByTime;
    public AnimationCurve DistanceByTime;
    public bool Zoom = false;
    public float HSpeed = 1;
    public float HAmount = 1;
    public float TargetH;
    public float DSpeed = 1;
    public float DistanceAmount = 1;
    public float TargetDistance;

    // Start is called before the first frame update
    void Start()
    {
        SolarSystemManager.EndLoadSystem += () => Log.Info("LOADED");
        SolarSystemManager.LoadSystem("TestName");
    }
    [ContextMenu("1")]
    public void T()
    {
        SolarSystemManager.LoadSystem("TestName");
    }
    [ContextMenu("2")]
    public void TT()
    {
        SolarSystemManager.LoadSystem("TestName2");
    }
    // Update is called once per frame
    void Update()
    {
        ZoomUpdate();
        SetCameraPos();
    }
    public void SetCameraPos()
    {
        Vector3 NewPos = Vector3.zero;
        Vector3 Rot = transform.eulerAngles;


        radius = disTarget;// * Mathf.Tan((Angle + 90) * Mathf.Deg2Rad);

        NewPos.z = Mathf.Cos(Rot.y * Mathf.Deg2Rad) * -radius;
        NewPos.y = h;
        NewPos.x = Mathf.Sin(Rot.y * Mathf.Deg2Rad) * -radius;
        Rot.x = Mathf.Atan(h / radius) * Mathf.Rad2Deg;
        if (float.IsNaN(Rot.x)) Rot.x = 0;
        transform.rotation = Quaternion.Euler(Rot.x, Rot.y, Rot.z);
        if (Target != null)
        {
            Vector3 newPos = new Vector3(0, NewPos.y + Target.position.y, 0);
            //newPos.z = Mathf.Lerp(transform.position.z, NewPos.z + Target.transform.position.z, Time.deltaTime * Speed);
            newPos.z = -NewPos.z + Target.position.z;
            //newPos.x = Mathf.Lerp(transform.position.x, NewPos.x + Target.transform.position.x, Time.deltaTime * Speed);
            newPos.x = NewPos.x + Target.position.x;
                
            transform.position = NewPos + Target.position;
        }
        else
        {
            transform.position = NewPos + Target.position;
        }
    }
    public void ZoomUpdate()
    {
        if (Zoom)
        {
            DistanceAmount += DSpeed * Time.deltaTime;
            HAmount += HSpeed * Time.deltaTime;
        }
        else
        {
            DistanceAmount -= DSpeed * Time.deltaTime;
            HAmount -= HSpeed * Time.deltaTime;
        }
        DistanceAmount = Mathf.Clamp01(DistanceAmount);
        HAmount = Mathf.Clamp01(HAmount);
        h = Mathf.Lerp(Height ,TargetH, HByTime.Evaluate(HAmount));
        disTarget = Mathf.Lerp(distanceToTarget, TargetDistance, DistanceByTime.Evaluate(DistanceAmount));
    }
    private void OnDrawGizmos()
    {
        if (this.enabled)
        {
            SetCameraPos();
        }
        if (DrawDebug)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(new Vector3(Target.position.x + radius, Target.position.y, transform.position.z), new Vector3(Target.position.x + radius, Target.position.y, Target.position.z));
            Gizmos.DrawLine(new Vector3(Target.position.x + radius, transform.position.y, transform.position.z), new Vector3(Target.position.x + radius, Target.position.y, Target.position.z));
            Gizmos.DrawLine(new Vector3(Target.position.x + radius, transform.position.y, transform.position.z), new Vector3(Target.position.x + radius, Target.position.y, transform.position.z));

            Gizmos.DrawLine(new Vector3(Target.position.x + radius, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y, transform.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, Target.position.y, transform.position.z));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Target.position);

            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.Scale(new Vector3(1, 0, 1));
            Gizmos.DrawWireSphere(Target.position, radius);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}
