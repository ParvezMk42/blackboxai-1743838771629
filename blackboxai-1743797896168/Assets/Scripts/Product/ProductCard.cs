using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ProductCard : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text productNameText;
    public TMP_Text priceText;
    public Image productImage;
    public Button addToCartButton;
    public Button viewInARButton;
    public GameObject loadingIndicator;

    private ProductData productData;
    private System.Action addToCartAction;
    private System.Action viewInARAction;

    public void Initialize(ProductData data, System.Action onAddToCart, System.Action onViewInAR)
    {
        productData = data;
        addToCartAction = onAddToCart;
        viewInARAction = onViewInAR;

        productNameText.text = data.Name;
        priceText.text = $"${data.Price:0.00}";
        
        addToCartButton.onClick.AddListener(() => addToCartAction());
        viewInARButton.onClick.AddListener(() => viewInARAction());

        StartCoroutine(LoadProductImage(data.ImageUrl));
    }

    IEnumerator LoadProductImage(string imageUrl)
    {
        SetLoading(true);
        
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                productImage.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
            else
            {
                Debug.LogError($"Failed to load image: {request.error}");
            }
        }

        SetLoading(false);
    }

    void SetLoading(bool isLoading)
    {
        loadingIndicator.SetActive(isLoading);
        productImage.gameObject.SetActive(!isLoading);
    }
}