using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherLine : MonoBehaviour
{
    
    [SerializeField] private TetherLinePoint[] tetherLinePoints;

    [SerializeField] private Rigidbody connection;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        SetConnection(connection);

        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        lineRenderer.SetPosition(0, tetherLinePoints[0].transform.position);

        for (int i = 0; i < tetherLinePoints.Length; i++)
        {
            if (tetherLinePoints[i].springJoint.connectedBody != null)
            {
                lineRenderer.SetPosition(i + 1, tetherLinePoints[i].springJoint.connectedBody.transform.position + tetherLinePoints[i].springJoint.connectedAnchor);

                Vector3 startPoint = lineRenderer.GetPosition(i);
                Vector3 endPoint = lineRenderer.GetPosition(i + 1);
                Vector3 midPoint = (endPoint - startPoint) * 0.5f + startPoint;
                tetherLinePoints[i].capsuleCollider.height = Vector3.Distance(startPoint, endPoint);
                tetherLinePoints[i].capsuleCollider.center = tetherLinePoints[i].transform.InverseTransformPoint(midPoint);
                tetherLinePoints[i].transform.LookAt(endPoint);
            }
        }
    }

    public void SetConnection(Rigidbody connection, Vector3 offset = new Vector3())
    {
        if (connection == null)
        {
            return;
        }

        this.connection = connection;
        tetherLinePoints[tetherLinePoints.Length - 1].springJoint.connectedBody = connection;
        tetherLinePoints[tetherLinePoints.Length - 1].springJoint.connectedAnchor = offset;
    }

    public Rigidbody GetRootPoint()
    {
        return tetherLinePoints[0].rb;
    }
}
