using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHair : MonoBehaviour
{
    public int num_steps;

    public float length; //the stretchiness of the hair
    public float spread; //the amplitude of the hair's waving motion
    public float freq; //how fast the hair waves around
    public float stiffness; //the lower the softer the hair
    public float blobSize; //size of the hair blob behind the face
    public float windMax, velEffectMax;

    public Vector2 lShift, rShift;
    public Vector2 lBlobShift, rBlobShift;

    private float[] axial; //axial deviation away from the main shape 
    private Vector2[] steps; //main shape of hair strand (denote by angle of each segment)

    private float timer = 0.0f;
    private Vector2 pos, vel, f;

    private LineRenderer renderer;
    private Rigidbody2D rigid;
    private SpriteRenderer hairBlob;
    private WindDetector wd;

    private PlayerBody bodyScript;

    private float nonStaticVelAng;

    // Start is called before the first frame update
    void Start()
    {
        //initialization
        axial = new float[num_steps];
        for(int i = 0; i < num_steps; i++)
        {
            axial[i] = 0.0f;
        }
        steps = new Vector2[num_steps];
        for (int i = 0; i < num_steps; i++)
        {
            steps[i] = new Vector2(-1, 0.1f); //in third quadrant at beginning
        }
        //graphic components
        renderer = GetComponent<LineRenderer>();
        rigid = GetComponentInParent<Rigidbody2D>();
        bodyScript = GetComponent<PlayerBody>();
        hairBlob = transform.GetChild(0).GetComponent<SpriteRenderer>();
        wd = GetComponentInParent<WindDetector>();

        hairBlob.color = renderer.startColor;
        hairBlob.transform.localScale = new Vector3(blobSize, blobSize, blobSize);

    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision);
    }

    // Update is called once per frame
    void Update()
    {
        //general
        timer = timer + Time.deltaTime % 10000000000.0f; //just to prevent overflow
        pos = rigid.position;
        vel = rigid.velocity;

        float vel_ang;
        if (vel.magnitude != 0)
        {
            vel_ang = Mathf.Atan2(vel.y, vel.x);
            if (vel_ang < 0.0f)
            {
                vel_ang += 2 * Mathf.PI;
            }
            nonStaticVelAng = vel_ang;
        }
        else
        {
            vel_ang = nonStaticVelAng;
        }

        // update hair shape:
        // determine basic constants
        // iterate through shape
        for (int i = num_steps - 1; i > 0; i--)
        {
            axial[i] = lerp(axial[i], axial[i-1], Time.deltaTime * stiffness, 1.0f);
            steps[i] = lerp(steps[i], steps[i-1], Time.deltaTime * stiffness, 1.0f);
        }

        steps[0] = v2FromAngle(vel_ang + Mathf.PI);
        axial[0] = getAxial();//v2FromAngle(vel_ang + Mathf.PI/2);

        // form main curvature
        Vector2 pos_temp = new Vector2(pos.x, pos.y);
        Vector2 blob_pos_temp = new Vector2(pos.x, pos.y);

        Vector2 equi_temp;

        if (bodyScript.facingLeft)
        {
            pos_temp += lShift;
            blob_pos_temp += lBlobShift;

            //equilibrium position when facing left
            equi_temp = new Vector2(1, 0);
        }
        else
        {
            pos_temp += rShift;
            blob_pos_temp += rBlobShift;

            //equilibrium position when facing right
            equi_temp = new Vector2(-1, 0);
        }

        equi_temp = lerp(equi_temp, wd.wind, (wd.wind.magnitude / windMax) % 1.0f, 1.0f);

        Vector3[] spline = new Vector3[num_steps+1];
        spline[0] = new Vector2(pos_temp.x, pos_temp.y);
        steps[0] = v2FromAngle(
            lerp(equi_temp + steps[0], steps[0], (vel.magnitude / velEffectMax) % 1.0f, 1.0f)
        );

        for(int i = 0; i < num_steps; i++)
        {
            pos_temp += steps[i] * length;
            Vector3 perp = new Vector2(-1 * steps[i].y, steps[i].x);
            spline[i + 1] = new Vector3(pos_temp.x, pos_temp.y) + perp * axial[i] * spread * i;
        }

        // draw hair in line renderer
        renderer.positionCount = num_steps + 1;
        renderer.SetPositions(spline);

        hairBlob.transform.position = blob_pos_temp;
    }

    // formula for sine wave for hair strand
    float getAxial()
    {
        float t = timer * freq;
        return Mathf.Sin(t);
    }
    
    Vector2 lerp(Vector2 start, Vector2 end, float progress, float span)
    {
        return end * (progress / span) + start *((span - progress) / span);
    }
    float lerp(float start, float end, float progress, float span)
    {
        return end * (progress / span) + start * ((span - progress) / span);
    }

    Vector2 v2FromAngle(float ang)
    {
        return new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
    }

    Vector3 rotate(Vector2 target, float ang)
    {
        return new Vector3(Mathf.Cos(target.x) - Mathf.Sin(target.y),
            -1 * Mathf.Sin(target.x) + Mathf.Cos(target.y));
    }

}
