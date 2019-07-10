using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] protected int maxHP;
    protected int currentHP;

    void Awake()
    {
        currentHP = maxHP;
    }
}
