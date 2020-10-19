using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;

public class PlayerBody : MonoBehaviour
{
    [HideInInspector]
    public bool facingLeft;
    [HideInInspector]
    public Vector2 headPos;

    private Rigidbody2D rigid;
    private PlayerController controller;
    private SpriteRenderer renderer_;
    private Animator playerAnimator;

    [HideInInspector]
    public bool bodyGlowing = false;
    [HideInInspector]
    public float playerSpriteAlpha;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
        controller = GetComponentInParent<PlayerController>();
        renderer_ = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        MessageBroker.Instance.PlayerDeathTopic += consumePDMessage;
    }

    private void consumePDMessage(object sender, PlayerDeathEventArguments pdModeChange)
    {
        print("player is killed to death");
        playerAnimator.SetBool("PendingDeath", true);
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

        playerAnimator.SetBool("UnderIntentionalForce", controller.intentionalForce);
        playerSpriteAlpha = renderer_.color.a;
    }
}

/*deprecated
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

}*/