
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    void OnMouseDown()
    {
        Collider[] listeners = Physics.OverlapSphere(transform.position, 10f);
        foreach (var hit in listeners)
        {
            Perception_AgentController ai = hit.GetComponent<Perception_AgentController>();
            if (ai != null)
            {
                ai.HearNoise(transform.position);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
