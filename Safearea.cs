using UnityEngine;

/// <summary>
/// Attach this to any panel that should respect the device safe area
/// (iPhone notch, Android punch-hole, rounded corners, etc.)
/// Place it on your root UI panel RectTransform.
/// </summary>
public class SafeArea : MonoBehaviour
{
    RectTransform panel;
    Rect lastSafeArea;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        // Re-apply if orientation changes
        if (lastSafeArea != Screen.safeArea)
            ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        lastSafeArea  = safeArea;

        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        Vector2 anchorMin = safeArea.position / screenSize;
        Vector2 anchorMax = (safeArea.position + safeArea.size) / screenSize;

        anchorMin.x = Mathf.Clamp01(anchorMin.x);
        anchorMin.y = Mathf.Clamp01(anchorMin.y);
        anchorMax.x = Mathf.Clamp01(anchorMax.x);
        anchorMax.y = Mathf.Clamp01(anchorMax.y);

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

        Debug.Log($"Safe Area applied: {anchorMin} → {anchorMax}");
    }
}