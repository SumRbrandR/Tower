using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyWanderState : EnemyState
{
    


    public override void SetUp(Dictionary<string, object> message)
    {
        base.SetUp(message);
        body.pathfinder.PathfindEnd += ()=> EndWander();
    
    }

    // Called when the node enters the scene tree for the first time.
    public override void OnStart(Dictionary<string, object> message)
    {
        base.OnStart(message);
        animator.Play("Walk");
         
        float wanderDirection = GD.RandRange(-160, 160);
        Vector2 goTo = new Vector2(wanderDirection, -Math.Abs(wanderDirection));
        body.pathfinder.CreateAndGoToPath(goTo);
    }

    void EndWander()
    {
        GD.Print("End wander");
        machine.ChangeState("EnemyIdleState", null);
    }

    public override void UpdateState(float delta)
    {
        //moveDirection = wanderDirection;
        base.UpdateState(delta);


    }

    public override void OnExit(string nextState)
    {
        base.OnExit(nextState);
    }

}
