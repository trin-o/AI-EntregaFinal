using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerShooting : Shooting
{
    PlayerStatus PS;

    void Awake()
    {
        PS = GetComponent<PlayerStatus>();
    }


    public new void Update()
    {
        switch (GameController.GC.state)
        {
            case GAME_STATE.JUGANDO:
                base.Update();
                if (PS.state == PLAYER_STATE.CONTROLANDO)
                {
                    for (int i = 0; i < Turrets.Length; i++)
                    {
                        Turrets[i].eulerAngles =
                            Vector3.forward *
                            Mathf.Sin(Time.time * 20) *
                            (i - 1 == 0 ? 0 : 2 * (i - 1));
                    }
                    ShootAllTurrets();
                }
                break;
        }
    }
}