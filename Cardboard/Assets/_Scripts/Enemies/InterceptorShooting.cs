using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum InterceptorShootingStates { SETUP, AIM, READY, LAUNCH }

public class InterceptorShooting : Shooting
{
    [Header("Interceptor Specifics")]
    [SerializeField] InterceptorShootingStates shootingState;
    [SerializeField] Vector3 setupScale = new Vector3(5, 5, 5);
    [SerializeField] float readyCoolDown = 2;
    Vector3[] setupPositions;

    protected override void FillPool()
    {
        pooledObjects = new List<Bullet>();
        for (int t = 0; t < Turrets.Length; t++)
        {
            for (int i = 0; i < amountToPoolPerTurret; i++)
            {
                Misile obj = new Misile(t, objectToPool, poolParent);
                pooledObjects.Add(obj);
            }
        }
    }

    protected override void Update()
    {
        if (GameController.GC.state == GAME_STATE.JUGANDO)
        {
            base.Update();
            if (shootingState == InterceptorShootingStates.AIM)
                ShootAllTurrets();
        }
    }

    protected override void UpdateBullets()
    {
        switch (shootingState)
        {
            case InterceptorShootingStates.SETUP:
                Setup();
                break;
            case InterceptorShootingStates.AIM:
                Aim();
                break;
            case InterceptorShootingStates.READY:
                Ready();
                break;
            case InterceptorShootingStates.LAUNCH:
                Launch();
                break;
        }
    }

    void Setup()
    {
        setupPositions = new Vector3[pooledObjects.Count];
        for (int i = 0; i < setupPositions.Length; i++)
        {
            setupPositions[i] = Vector3.Scale(Random.insideUnitCircle, setupScale);
        }
        shootingState = InterceptorShootingStates.AIM;
    }

    void Aim()
    {
        int arriveCount = 0;
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            Misile m = (Misile)pooledObjects[i];
            m.GetInPosition(setupPositions[i] + transform.position);
            if (m.InPosition)
            {
                arriveCount++;
                m.Aim(GameController.GC.Player.position);
            }
        }
        if (arriveCount >= pooledObjects.Count)
        {
            StartCoroutine(ReadyTimer());
        }
    }
    IEnumerator ReadyTimer()
    {
        shootingState = InterceptorShootingStates.READY;
        yield return new WaitForSeconds(readyCoolDown);
        shootingState = InterceptorShootingStates.LAUNCH;
    }

    void Ready()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            Misile m = (Misile)pooledObjects[i];
            m.GetInPosition(setupPositions[i] + transform.position);
            m.Aim(GameController.GC.Player.position);
        }
    }

    void Launch()
    {
        int deadCounter = 0;
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].Update();
            if (!pooledObjects[i].Active) deadCounter++;
        }
        if (deadCounter >= pooledObjects.Count)
        {
            shootingState = InterceptorShootingStates.SETUP;
        }
    }
}