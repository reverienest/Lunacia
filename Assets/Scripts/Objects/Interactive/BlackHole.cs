using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;

public class BlackHole : MonoBehaviour
{
    public float particle_angular_speed, particle_radial_speed;
    private bool counter_clockwise;
    public Gradient normalGlow, wakingGlow;
    public float attraction;
    [HideInInspector]
    public bool inWakingSight;
    public enum ForceMode_
    {
        constant,
        inverse,
        inverseSqr
    }
    public ForceMode_ attraction_mode;

    private ParticleSystem pSystem;
    private ParticleSystem.TrailModule pSystemTrail;
    private ParticleSystem.ShapeModule pSystemShape;
    private ArrayList attracting;
    private SpriteRenderer blackholeSprite;
    private float P_SYSTEM_SHAPE_RADIUS, P_SYSTEM_SHAPE_RADIUS_INNER;

    // Start is called before the first frame update
    void Start()
    {
        //connect to graphic components
        pSystem = GetComponent<ParticleSystem>();
        pSystemTrail = pSystem.trails;
        pSystemTrail.colorOverLifetime = inWakingSight ?  wakingGlow : normalGlow;
        pSystemShape = pSystem.shape;
        P_SYSTEM_SHAPE_RADIUS = pSystemShape.radius;
        P_SYSTEM_SHAPE_RADIUS_INNER = P_SYSTEM_SHAPE_RADIUS * pSystemShape.radiusThickness;

        blackholeSprite = GetComponent<SpriteRenderer>();
        blackholeSprite.color = inWakingSight ? wakingGlow.colorKeys[0].color : normalGlow.colorKeys[1].color;

        counter_clockwise = !inWakingSight;

        attracting = new ArrayList();

        MessageBroker.Instance.WakingSightModeTopic += consumeWSMessage;
    }

    private void consumeWSMessage(object sender, WakingSightModeEventArgs wsModeChange)
    {
        switch (wsModeChange.ActiveMode)
        {
            case 1:
                inWakingSight = true;
                counter_clockwise = false;

                blackholeSprite.color = wakingGlow.colorKeys[0].color;
                pSystemTrail.colorOverLifetime = wakingGlow;

                pSystemShape.radius = P_SYSTEM_SHAPE_RADIUS_INNER;
                break;
            case 0:
                inWakingSight = false;
                counter_clockwise = true;

                blackholeSprite.color = normalGlow.colorKeys[0].color;
                pSystemTrail.colorOverLifetime = normalGlow;

                pSystemShape.radius = P_SYSTEM_SHAPE_RADIUS;
                break;
        }
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

            switch (attraction_mode)
            {
                case ForceMode_.constant:
                    force = attraction * v2FromAngle(angleFromV2(force));
                    break;
                case ForceMode_.inverse:
                    force = attraction * v2FromAngle(angleFromV2(force)) / Mathf.Max(0.1f, force.magnitude);
                    break;
                case ForceMode_.inverseSqr:
                    force = attraction* v2FromAngle(angleFromV2(force)) / Mathf.Max(0.01f, force.magnitude * force.magnitude);
                    break;
            }
            //print("applying" + force);
            rigid.AddForce((inWakingSight ? -1.0f : 1.0f) * force);
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
            /*
            if (particles[i].position.magnitude < pSystem.shape.radius * 0.1)
            {
                particles[i].remainingLifetime = 0;
                continue;
            }*/
            //particles[i].position *= 0.9f;
            particles[i].velocity = particle_angular_speed * rotateV3(
                v3FromAngle(angleFromV2(particles[i].position)),
                (counter_clockwise ? -0.5f * Mathf.PI : 0.5f * Mathf.PI)
                ) + (inWakingSight ? -1.0f : 1.0f) * particle_radial_speed * particles[i].position;
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
