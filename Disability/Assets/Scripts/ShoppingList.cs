using UnityEngine;
using TMPro;

public class ShoppingList : MonoBehaviour
{
    public RectTransform listImage;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    // Positions et tailles pour l'image
    public Vector2 imageSmallPosition = new Vector2(-150, 150);
    public Vector2 imageLargePosition = Vector2.zero;
    public Vector2 imageSmallSize = new Vector2(300, 200);
    public Vector2 imageLargeSize = new Vector2(800, 600);

    // Positions pour les textes
    public Vector2 text1SmallPosition = new Vector2(-100, 100);
    public Vector2 text1LargePosition = new Vector2(-200, 200);
    public Vector2 text2SmallPosition = new Vector2(-100, 50);
    public Vector2 text2LargePosition = new Vector2(-200, 100);

    // Tailles de police
    public float text1SmallFontSize = 15f;
    public float text1LargeFontSize = 30f;
    public float text2SmallFontSize = 12f;
    public float text2LargeFontSize = 25f;

    public float transitionSpeed = 10f;
    private bool isZoomed = false;

    void Update()
    {
        // Vérifie le clic droit
        if (Input.GetMouseButtonDown(1))
        {
            isZoomed = !isZoomed;
        }

        // Transition pour l'image
        Vector2 imageTargetPosition = isZoomed ? imageLargePosition : imageSmallPosition;
        Vector2 imageTargetSize = isZoomed ? imageLargeSize : imageSmallSize;
        listImage.anchoredPosition = Vector2.Lerp(listImage.anchoredPosition, imageTargetPosition, Time.deltaTime * transitionSpeed);
        listImage.sizeDelta = Vector2.Lerp(listImage.sizeDelta, imageTargetSize, Time.deltaTime * transitionSpeed);

        // Transition pour le texte 1
        Vector2 text1TargetPosition = isZoomed ? text1LargePosition : text1SmallPosition;
        text1.rectTransform.anchoredPosition = Vector2.Lerp(text1.rectTransform.anchoredPosition, text1TargetPosition, Time.deltaTime * transitionSpeed);
        float targetFontSize1 = isZoomed ? text1LargeFontSize : text1SmallFontSize;
        text1.fontSize = Mathf.Lerp(text1.fontSize, targetFontSize1, Time.deltaTime * transitionSpeed);

        // Transition pour le texte 2
        Vector2 text2TargetPosition = isZoomed ? text2LargePosition : text2SmallPosition;
        text2.rectTransform.anchoredPosition = Vector2.Lerp(text2.rectTransform.anchoredPosition, text2TargetPosition, Time.deltaTime * transitionSpeed);
        float targetFontSize2 = isZoomed ? text2LargeFontSize : text2SmallFontSize;
        text2.fontSize = Mathf.Lerp(text2.fontSize, targetFontSize2, Time.deltaTime * transitionSpeed);
    }
}
