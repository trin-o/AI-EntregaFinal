using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : Status
{
    [Tooltip("Scripts a activar o desactivar si esta o no en la pantalla")]
    public MonoBehaviour[] scripts;
    public Renderer rend;
    bool prevIsVisible;
    [SerializeField] Material mat;

    private void Start()
    {
        for (int i = 0; i < scripts.Length; i++)
        {
            scripts[i].enabled = false;
        }
        mat.color = new Color(0, mat.color.g, mat.color.b, 1);
    }

    private void Update()
    {
        if (prevIsVisible != rend.IsVisibleFrom(Camera.main))
        {
            prevIsVisible = rend.IsVisibleFrom(Camera.main);
            for (int i = 0; i < scripts.Length; i++)
            {
                scripts[i].enabled = prevIsVisible;
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D info)
    {
        if (info.CompareTag("Player/Bullet"))
            TakeDamage(info.transform, 1, 0.025f);
    }

    void HealthColor()
    {
        mat.color = new Color(((maxHP - currentHP) / (float)maxHP), mat.color.g, mat.color.b, 1);
    }

    public void TakeDamage(Transform obj, int damage, float time)
    {
        if (currentHP > 0)
        {
            currentHP -= damage;
            HealthColor();
        }
        else if (currentHP <= 0)
        {
            GameController.GC.state = GAME_STATE.FIN;
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 3;
            rb.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
            enabled = false;
            GameObject.Destroy(gameObject, 1.5f);
        }
    }

}
