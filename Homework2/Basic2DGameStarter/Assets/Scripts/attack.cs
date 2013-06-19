using UnityEngine;
using System.Collections;

public class attack : MonoBehaviour
{
    public GameObject Target = null;
    public Vector3 Direction;

    public float MoveSpeed = 20;
    public float TurnSpeed = 4;

    public float AttackDistance = 60;
    public float RetreatDistance = 10;

    public GameObject ObjectToShoot;
    public Transform GunLocation;

    protected float LastShotTime = -1;

    public enum StateType
    {
        Attacking,
        Retreat,
    }

    public float ShotLifeTime = 10;
    public float MinAttacktime = 3;

    protected StateType State = StateType.Attacking;

    protected Vector3 StartingPostion;

    protected bool Turning = false;

	// Use this for initialization
	void Start ()
    {
        Target = GameObject.Find("Base");
        StartingPostion = this.transform.position;
	}

    void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), TurnSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);

     //   bool wasTurning = Turning;
        Turning = Vector3.Dot(transform.forward,Direction) < 0.0001;

    //    if (wasTurning != Turning)
      //      LastShotTime = Time.fixedTime - MinAttacktime + 1;
    }

    public float TargetAngle = 0;

    void Shoot()
    {
        Vector3 vecToTarget = this.transform.position - Target.transform.position;
        vecToTarget.Normalize();

        float angle = Vector3.Dot(this.transform.forward,-vecToTarget);

        TargetAngle = Mathf.Acos(angle) * Mathf.Rad2Deg;

        if (Turning || ObjectToShoot == null || LastShotTime + MinAttacktime > Time.fixedTime)
            return;

        if (TargetAngle < 35)
        {
            LastShotTime = Time.fixedTime;
            Transform t = this.transform;
            if (GunLocation != null)
                t = GunLocation;

            GameObject newShot = Instantiate(ObjectToShoot, t.position, t.rotation) as GameObject;
            if (newShot != null)
                Destroy(newShot, ShotLifeTime); // make them die
        }
    }

	// Update is called once per frame
	void Update () 
    {
        Direction = Target.transform.position - transform.position;

        if (Direction.magnitude > AttackDistance)
            State = StateType.Attacking;
        else if (Direction.magnitude < RetreatDistance)
            State = StateType.Retreat;

        switch (State)
        {
            case StateType.Attacking:
                Move();
                Shoot();
                break;

            case StateType.Retreat:
                Direction *= -1f;
                Move();
                break;
        }
  
	}
}
