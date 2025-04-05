using System.Collections.Generic;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductListManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject productCardPrefab;
    public Transform productListContainer;
    public TMP_Text categoryTitle;
    public Button backButton;
    public Button cartButton;
    public TMP_Text cartCountText;
    public GameObject loadingIndicator;

    [Header("AR References")] 
    public Button arViewButton;
    public string arSceneName = "ARPreview";

    private FirebaseFirestore db;
    private List<ProductData> products = new List<ProductData>();
    private List<ProductData> cartItems = new List<ProductData>();

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        backButton.onClick.AddListener(() => SceneLoader.LoadScene("Login"));
        cartButton.onClick.AddListener(() => SceneLoader.LoadScene("Cart"));
        arViewButton.onClick.AddListener(() => SceneLoader.LoadScene(arSceneName));
        
        LoadProducts();
    }

    async void LoadProducts()
    {
        try
        {
            SetLoading(true);
            
            QuerySnapshot querySnapshot = await db.Collection("products").GetSnapshotAsync();
            products.Clear();
            
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Dictionary<string, object> productDict = documentSnapshot.ToDictionary();
                ProductData product = new ProductData
                {
                    Id = documentSnapshot.Id,
                    Name = productDict["name"].ToString(),
                    Price = float.Parse(productDict["price"].ToString()),
                    Description = productDict["description"].ToString(),
                    ImageUrl = productDict["imageUrl"].ToString(),
                    ModelPath = productDict["modelPath"].ToString(),
                    Category = productDict["category"].ToString()
                };
                products.Add(product);
            }

            PopulateProductList();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading products: {ex.Message}");
        }
        finally
        {
            SetLoading(false);
        }
    }

    void PopulateProductList()
    {
        // Clear existing items
        foreach (Transform child in productListContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new product cards
        foreach (ProductData product in products)
        {
            GameObject card = Instantiate(productCardPrefab, productListContainer);
            ProductCard cardScript = card.GetComponent<ProductCard>();
            
            cardScript.Initialize(
                product,
                () => AddToCart(product),
                () => ViewInAR(product)
            );
        }
    }

    void AddToCart(ProductData product)
    {
        cartItems.Add(product);
        UpdateCartCount();
        Debug.Log($"Added {product.Name} to cart");
    }

    void ViewInAR(ProductData product)
    {
        // Store selected product for AR scene
        PlayerPrefs.SetString("SelectedProduct", JsonUtility.ToJson(product));
        SceneLoader.LoadScene(arSceneName);
    }

    void UpdateCartCount()
    {
        cartCountText.text = cartItems.Count.ToString();
        cartCountText.gameObject.SetActive(cartItems.Count > 0);
    }

    void SetLoading(bool isLoading)
    {
        loadingIndicator.SetActive(isLoading);
        productListContainer.gameObject.SetActive(!isLoading);
    }
}

[System.Serializable]
public class ProductData
{
    public string Id;
    public string Name;
    public float Price;
    public string Description;
    public string ImageUrl;
    public string ModelPath;
    public string Category;
}