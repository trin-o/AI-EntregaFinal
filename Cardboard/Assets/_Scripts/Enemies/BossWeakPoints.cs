using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoints : Status
{
    [SerializeField] GameObject dust;
    bool invulnerable = false;
    float damageCooldown = 0.0f;
    Renderer rend;
    BossStatus boss;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        boss = transform.parent.parent.GetComponent<BossStatus>();
    }

    protected void OnTriggerEnter2D(Collider2D info)
    {
        if (info.CompareTag("Player/Bullet"))
            TakeDamage(info.transform, 1, 0.025f);
    }


    public void TakeDamage(Transform obj, int damage, float time)
    {
        Dust(obj);

        if (!invulnerable && boss.enabled)
        {
            boss.TakeDamage(obj, damage, time);
            damageCooldown = time;
            StartCoroutine(ResetCooldown());
        }
    }

    IEnumerator ResetCooldown()
    {
        invulnerable = true;
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
