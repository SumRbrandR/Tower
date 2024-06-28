using Godot;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

public partial class EnemyDamageState : EnemyState
{
    [Export] float knockbackAmount = 150f;
    [Export] float damageWaitTime = 0.5f;

    public override void SetUp(Dictionary<string, object> message)
    {
        base.SetUp(message);
        animator.AnimationFinished += () => Destroy();

    }

   


    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
        GD.Print(Owner.Name + " is being damaged");
        int damage = (int)message["damage"];
        HitBox2D caller = (HitBox2D)message["hitBox"];
        
        logic.hitPoints -= damage;
        logic.isBusy = true;
        logic.takingDamage = true;
        //body.takingDamage = true;
        if (logic.hitPoints <= 0)
        {
            animator.Play("Death");
            
            
            // logic.hurtBox.SetDeferred("disabled", true);
            // logic.collisionBox.SetDeferred("disabled", true);
            // animator.Stop();

        }
        else
        {var direction = Mathf.Sign(logic.GlobalPosition.X - caller.GlobalPosition.X);

            animator.Play("Damage");

            var hitVelX = direction * knockbackAmount * 2;
            var hitVelY = -GD.RandRange(100, 200);



            logic.Velocity = new Vector2(hitVelX, hitVelY);


        }
    }

    void Destroy()
    {

        logic.isBusy = false;
        logic.takingDamage = false;
            if (animator.Animation == "Death")
            {
                logic.Destroy ();
            }
            else if (animator.Animation == "Damage")
            {
                if (!logic.canSee)
                {

                    machine.ChangeState("EnemyConfusedState", null);
                }
                else { machine.ChangeState("EnemyChaseState", null); }
            }

        
    }
}
