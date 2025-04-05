using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartItem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text productNameText;
    public TMP_Text priceText;
    public Image productImage;
    public Button removeButton;
    public Button decreaseButton;
    public Button increaseButton;
    public TMP_Text quantityText;

    private ProductData productData;
    private System.Action removeAction;
    private int quantity = 1;

    public void Initialize(ProductData data, System.Action onRemove)
    {
        productData = data;
        removeAction = onRemove;

        productNameText.text = data.Name;
        priceText.text = $"₹{data.Price:0.00}";
        quantityText.text = quantity.ToString();

        removeButton.onClick.AddListener(() => removeAction());
        decreaseButton.onClick.AddListener(DecreaseQuantity);
        increaseButton.onClick.AddListener(IncreaseQuantity);

        // Load product image
        StartCoroutine(LoadProductImage(data.ImageUrl));
    }

    IEnumerator LoadProductImage(string imageUrl)
    {
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
        }
    }

    void DecreaseQuantity()
    {
        if (quantity > 1)
        {
            quantity--;
            quantityText.text = quantity.ToString();
            priceText.text = $"₹{productData.Price * quantity:0.00}";
        }
    }

    void IncreaseQuantity()
    {
        quantity++;
        quantityText.text = quantity.ToString();
        priceText.text = $"₹{productData.Price * quantity:0.00}";
    }

    public float GetTotalPrice()
    {
        return productData.Price * quantity;
    }
}