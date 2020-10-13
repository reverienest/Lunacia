using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float iRadius, oRadius;
    public float particle_centri_force, particle_angular_speed, particle_radial_speed;
    public bool counter_clockwise;
    public float attraction;

    private ParticleSystem pSystem;
    private ArrayList attracting;

    // Start is called before the first frame update
    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
        attracting = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody2D rigid in attracting)
        {
            Vector2 force = (new Vector2(transform.position.x, transform.position.y) - rigid.position);

            //IMPLEMENT ACTUAL FORCE HERE, my focus was on the visuals and the below are just for demo purposes!

            //constant force
            //force = attraction * v2FromAngle(angleFromV2(force))
            //inverse force
            force = attraction * v2FromAngle(angleFromV2(force)) / Mathf.Max(0.1f, force.magnitude);
            //inverse square force
            //force = attraction * v2FromAngle(angleFromV2(force)) / Mathf.Max(0.01f, force.magnitude * force.magnitude);

            //print("applying" + force);
            rigid.AddForce(force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rigid = collision.GetComponent<Rigidbody2D>();
        if (rigid)
        {
            attracting.Add(rigid);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rigid = collision.GetComponent<Rigidbody2D>();
        if (rigid)
        {
            attracting.Remove(rigid);
        }
    }

    private void LateUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[pSystem.particleCount];
        pSystem.GetParticles(particles);

        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].position.magnitude < pSystem.shape.radius * 0.3)
            {
                particles[i].remainingLifetime = 0;
                continue;
            }
            //particles[i].position *= 0.9f;
            particles[i].velocity = particle_angular_speed * rotateV3(
                v3FromAngle(angleFromV2(particles[i].position)),
                (counter_clockwise ? -0.5f * Mathf.PI : 0.5f * Mathf.PI)
                ) + particle_radial_speed * particles[i].position;
        }

        pSystem.SetParticles(particles, particles.Length);
    }

    float angleFromV2(Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x);
    }

    Vector3 v3FromAngle(float theta)
    {
        return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
    }

    Vector2 v2FromAngle(float theta)
    {
        return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
    }

    Vector3 rotateV3(Vector3 v, float theta)
    {
        Vector3 v_ = new Vector3(
                Mathf.Cos(theta)*v.x + Mathf.Sin(theta)*v.y,
                Mathf.Sin(theta) * -1 * v.x + Mathf.Cos(theta) * v.y
            );
        return v_;
    }
}
