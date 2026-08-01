using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Tile : MonoBehaviour
{
    public int value;

    public TMP_Text numberText;

    public Image tileImage;

    RectTransform rectTransform;

    Coroutine moveRoutine;
    Coroutine scaleRoutine;
Coroutine flashRoutine;
    [SerializeField]
float moveDuration = 0.08f;

    void Awake()
    {
        rectTransform =
            GetComponent<RectTransform>();
    }

    void Start()
    {
        SpawnAnimation();
    }

    public void SetValue(int newValue)
    {
        value = newValue;

        numberText.text =
            value.ToString();

        UpdateColor();
    }

  void UpdateColor()
{
    switch (value)
    {
        case 2:
            tileImage.color = new Color32(255, 224, 178, 255);  // FFE0B2 light peach
            numberText.color = new Color32(62, 16, 0, 255);     // dark brown text
            break;

        case 4:
            tileImage.color = new Color32(255, 204, 128, 255);  // FFCC80 peach
            numberText.color = new Color32(62, 16, 0, 255);
            break;

        case 8:
            tileImage.color = new Color32(255, 167, 38, 255);   // FFA726 amber
            numberText.color = new Color32(62, 16, 0, 255);
            break;

        case 16:
            tileImage.color = new Color32(255, 140, 0, 255);    // FF8C00 orange
            numberText.color = new Color32(255, 248, 240, 255);
            break;

        case 32:
            tileImage.color = new Color32(255, 109, 0, 255);    // FF6D00 deep orange
            numberText.color = new Color32(255, 248, 240, 255);
            break;

        case 64:
            tileImage.color = new Color32(244, 67, 54, 255);    // F44336 red
            numberText.color = new Color32(255, 248, 240, 255);
            break;

        case 128:
            tileImage.color = new Color32(229, 57, 53, 255);    // E53935 deep red
            numberText.color = new Color32(255, 248, 240, 255);
            break;

        case 256:
            tileImage.color = new Color32(198, 40, 40, 255);    // C62828 dark red
            numberText.color = new Color32(255, 248, 240, 255);
            break;

        case 512:
            tileImage.color = new Color32(183, 28, 28, 255);    // B71C1C darker red
            numberText.color = new Color32(255, 248, 240, 255);
            break;

        case 1024:
            tileImage.color = new Color32(255, 23, 68, 255);    // FF1744 bright red
            numberText.color = new Color32(255, 248, 240, 255);
            break;

        case 2048:
            tileImage.color = new Color32(255, 214, 0, 255);    // FFD600 gold
            numberText.color = new Color32(62, 16, 0, 255);
            break;

        case 4096:
            tileImage.color = new Color32(255, 248, 240, 255);  // cream with red text
            numberText.color = new Color32(255, 23, 68, 255);
            break;

        case 8192:
            tileImage.color = new Color32(255, 214, 0, 255);    // gold with dark text
            numberText.color = new Color32(183, 28, 28, 255);
            break;

        default:
            tileImage.color = new Color32(255, 224, 178, 255);
            numberText.color = new Color32(62, 16, 0, 255);
            break;
    }

}

    public void MoveTo(Vector2 target)
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        moveRoutine =
            StartCoroutine(
                SmoothMove(target)
            );
    }

    IEnumerator SmoothMove(Vector2 target)
    {
        Vector2 start =
            rectTransform.anchoredPosition;

        float time = 0;

        while (time < moveDuration)
        {
            float t = time / moveDuration;

t = Mathf.SmoothStep(0f, 1f, t);

rectTransform.anchoredPosition =
    Vector2.Lerp(
        start,
        target,
        t
    );
            time += Time.deltaTime;

            yield return null;
        }

        rectTransform.anchoredPosition =
            target;
    }

   void SpawnAnimation()
{
    if (scaleRoutine != null)
        StopCoroutine(scaleRoutine);

    scaleRoutine = StartCoroutine(SpawnRoutine());
}
    IEnumerator SpawnRoutine()
    {
        transform.localScale =
            Vector3.zero;

        float time = 0;

        while (time < 0.12f)
        {
            transform.localScale =
                Vector3.Lerp(
                    Vector3.zero,
                    Vector3.one,
                    time / 0.12f
                );

            time += Time.deltaTime;

            yield return null;
        }

        transform.localScale =
            Vector3.one;
    }

   public void PunchAnimation()
{
    if (scaleRoutine != null)
        StopCoroutine(scaleRoutine);

    scaleRoutine = StartCoroutine(PunchRoutine());
}

    IEnumerator PunchRoutine()
    {
        float time = 0;

        Vector3 start =
            Vector3.one;

       float scale = 1.12f;

if (value >= 128)
    scale = 1.16f;

if (value >= 512)
    scale = 1.20f;

if (value >= 2048)
    scale = 1.25f;

Vector3 target =
    Vector3.one * scale;
        while (time < 0.08f)
        {
            transform.localScale =
                Vector3.Lerp(
                    start,
                    target,
                    time / 0.08f
                );

            time += Time.deltaTime;

            yield return null;
        }

        time = 0;

        while (time < 0.08f)
        {
            transform.localScale =
                Vector3.Lerp(
                    target,
                    Vector3.one,
                    time / 0.08f
                );

            time += Time.deltaTime;

            yield return null;
        }

        transform.localScale =
            Vector3.one;
    }
  public void Flash()
{
    if (flashRoutine != null)
        StopCoroutine(flashRoutine);

    flashRoutine = StartCoroutine(FlashRoutine());
}

IEnumerator FlashRoutine()
{
    Color original = tileImage.color;

    tileImage.color = Color.white;

    yield return new WaitForSeconds(0.05f);

    tileImage.color = original;
}
   
}