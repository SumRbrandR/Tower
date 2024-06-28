using Godot;
using System;

public partial class JumpThruDoor : Door
{
	Sprite2D sprite;
	CollisionShape2D collisionShape;
	StaticBody2D characterBody;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		sprite = GetNode<Sprite2D>("Sprite2D");
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		characterBody = GetNode<StaticBody2D>(GetPath());
        if (!characterBody.GetCollisionLayerValue(1))
        {
            characterBody.SetCollisionLayerValue(1, true);
        }
    }

    public override bool AttemptToOpen()
    {
		if (!base.AttemptToOpen())
		{
			return false;
		}
		if (opened)
		{

			if (characterBody.GetCollisionLayerValue(1))
			{
				characterBody.SetCollisionLayerValue(1, false);
			}

			sprite.Frame = 1;
			collisionShape.SetDeferred("one_way_collision", true);
		}
		return true;
    }

    public override bool AttemptToClose()
    {
		if (!base.AttemptToClose())
		{
			return false;
		}

		if(!opened)
		{
            if (!characterBody.GetCollisionLayerValue(1))
            {
                characterBody.SetCollisionLayerValue(1, true);
            }
            sprite.Frame = 0;
			collisionShape.SetDeferred("one_way_collision", false);
        }
		return true;
    }
}
