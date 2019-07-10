using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Formation))]
public class FormationPreview : MonoBehaviour
{
    [SerializeField] Formation formation;
    private void Awake()
    {
        formation = GetComponent<Formation>();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        for (int i = 0; i < formation.AmountToSpawn; i++)
        {
            float percent;
            if (formation.AmountToSpawn == 1) percent = 0.5f;
            else
                percent = i / (formation.AmountToSpawn - 1.0f);

            Gizmos.DrawWireSphere(
                transform.position +
                new Vector3(
                    formation.FormationX.Evaluate(percent),
                    formation.FormationY.Evaluate(percent),
                    formation.FormationZ.Evaluate(percent)
                ),
                0.5f
            );
        }
    }
}