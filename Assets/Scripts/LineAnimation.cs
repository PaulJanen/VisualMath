using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAnimation : MonoBehaviour
{
    public bool drawSegments;
    public float segmentDistance;
    public GameObject segmentPrefab;

    private float segmentCumulativeDistance;
    LineRenderer lr;
    Vector3 startPos;
    Vector3 endPos;
    public Transform arrow;
    [SerializeField] private Ease animationType;
    [SerializeField] private float animationDuration = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        segmentCumulativeDistance = segmentDistance;
        lr = GetComponent<LineRenderer>();
        startPos = lr.GetPosition(0);
        endPos = lr.GetPosition(1);
        CorrectArrowsHeading();

        lr.SetPosition(1, startPos);
    }

    void CorrectArrowsHeading()
    {
        arrow.forward = lr.GetPosition(1) - lr.GetPosition(0);
    }

    public void AnimateLine()
    {
        DOVirtual.Float(0f, 1f, animationDuration, (t) => SetLineToNewPos(t)).SetEase(animationType);
    }

    void SetLineToNewPos(float t)
    {
        lr.SetPosition(1, Vector3.Lerp(startPos, endPos, t));
        arrow.position = lr.transform.position + lr.GetPosition(0) + lr.GetPosition(1);
        CorrectArrowsHeading();

        if(drawSegments)
            DrawSegment();
    }

    void DrawSegment()
    {
        float distance = Vector3.Distance(lr.GetPosition(0), lr.GetPosition(1));
        if(distance > segmentCumulativeDistance)
        {
            segmentCumulativeDistance += segmentDistance;
            Vector3 position = lr.transform.position + lr.GetPosition(0) + lr.GetPosition(1);
            GameObject obj = Instantiate(segmentPrefab, position, segmentPrefab.transform.rotation);
            obj.SetActive(true);
        }
    }
}
