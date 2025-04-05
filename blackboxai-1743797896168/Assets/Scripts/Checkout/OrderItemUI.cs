using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class OrderItemUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text productNameText;
    public TMP_Text priceText;
    public TMP_Text quantityText;
    public Image productImage;
    public GameObject loadingIndicator;

    public void Initialize(ProductData product, int quantity = 1)
    {
        productNameText.text = product.Name;
        priceText.text = $"₹{product.Price:0.00}";
        quantityText.text = $"Qty: {quantity}";
        
        StartCoroutine(LoadProductImage(product.ImageUrl));
    }

    IEnumerator LoadProductImage(string imageUrl)
    {
        loadingIndicator.SetActive(true);
        productImage.gameObject.SetActive(false);

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
                productImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError($"Failed to load product image: {request.error}");
            }
        }

        loadingIndicator.SetActive(false);
    }
}