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
    public float windMax, velEffectMax, windEffectMag;

    private float[] axial; //axial deviation away from the main shape 
    private Vector2[] steps; //main shape of hair strand (denote by angle of each segment)

    private float timer = 0.0f;
    private Vector2 pos, vel;
    private float windTransProgress, windEffect = 0.0f;

    private LineRenderer renderer_;
    private Rigidbody2D rigid;
    private SpriteRenderer hairBlob;
    private WindDetector wd;

    private PlayerBody bodyScript;

    private float nonStaticVelAng;

    private Color hairColor;

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
        renderer_ = GetComponent<LineRenderer>();
        rigid = GetComponentInParent<Rigidbody2D>();
        bodyScript = GetComponent<PlayerBody>();
        hairBlob = transform.GetChild(0).GetComponent<SpriteRenderer>();
        wd = GetComponentInParent<WindDetector>();
        hairColor = renderer_.startColor;

        hairBlob.color = renderer_.startColor;
        hairBlob.transform.localScale = new Vector3(blobSize, blobSize, blobSize);
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
        float[] temp = { length, spread, freq }; // save original for later use
        bool resumeSavedWindEffect = false;
        float windEffectTemp = windEffect;


        if (wd.wind.magnitude != 0) { // transition period

            windEffect = windEffectMag * Mathf.Min(wd.wind.magnitude, windMax) / windMax; // magnitude of effect
            windTransProgress += Mathf.Sign(vel.x * wd.wind.x + vel.y * wd.wind.y) * Time.deltaTime;
            windTransProgress = Mathf.Min(Mathf.Abs(windTransProgress), 0.5f) * Mathf.Sign(windTransProgress);

            windEffect *= windTransProgress / 0.5f;
        }
        else // reverse transition period
        {
            bool startOutAsPositive = Mathf.Sign(windTransProgress) > 0;
            windTransProgress -= Mathf.Sign(windTransProgress) * Time.deltaTime;
            if (Mathf.Sign(windTransProgress) <= 0 == startOutAsPositive)
            {
                windTransProgress = 0.0f;
                windEffect = 0.0f;
            }
            else
            {
                windEffect *= windTransProgress / 0.5f;
                resumeSavedWindEffect = true;
            }
        }
        //print(windTransProgress);

        length *= 1 - windEffect;
        spread *= 1 + windEffect;
        freq *= 1 + Mathf.Abs(windEffect);

        if (resumeSavedWindEffect)
        {
            windEffect = windEffectTemp;
        }

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

        pos_temp += bodyScript.headPos;
        blob_pos_temp += bodyScript.headPos;
        equi_temp = new Vector2((bodyScript.facingLeft ? 1 : -1), 0);

        equi_temp = lerp(equi_temp, wd.wind, Mathf.Min(wd.wind.magnitude, windMax), windMax);

        Vector3[] spline = new Vector3[num_steps+1];
        spline[0] = new Vector2(pos_temp.x, pos_temp.y);
        steps[0] = lerp(
                equi_temp, steps[0],
                Mathf.Min(vel.magnitude, velEffectMax), velEffectMax
            );
        steps[0].Normalize();

        for(int i = 0; i < num_steps; i++)
        {
            pos_temp += steps[i] * length;
            Vector3 perp = new Vector2(-1 * steps[i].y, steps[i].x);
            spline[i + 1] = new Vector3(pos_temp.x, pos_temp.y) + perp * axial[i] * spread * i;
        }

        // draw hair in line renderer
        renderer_.positionCount = num_steps + 1;
        renderer_.SetPositions(spline);
        if (bodyScript.bodyGlowing)
        {
            renderer_.startColor = Color.white;
            renderer_.endColor = Color.white;

            hairBlob.color = Color.white;
        }
        else
        {
            renderer_.startColor = hairColor;
            renderer_.endColor = hairColor;

            hairBlob.color = hairColor;
        }

        Color cTemp = renderer_.startColor;
        cTemp.a = bodyScript.playerSpriteAlpha;
        renderer_.startColor = cTemp;
        renderer_.endColor = cTemp;

        Color cBlobTemp = hairBlob.color;
        cBlobTemp.a = bodyScript.playerSpriteAlpha;
        hairBlob.color = cBlobTemp;

        hairBlob.transform.position = blob_pos_temp;

        length = temp[0];
        spread = temp[1];
        freq = temp[2];

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
