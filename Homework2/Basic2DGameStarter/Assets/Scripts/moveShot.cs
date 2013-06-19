using UnityEngine;
using System.Collections;

public class moveShot : MonoBehaviour
{
    public float Speed = 30.0f;

    private GameObject Target;
    public GameObject ExplodesAs;

    protected ParticleSystem ParticleStream;

	// Use this for initialization
	void Start ()
    {
        Target = GameObject.Find("Base");
        ParticleStream = this.GetComponentInChildren(typeof(ParticleSystem)) as ParticleSystem;
	}

    void Explode()
    {
        if (ParticleStream != null)
        {
            ParticleStream.Stop();
            Destroy(ParticleStream);
        }

        if (ExplodesAs != null)
            Destroy(Instantiate(ExplodesAs, this.transform.position, this.transform.rotation), 1.0f);
        Destroy(gameObject);
        Destroy(this);
    }

	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.forward * (Speed * Time.deltaTime));

        float radius = Target.collider.bounds.size.x / 2;
        if (Vector3.Distance(Target.transform.position, this.transform.position) < radius)
            Explode();
	}
}
