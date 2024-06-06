using UnityEngine;
using System.Collections;

public class DetectEnemy : MonoBehaviour
{
    public Color hitColor = Color.red; // 적이 닿았을 때 변경할 색상
    public float colorChangeDuration = 2.0f; // 색상 변경 지속 시간

    private LineRenderer lineRenderer;
    private Color originalColor;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            originalColor = lineRenderer.startColor; // 원래 색상을 저장
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Test") && lineRenderer != null) // 적과 충돌하면
        {
            StartCoroutine(ChangeColorTemporarily());
        }
    }

    IEnumerator ChangeColorTemporarily()
    {
        // 색상을 hitColor로 변경
        lineRenderer.startColor = hitColor;
        lineRenderer.endColor = hitColor;

        // 일정 시간 대기
        yield return new WaitForSeconds(colorChangeDuration);

        // 원래 색상으로 복원
        lineRenderer.startColor = originalColor;
        lineRenderer.endColor = originalColor;
    }
}
