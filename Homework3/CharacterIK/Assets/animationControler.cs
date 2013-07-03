using UnityEngine;
using System.Collections;

public class animationControler : MonoBehaviour
 {
    private float Speed = 0.01f;
    private float TurnSpeed = 20.0f;

    private GameObject AimPoint;

    public Transform ArmR;
    public Transform ForearmR;
    public Transform HandR;
    public Matrix4x4 RightHandMatrix;

    public Transform ArmL;
    public Transform ForearmL;
    public Transform HandL;
    public Matrix4x4 LeftHandMatrix;

    public Transform Gun;

    IK1JointAnalytic IKSolver = new IK1JointAnalytic();

    Matrix4x4 ChangeCoordinateSystems(Transform from, Transform to)
    {
        return to.worldToLocalMatrix * from.localToWorldMatrix;
    }

    Quaternion QuatFromMatrix(Matrix4x4 theMatrix)
    {

        float x,y,z,w;

        w = Mathf.Sqrt(Mathf.Max(0, 1.0f + theMatrix.m00 + theMatrix.m11+ theMatrix.m22 ) ) / 2.0f;
        x = Mathf.Sqrt(Mathf.Max(0, 1.0f + theMatrix.m00 - theMatrix.m11 - theMatrix.m22)) / 2.0f;
        y = Mathf.Sqrt(Mathf.Max(0, 1.0f - theMatrix.m00 + theMatrix.m11 - theMatrix.m22) ) / 2.0f;
        z = Mathf.Sqrt(Mathf.Max(0, 1.0f - theMatrix.m00 - theMatrix.m11 + theMatrix.m22 ) ) / 2.0f;

        x *= Mathf.Sign(x * ( theMatrix.m21 - theMatrix.m12 ) );
        y *= Mathf.Sign(y * ( theMatrix.m02 - theMatrix.m02 ) );
        z *= Mathf.Sign(z * (theMatrix.m10 - theMatrix.m01));

        return new Quaternion(x,y,z,w);
    }

    void Awake()
    {
        animation.wrapMode = WrapMode.Loop;
        animation.Play("Idle");

        AimPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        AimPoint.renderer.material.color = Color.magenta;
        Destroy(AimPoint.collider);

        animation.Play("AimStraight");
    }

	void Update () 
	{
        if (Input.GetMouseButton(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit, 100f))
            {
                AimPoint.transform.position = hit.point;
            }
        }

        Vector3 dir = AimPoint.transform.position - this.transform.position;
        float angleToTarget = Vector3.Angle(dir, this.transform.up);
        if (angleToTarget < 60)
            animation.CrossFade("AimUp");
        else if (angleToTarget < 80)
            animation.CrossFade("AimStraight");
        else
            animation.CrossFade("AimDown");

        dir.Set(dir.x, 0, dir.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);

        if (Input.GetKey("up"))
        {
            animation.CrossFade("WalkForward");
            transform.position += transform.forward * Speed;
        }
        else if (Input.GetKey("down"))
        {
            animation.CrossFade("WalkBackward");
            transform.position -= transform.forward * Speed;
        }
        else if (Input.GetKey("right shift"))
        {
            animation.CrossFade("RunForward");
            Speed = 0.1f;
            transform.position += transform.forward * Speed;
        }
        else if (Input.GetKey("down"))
        {
            animation.CrossFade("WalkBackward");
            transform.position -= transform.forward * Speed;
        }


        if (Input.GetKey("left"))
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * TurnSpeed);
        }
        else if (Input.GetKey("right"))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * TurnSpeed);
        }

        if (Input.GetKeyUp("up") || Input.GetKeyUp("down") || Input.GetKeyUp("right shift"))
        {
            animation.CrossFade("AimStraight");
        }
	}

    void LateUpdate()
    {
        RightHandMatrix = ChangeCoordinateSystems(HandR, Gun);
        LeftHandMatrix = ChangeCoordinateSystems(HandL, Gun);

        Gun.LookAt(AimPoint.transform.position);
        Gun.transform.Rotate(Vector3.up, 90);

        //Right arm
        Vector3 desiredRightWristPosition = (Gun.localToWorldMatrix * RightHandMatrix).MultiplyPoint3x4 (Vector3.zero);
        Transform[] bonesR = new Transform[3];

        bonesR[0]=ArmR;
        bonesR[1]=ForearmR;
        bonesR[2]=HandR;

        IKSolver.Solve( bonesR, desiredRightWristPosition );

        HandR.rotation = QuatFromMatrix(Gun.localToWorldMatrix * RightHandMatrix);
        
        //Left arm
        Vector3 desiredLeftWristPosition = (Gun.localToWorldMatrix * LeftHandMatrix).MultiplyPoint3x4 (Vector3.zero);

        Transform[] bonesL = new Transform[3];
        bonesL[0] = ArmL;
        bonesL[1]  =ForearmL;
        bonesL[2] = HandL;

        IKSolver.Solve( bonesL, desiredLeftWristPosition );
        HandL.rotation = QuatFromMatrix(Gun.localToWorldMatrix * RightHandMatrix);
    }
}
