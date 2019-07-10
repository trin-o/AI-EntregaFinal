using System.Collections;
using UnityEngine;

public class EnemyStatus : Status
{
    [Tooltip("Scripts a activar o desactivar si esta o no en la pantalla")]
    public MonoBehaviour[] scripts;
    [SerializeField] bool Unbreakable = false;
    SpriteRenderer spr;
    bool prevIsVisible;
    bool invulnerable = false;
    float damageCooldown = 0.0f;

    public GameObject dust;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        for (int i = 0; i < scripts.Length; i++)
        {
            scripts[i].enabled = false;
        }

    }

    private void Update()
    {
        if (prevIsVisible != spr.IsVisibleFrom(Camera.main))
        {
            prevIsVisible = spr.IsVisibleFrom(Camera.main);
            for (int i = 0; i < scripts.Length; i++)
            {
                scripts[i].enabled = prevIsVisible;
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D info)
    {
        if (Unbreakable) return;
        if (info.CompareTag("Player/Bullet"))
            TakeDamage(info.transform, 1, 0.025f);
    }

    void HealthColor()
    {
        spr.color = new Color(1, (currentHP / (float)maxHP) + 0.25f, (currentHP / (float)maxHP) + 0.25f, 1);
    }

    public void TakeDamage(Transform obj, int damage, float time)
    {
        Dust(obj);

        if (!invulnerable && gameObject)
        {
            if (currentHP > 0)
            {
                currentHP -= damage;
                damageCooldown = time;
                invulnerable = true;
                HealthColor();
                StartCoroutine(ResetCooldown());
            }
            else if (currentHP <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        StopCoroutine(ResetCooldown());
        invulnerable = false;
    }

    void Dust(Transform obj)
    {
        GameObject temp = Instantiate(dust, obj.position, obj.rotation);
        Destroy(temp, 1.5f);
    }
}
