using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Transform[] Turrets;
    [SerializeField] float bulletSpeed;
    [SerializeField] float coolDownBetweenBullets;
    [SerializeField] int amountPerBurst = 1;
    [SerializeField] float coolDownBetweenBursts;
    protected float bulletsTimer;
    protected float burstsTimer;
    protected int burstCounter = 0;

    [Header("Pooling Settings")]
    [SerializeField] protected int amountToPoolPerTurret;
    [SerializeField] protected GameObject objectToPool;
    protected List<Bullet> pooledObjects;
    protected Transform poolParent;


    protected void Start()
    {
        poolParent = new GameObject("Bullet Parent").transform;
        FillPool();
    }
    protected virtual void FillPool()
    {
        pooledObjects = new List<Bullet>();
        for (int t = 0; t < Turrets.Length; t++)
        {
            for (int i = 0; i < amountToPoolPerTurret; i++)
            {
                Bullet obj = new Bullet(t, objectToPool, poolParent);
                pooledObjects.Add(obj);
            }
        }
    }

    protected virtual void Update()
    {
        bulletsTimer += Time.deltaTime;
        if (burstCounter >= amountPerBurst)
        {
            if (burstsTimer >= coolDownBetweenBursts)
            {
                burstsTimer = 0;
                burstCounter = 0;
            }
            burstsTimer += Time.deltaTime;
        }
        UpdateBullets();
    }

    protected virtual void UpdateBullets()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].Update();
        }
    }

    protected void ShootAllTurrets()
    {
        if (burstCounter < amountPerBurst)
        {
            if (bulletsTimer >= coolDownBetweenBullets)
            {
                bulletsTimer = 0;
                for (int i = 0; i < Turrets.Length; i++)
                {
                    Shoot(Turrets[i], i);
                }
                burstCounter++;
            }
        }
    }

    public void Shoot(Transform tr, int id)
    {
        Bullet bullet = GetPooledObject(id);
        if (bullet != null)
        {
            bullet.Spawn(tr, bulletSpeed);
        }
    }

    Bullet GetPooledObject(int id)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].Active && pooledObjects[i].ParentId == id)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    private void OnDisable()
    {
        if (pooledObjects != null)
            for (int i = pooledObjects.Count - 1; i >= 0; i--)
            {
                if (pooledObjects[i].Active)
                {
                    pooledObjects[i].Kill();
                }
            }
    }

}


public class Bullet
{
    public float BulletSpeed;
    public int ParentId;
    public bool Active
    {
        set { obj.SetActive(value); }
        get
        {
            if (!obj) return true;
            return obj.activeInHierarchy;
        }
    }

    protected GameObject obj;
    protected SpriteRenderer spr;

    public Bullet(int id, GameObject prefab, Transform parent = null)
    {
        ParentId = id;

        obj = Object.Instantiate(prefab, parent);
        Active = false;
        spr = obj.GetComponent<SpriteRenderer>();
    }

    public virtual void Spawn(Transform origin, float speed)
    {
        Vector3 pos = origin.position;
        pos.z = origin.parent.position.z;
        obj.transform.position = pos;
        obj.transform.rotation = origin.rotation;

        BulletSpeed = speed;

        Active = true;
    }

    public void Update()
    {
        if (Active)
        {
            CheckDead();
            Move();
        }
    }

    protected virtual void CheckDead()
    {
        if (!spr.IsVisibleFrom(Camera.main))
        {
            Active = false;
        }
    }

    protected virtual void Move()
    {
        obj.transform.position += obj.transform.right * BulletSpeed * Time.deltaTime;
    }

    public virtual void Kill()
    {
        if (!obj) return;
        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3;
        rb.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
        GameObject.Destroy(obj, 1.5f);
    }
}