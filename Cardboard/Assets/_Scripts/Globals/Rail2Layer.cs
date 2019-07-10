using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail2Layer : MonoBehaviour
{
    [SerializeField] bool UpdateRail = false;
    float FrontRailZ = 0;
    float BackRailZ = 5;
    int FrontRailLayer = 8;
    int BackRailLayer = 9;
    int MediumRailLayer = 10;

    SpriteRenderer spr;

    float prevZ;
    void OnEnable()
    {
        spr = GetComponent<SpriteRenderer>();
        prevZ = transform.position.z;
        PlaceInRail();
    }

    void Update()
    {
        if (UpdateRail)
        {
            if (prevZ != transform.position.z) PlaceInRail();
            prevZ = transform.position.z;
        }

        if (spr)
        {
            float dist = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
            Color col = spr.color;
            if (dist < 10)
                col.a = Geometry.Remap(dist, 5, 10, .2f, 1f);
            else if (dist > 10)
                col.a = Geometry.Remap(dist, 10, 15, 1f, .2f);
            else
                col.a = 1;
            spr.color = col;
        }

        if (!UpdateRail && !spr) enabled = false;
    }

    void PlaceInRail()
    {
        float z = transform.position.z;
        if (z == FrontRailZ) gameObject.layer = FrontRailLayer;
        else if (z == BackRailZ) gameObject.layer = BackRailLayer;
        else gameObject.layer = MediumRailLayer;
    }
}
