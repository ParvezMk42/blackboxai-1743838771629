using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderConfirmationManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text orderIdText;
    public TMP_Text orderDateText;
    public TMP_Text customerNameText;
    public TMP_Text deliveryAddressText;
    public TMP_Text paymentMethodText;
    public TMP_Text totalAmountText;
    public Transform orderItemsContainer;
    public GameObject orderItemPrefab;
    public Button continueShoppingButton;

    void Start()
    {
        continueShoppingButton.onClick.AddListener(() => 
            SceneLoader.LoadScene("ProductList"));
        
        LoadOrderDetails();
    }

    void LoadOrderDetails()
    {
        string orderJson = PlayerPrefs.GetString("CurrentOrder");
        if (!string.IsNullOrEmpty(orderJson))
        {
            OrderData order = JsonUtility.FromJson<OrderData>(orderJson);
            DisplayOrderDetails(order);
        }
    }

    void DisplayOrderDetails(OrderData order)
    {
        orderIdText.text = $"Order #: {order.GetHashCode():X8}";
        orderDateText.text = $"Date: {order.orderDate}";
        customerNameText.text = $"Customer: {order.customerName}";
        deliveryAddressText.text = $"Address: {order.customerAddress}";
        paymentMethodText.text = $"Payment: {order.paymentMethod}";
        totalAmountText.text = $"Total: ₹{order.totalAmount:0.00}";

        // Display order items
        foreach (Transform child in orderItemsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in order.items)
        {
            GameObject itemObj = Instantiate(orderItemPrefab, orderItemsContainer);
            OrderItemUI itemUI = itemObj.GetComponent<OrderItemUI>();
            itemUI.Initialize(item);
        }
    }
}