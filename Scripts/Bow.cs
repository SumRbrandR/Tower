using Godot;
using System;
using System.Reflection.Metadata;

public partial class Bow : Node, ITool
{
    public bool useRelease { get; } = true;
    public bool charged { get; private set; } = false;
    public string name { get; } = "Bow";


    int arrowCount = 99999;
	PackedScene projectile;
	Marker2D arrowSpawnPoint;
	AnimatedSprite2D linkSprite;
    player link;
    public bool animating {  get; private set; } 
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        projectile = GD.Load<PackedScene>("res://Scenes/arrow.tscn");
		arrowSpawnPoint = (Marker2D)GetNode<Marker2D>(new NodePath("BowSpawner"));//Owner.GetNode<Marker2D>(new NodePath("ProjectileSpawn"));
	}

    void Charge()
    {
        if (linkSprite.Animation == "BowDraw" || linkSprite.Animation == "BowDrawWalk")
        {
            charged = true;
            linkSprite.Play("BowHold");
        }
        else if (linkSprite.Animation == "BowDrawVert" || linkSprite.Animation == "BowDrawWalkVert")
        {
            charged = true;
            linkSprite.Play("BowHoldVert");
        }
        else if (linkSprite.Animation == "BowHold" || linkSprite.Animation == "BowHoldVert")
        {
            animating = false;
        }
    }

    void ShootH()
    {
        bool flipped = linkSprite.FlipH;


        arrowSpawnPoint.Set("position", flipped ? new Vector2(6, 2) : new Vector2(-6, 2));


        var arrow = projectile.Instantiate<Projectile>();
        arrow.shootDirection = Vector2.Right;

        arrow.GlobalPosition = arrowSpawnPoint.GlobalPosition;
        arrow.GlobalRotation = flipped ? Mathf.DegToRad(180f) : Mathf.DegToRad(0f);
        arrow.speed *= flipped ? 1 : -1;

        GetTree().Root.AddChild(arrow);

        GD.Print("Used Bow");
        link.usingTool = false;
        charged = false;
    }
    void ShootV()
    {
        arrowSpawnPoint.Set("position", new Vector2(0,-6));


        var arrow = projectile.Instantiate<Projectile>();
        arrow.shootDirection = Vector2.Up;
        arrow.GlobalPosition = arrowSpawnPoint.GlobalPosition;
        arrow.GlobalRotation = Mathf.DegToRad(90f);

        GetTree().Root.AddChild(arrow);

        GD.Print("Used Bow");
        link.usingTool = false;
        charged = false;
    }
	void Shoot(Vector2 direction)
	{
        if (linkSprite.Animation == new StringName("BowShoot") && direction.Y == 0)
            ShootH();
        else if (linkSprite.Animation == new StringName("BowShootVert") && direction.Y < 0)
            ShootV();
    }

	public bool Identify()
	{
		GD.Print("I am Bow");
		return true;
	}

	public void Use(Vector2 direction)
	{
        if (charged)
        {
            if(linkSprite.Animation == "BowHoldVert"|| linkSprite.Animation == "BowWalkVert")
            {
                linkSprite.Play("BowShootVert");
                ShootV();
            }
            else if(linkSprite.Animation == "BowHold" || linkSprite.Animation == "BowWalk")
            {
                linkSprite.Play("BowShoot");
                ShootH();
            }

        }
            //Shoot();
	}

    public void PreUse(Vector2 direction)
    {
        bool isWalking = direction.X != 0 ? true : false;
        if (isWalking)
        {
            if (direction.Y < 0)
            {
                linkSprite.Play("BowDrawWalkVert");
            }
            else
            {
                linkSprite.Play("BowDrawWalk");
            }
        }
        else if (direction.Y < 0)
        {
            linkSprite.Play("BowDrawVert");
        }
        else
        {
            linkSprite.Play("BowDraw");
        }
            animating = true;
    }

    public void WalkWhileUseAnim(Vector2 direction)
    {
        if(direction.Y < 0)
        {
            linkSprite.Play("BowWalkVert");
        }
        else
        {
            linkSprite.Play("BowWalk");
        }
    }

    public void UpdateUseDirection(Vector2 direction) 
    {
        if (direction.Y < 0 && direction.X == 0)
        {
            linkSprite.Play("BowHoldVert");
        }
        else if (direction.Y >= 0 && direction.X == 0)
        {
            linkSprite.Play("BowHold");
        }
        

    }

    public void SetupTool(AnimatedSprite2D character, player playerLink)
    {
        if (link == null)
        {
            link = playerLink;
        }
        if (linkSprite == null)
        {
            GD.Print("Bow Animator Setup");
            linkSprite = character;
            linkSprite.AnimationFinished += () => Charge();

        }

    }

}
