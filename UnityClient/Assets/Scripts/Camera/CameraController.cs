using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    [SerializeField] float distanceToTarget = 5;
    public float Angle = 45f;
    public float DistanceToTarget
    {
        get
        {
            return distanceToTarget;
        }
        set
        {
            distanceToTarget = value;
        }
    }
    public bool DrawDebug = false;
    float radius;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetCameraPos();
    }
    public void SetCameraPos()
    {
        Vector3 NewPos = Vector3.zero;
        Vector3 Rot = transform.eulerAngles;


        radius = distanceToTarget * Mathf.Tan((Angle + 90) * Mathf.Deg2Rad);

        NewPos.z = Mathf.Cos(Rot.y * Mathf.Deg2Rad) * radius;
        NewPos.y = distanceToTarget;
        NewPos.x = Mathf.Sin(Rot.y * Mathf.Deg2Rad) * radius;
        transform.rotation = Quaternion.Euler(Angle, Rot.y, Rot.z);
        if (Target != null)
        {
            transform.position = NewPos + Target.position;
        }
        else
        {
            transform.position = NewPos;
        }
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
