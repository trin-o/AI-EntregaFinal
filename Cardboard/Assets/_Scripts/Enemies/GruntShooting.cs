using UnityEngine;

public class GruntShooting : Shooting
{
    protected override void Update()
    {
        if (GameController.GC.state == GAME_STATE.JUGANDO)
        {
            base.Update();
            ShootAllTurrets();
        }
    }
}