using UnityEngine;

public class Formation : MonoBehaviour
{
    [SerializeField] GameObject Prefab;
    public int AmountToSpawn;
    public AnimationCurve FormationX = AnimationCurve.Constant(0, 1, 0);
    public AnimationCurve FormationY = AnimationCurve.Constant(0, 1, 0);
    public AnimationCurve FormationZ = AnimationCurve.Constant(0, 1, 0);

    [Tooltip("Enemies move along the curve \n" + "Make sure curves aren't Clamped")]
    public bool March = false;
    public float MarchSpeedMultiplier = 0.25f;

    Vector3[] positions;
    Transform membersParent;

    void Start()
    {
        membersParent = new GameObject(gameObject.name + " Members").transform;
        positions = new Vector3[AmountToSpawn];
        for (int i = 0; i < AmountToSpawn; i++)
        {
            float percent;
            if (AmountToSpawn == 1) percent = 0.5f;
            else
                percent = i / (AmountToSpawn - 1.0f);

            positions[i] = new Vector3(
                FormationX.Evaluate(percent),
                FormationY.Evaluate(percent),
                FormationZ.Evaluate(percent)
            );

            Instantiate(Prefab,
                transform.position +
                positions[i],
                Quaternion.identity,
                membersParent
            );
        }
    }


    void Update()
    {
        if (March)
            for (int i = 0; i < membersParent.childCount; i++)
            {
                float percent;
                if (membersParent.childCount == 1) percent = 0.5f;
                else
                    percent = i / (membersParent.childCount - 0.0f);
                percent += Time.time * MarchSpeedMultiplier;

                positions[i].Set(
                    FormationX.Evaluate(percent),
                    FormationY.Evaluate(percent),
                    FormationZ.Evaluate(percent)
                );

                membersParent.GetChild(i).position = transform.position + positions[i];
            }
    }

}
