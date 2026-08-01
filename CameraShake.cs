using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    Vector3 originalPos;

    void Awake()
    {
        instance = this;

        originalPos =
            transform.localPosition;
    }

    public void Shake(float strength)
    {
        StopAllCoroutines();

        StartCoroutine(
            ShakeRoutine(strength)
        );
    }

    IEnumerator ShakeRoutine(float strength)
    {
        float time = 0f;

        while (time < 0.15f)
        {
            transform.localPosition =
                originalPos +
                (Vector3)Random.insideUnitCircle
                * (strength * 0.01f);

            time += Time.deltaTime;

            yield return null;
        }

        transform.localPosition =
            originalPos;
    }
}