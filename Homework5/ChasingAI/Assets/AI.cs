using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour
{
    public Transform Target = null;
    private float SeeRange = 100;
    private float ShootRange = 20.0f;
    private float KeepDistance = 10.0f;
    private float RotationSpeed = 5.0f;
    private float Speed = 0.01f;

    private float SightAngle = 30;

    public enum States
    {
        Patrol,
        Shooting,
        Pursue,
    }

    protected States State = States.Patrol;

	// Use this for initialization
	void Start ()
    {
        Patrol();
        this.animation["shoot"].wrapMode = WrapMode.Loop;
        this.animation["run"].wrapMode = WrapMode.Loop;
        this.animation["idle"].wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (CanSeeTarget())
        {
            if (CanShoot())
            {
                State = States.Shooting;
                animation.CrossFade("shoot");
                Speed = 0;
                Shoot();
            }
            else
            {
                State = States.Pursue;
                animation.CrossFade("run");
                Speed = 0.08f;
                Pursue();
            }
        }
        else
        {
            State = States.Pursue;
            if (animation.IsPlaying("idle"))
            {
                animation.Play("idle");
                Speed = 0;
            }
            Patrol();
        }
	}

    void Patrol()
    {
    }

    bool CanSeeTarget()
    {
        Vector3 directionToTarget = Target.position - transform.position;
        float angle = Vector3.Angle(directionToTarget, transform.forward);

        if (Vector3.Distance(transform.position, Target.position) > SeeRange || angle > SightAngle)
            return false;

        return true;
    }

    bool CanShoot()
    {
        if (Vector3.Distance(transform.position, Target.position) > ShootRange)
            return false;

        return true;
    }

    void Pursue()
    {
        Vector3 position = Target.position;
        Vector3 direction = position - transform.position;
        direction.Set(direction.x, 0, direction.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (direction.magnitude > KeepDistance)
        {
            direction = direction.normalized * Speed;
            transform.position += direction;
        }
    }

    void Shoot()
    {
        Vector3 position = Target.position;
        Vector3 direction = position - transform.position;
        direction.Set(direction.x, 0, direction.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
