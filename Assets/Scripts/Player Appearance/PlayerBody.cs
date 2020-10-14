using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public bool facingLeft = false;
    public Vector2 headPos;

    public float animationSpeed;
    private float frameTimer;

    private Rigidbody2D rigid;
    private SpriteRenderer renderer_;
    private PlayerController controller;
    private PlayerHair hairScript;

    //all modes that the character can behave under
    //bounce, idle, and loop are all just modes of playing the animation
    public enum AnimationModeIndex
    {
        idle = 0,
        accel = 1,
        hover = 2,
        deccel = 3,
    }

    public readonly CharAnimMode[] AnimationList = {
        new BounceMode("idle_sheet", new Vector2[] {
            new Vector2(0, .33f), new Vector2(0, .31f), new Vector2(0, .29f)
        }),
        new OneWayMode("accel_sheet", new Vector2[] {
            new Vector2(0, .3f), new Vector2(0, .3f), new Vector2(0, .3f), new Vector2(0, .3f), new Vector2(0, .3f)
        }),
        new BounceMode("hover_sheet", new Vector2[] {
            new Vector2(0, .3f), new Vector2(0, .3f), new Vector2(0, .3f)
        }),
        new OneWayMode("accel_sheet", new Vector2[] {
            new Vector2(0, .3f), new Vector2(0, .3f), new Vector2(0, .3f), new Vector2(0, .3f), new Vector2(0, .3f)
        }, -1)
     };

    private AnimationModeIndex curr;
    private AnimationModeIndex pending;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
        renderer_ = GetComponent<SpriteRenderer>();
        controller = GetComponentInParent<PlayerController>();
        hairScript = GetComponent<PlayerHair>();

        foreach (CharAnimMode i in AnimationList)
        {
            i.Load();
        }

        vTemp = new Vector2(0, 0);
        curr = AnimationModeIndex.idle;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rigid.velocity;

        //determine which horizontal direction the character is facing
        if (!Mathf.Approximately(vel.x, 0.0f))
        {
            facingLeft = vel.x < 0;
            renderer_.flipX = vel.x < 0;
            facingLeft = vel.x < 0.0f;
        }

        float vel_gauge = hairScript.velEffectMax;

        //determine the current state of the character based on her movement
        if (controller.intentionalForce)
        { 
            if (vel.magnitude > vel_gauge)
            {
                pending = AnimationModeIndex.hover;
            }
            else
            {
                pending = AnimationModeIndex.accel;
            }
        }
        else
        {
            if (basicallyEqual(vel, Vector2.zero) || curr.Equals(AnimationModeIndex.idle))
            {
                pending = AnimationModeIndex.idle;
            }
            else
            {
                pending = AnimationModeIndex.deccel;
            }
        }

        frameTimer += Time.deltaTime;
        //switch the animation state if needed
        if (!pending.Equals(curr) && AnimationList[(int)curr].isComplete())
        {
            //switch to new animation
            //print("switch from " + curr + " to " + pending);
            curr = pending;
            AnimationList[(int)curr].Init();
            frameTimer = 0;
        }
        else if (curr.Equals(AnimationModeIndex.deccel) && AnimationList[(int)curr].isComplete())
            //deceleration automatically changes into idle-ness after completion
        {
            curr = AnimationModeIndex.idle;
            AnimationList[(int)curr].Init();
            frameTimer = 0;
        }

        //update the frame of the current animation state
        if (frameTimer > (1.0f / animationSpeed))
        {
            renderer_.sprite = AnimationList[(int)curr].next();
            frameTimer = frameTimer % (1.0f / animationSpeed);
            headPos = AnimationList[(int)curr].getHeadLocation();
        }

        vTemp = vel;
    }

    public bool basicallyEqual(Vector2 v1, Vector2 v2)
    {
        return Mathf.Approximately(v1.x, v2.x) &&
            Mathf.Approximately(v1.y, v2.y);
    }
}
public abstract class CharAnimMode
{
    protected Sprite[] content;
    protected int index;
    protected int direction;
    protected readonly int dir_initial;
    protected Vector2[] headTracker;

    protected string prefix;

    public CharAnimMode(string prefix, Vector2[] headTracker, int direction)
    {
        this.headTracker = headTracker;
        this.prefix = prefix;

        this.direction = direction;
        dir_initial = direction;
    }

    //load the sprites located in the resources
    public void Load()
    {
        Object[] spriteList = Resources.LoadAll(prefix);
        content = new Sprite[spriteList.Length - 1];
        for (int i = 0; i < content.Length; i++)
        {
            content[i] = spriteList[i + 1] as Sprite;
        }

        Init();
    }

    public Vector2 getHeadLocation()
    {
        return headTracker[index];
    }

    public void Init()
    {
        direction = dir_initial;
        if (direction == -1)
        {
            index = content.Length - 1;
        }
        else
        {
            index = 0;
        }
    }
    abstract public Sprite next();
    abstract public bool isComplete();
}

//a bounce animation goes back and force
public class BounceMode : CharAnimMode
{

    public BounceMode(string prefix, Vector2[] headTracker) : base(prefix, headTracker, 1) { }
    public BounceMode(string prefix, Vector2[] headTracker, int direction) : base(prefix, headTracker, direction) { }

    override public Sprite next()
    {
        //Debug.Log(prefix + " " + index);
        if (direction == 1 && index == content.Length - 1)
        {
            direction = -1;
        }
        if (direction == -1 && index == 0)
        {
            direction = 1;
        }
        index += direction;
        return content[index];
    }

    override public bool isComplete()
    {
        return true;
    }

}

//a loop animation cycles
public class LoopMode : CharAnimMode
{
    public LoopMode(string prefix, Vector2[] headTracker) : base(prefix, headTracker, 1) { }

    public LoopMode(string prefix, Vector2[] headTracker, int direction) : base(prefix, headTracker, direction) { }

    override public Sprite next()
    {
        if (direction == 1)
        {
            if (index < content.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }
        else
        {
            if (index > 0)
            {
                index--;
            }
            else
            {
                index = content.Length;
            }
        }
        
        return content[index];
    }

    override public bool isComplete()
    {
        return true;
    }

}

//a one way animation goes "one way" and stops at the end
public class OneWayMode : CharAnimMode
{
    public OneWayMode(string prefix, Vector2[] headTracker) : base(prefix, headTracker, 1) { }

    public OneWayMode(string prefix, Vector2[] headTracker, int direction) : base(prefix, headTracker, direction) { }

    override public Sprite next()
    {
        //Debug.Log(prefix + " " + index + " " + direction);
        
        if (direction == 1 && index < content.Length - 1)
        {
            index++;
        }
        if (direction == -1 && index > 0)
        {
            index--;
        }
        return content[index];
    }

    override public bool isComplete()
    {
        return direction == 1 ? index == content.Length - 1 : index == 0;
    }

}