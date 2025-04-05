using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class OrderData
{
    public string customerName;
    public string customerAddress;
    public string customerPhone;
    public string customerEmail;
    public string paymentMethod;
    public List<ProductData> items;
    public float totalAmount;
    public string status = "Pending";
    public string orderDate = System.DateTime.Now.ToString();
}

public class CheckoutManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_InputField addressInput;
    public TMP_InputField phoneInput;
    public TMP_InputField emailInput;
    public TMP_Dropdown paymentMethodDropdown;
    public Button placeOrderButton;
    public GameObject loadingIndicator;
    public TMP_Text statusText;

    private List<ProductData> cartItems;
    private float totalAmount;

    void Start()
    {
        InitializeCheckout();
    }

    void InitializeCheckout()
    {
        LoadCartItems();
        SetupPaymentMethods();
        SetupFormValidation();
    }