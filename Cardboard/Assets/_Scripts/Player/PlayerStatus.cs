using System.Collections;
using UnityEngine;

public enum PLAYER_STATE { CONTROLANDO, INACTIVO, MUERTO }
public class PlayerStatus : Status
{
    public PLAYER_STATE state;
    bool invulnerable = false;
    float damageCooldown = 0.0f;
    public GameObject dust;
    public GameObject healthGFX;

    void OnTriggerEnter2D(Collider2D info)
    {
        // Debug.Log(info);

        switch (info.tag)
        {
            case "Enemy":
                TakeDamage(info.transform, 20, 0.5f);
                info.gameObject.SetActive(false);
                break;
            case "Enemy/Misile":
                TakeDamage(info.transform, 4, 0.0f);
                info.gameObject.SetActive(false);
                break;
            case "Enemy/GruntBullet":
                TakeDamage(info.transform, 2, 0.0f);
                info.gameObject.SetActive(false);
                break;
            case "Enemy/Rock":
                TakeDamage(info.transform, 20, 0.5f);
                Push(info.transform);
                break;
            case "Player/Life":
                HealthValidate(info);
                break;
            default:
                break;
        }

        if (!info.CompareTag("Player/Bullet"))
        {
            GameController.GC.UpdateHealthBar(currentHP);
        }
    }


    // player
    // => damage
    public void TakeDamage(Transform obj, int damage, float time)
    {
        Dust(obj);
        if (!invulnerable)
        {
            if (currentHP > 0)
            {
                currentHP -= damage;
                damageCooldown = time;
                invulnerable = true;

                StartCoroutine(ResetCooldown());
            }
            else if (currentHP <= 0)
            {
                gameObject.SetActive(false);
                GameController.GC.EndGame();
            }
        }
    }

    void Push(Transform obj)
    {
        if (obj.position.x > transform.position.x)
        {
            transform.position += new Vector3(-1f, 0, 0);
        }

        if (obj.position.x < transform.position.x)
        {
            transform.position += new Vector3(1f, 0, 0);
        }

        if (obj.position.y > transform.position.y)
        {
            transform.position += new Vector3(0, -1f, 0);
        }

        if (obj.position.y < transform.position.y)
        {
            transform.position += new Vector3(0, 1f, 0);
        }
    }

    // => health
    public void TakeHealth(int life)
    {
        currentHP += life;

        if (currentHP >= 100)
        {
            currentHP = 100;
        }
    }

    public void HealthValidate(Collider2D info)
    {
        Vector3 offset = new Vector3(2, 0, 0);
        if (currentHP < 100)
        {
            TakeHealth(50);
            info.gameObject.SetActive(false);
            GameObject temp = Instantiate(healthGFX, info.transform.position - offset, info.transform.rotation, transform);
            Destroy(temp, 5f);
        }
    }

    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        invulnerable = false;
    }

    void Dust(Transform obj)
    {
        GameObject temp = Instantiate(dust, obj.position - Vector3.forward * 0.01f, obj.rotation);
        Destroy(temp, 1.5f);
    }
}
