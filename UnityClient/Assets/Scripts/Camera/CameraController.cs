using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public Rigidbody TargetRigid;
    [SerializeField] float distanceToTarget = 5;
    public float Angle = 45f;
    public float Speed = 1;
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
    Vector3 offset;
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
            //float x = transform.position.x - (NewPos.x + Target.position.x);
            //float z = transform.position.z - (NewPos.z + Target.position.z);
            //if (x > 5)
            //{
            //    transform.position += new Vector3(5 - x, 0, 0);
            //}
            //else if (x < -5)
            //{
            //    transform.position += new Vector3(-5 - x, 0, 0);
            //}
            //if (z > 5)
            //{
            //    transform.position += new Vector3(0, 0, 5 - z);
            //}
            //else if (z < -5)
            //{
            //    transform.position += new Vector3(0, 0, -5 - z);
            //}
            //if (x > -5 && x < 5 && z > -5 && z < 5)
            {
                if(TargetRigid != null)
                {
                    offset.x = Mathf.Lerp(offset.x, Mathf.Clamp((TargetRigid.velocity).x, -1, 1), Time.deltaTime * Speed);
                    offset.z = Mathf.Lerp(offset.z, Mathf.Clamp((TargetRigid.velocity).z, -1, 1), Time.deltaTime * Speed);
                }

                Vector3 newPos = new Vector3(0, NewPos.y + Target.position.y, 0);
                newPos.z = Mathf.Lerp(transform.position.z, NewPos.z + Target.transform.position.z, Time.deltaTime * Speed);
                newPos.x = Mathf.Lerp(transform.position.x, NewPos.x + Target.transform.position.x, Time.deltaTime * Speed);
                transform.position = NewPos + Target.position + offset;
            }
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
