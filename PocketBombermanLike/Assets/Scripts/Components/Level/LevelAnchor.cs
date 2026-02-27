using UnityEngine;

public class LevelAnchor : MonoBehaviour
{
    [SerializeField] private LevelAnchorType _anchorType;
    public LevelAnchorType AnchorType => _anchorType;

    private void OnDrawGizmos()
    {
        switch (_anchorType)
        {
            case LevelAnchorType.Start:
                Gizmos.color = Color.green;
                break;

            case LevelAnchorType.End:
                Gizmos.color = Color.red;
                break;
        }

        Gizmos.DrawSphere(transform.position, 0.75f);
    }
}

