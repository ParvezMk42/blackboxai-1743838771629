using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class PaymentManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text totalAmountText;
    public Button googlePayButton;
    public Button paytmButton;
    public Button phonePeButton;
    public GameObject loadingIndicator;
    public TMP_Text statusText;

    private List<ProductData> cartItems;
    private float totalAmount;
    private string paymentGatewayUrl = "https://your-payment-gateway-api.com";

    void Start()
    {
        // Load cart items from PlayerPrefs or other storage
        string cartJson = PlayerPrefs.GetString("CartItems");
        if (!string.IsNullOrEmpty(cartJson))
        {
            cartItems = JsonUtility.FromJson<List<ProductData>>(cartJson);
            CalculateTotal();
        }

        googlePayButton.onClick.AddListener(() => StartCoroutine(ProcessPayment("googlepay")));
        paytmButton.onClick.AddListener(() => StartCoroutine(ProcessPayment("paytm")));
        phonePeButton.onClick.AddListener(() => StartCoroutine(ProcessPayment("phonepe")));
    }

    void CalculateTotal()
    {
        totalAmount = 0f;
        foreach (var item in cartItems)
        {
            totalAmount += item.Price;
        }
        totalAmountText.text = $"Total: ₹{totalAmount:0.00}";
    }

    IEnumerator ProcessPayment(string gateway)
    {
        SetLoading(true);
        statusText.text = "Processing payment...";
        statusText.color = Color.yellow;

        // Create payment request
        WWWForm form = new WWWForm();
        form.AddField("amount", totalAmount.ToString("0.00"));
        form.AddField("gateway", gateway);
        form.AddField("order_id", Guid.NewGuid().ToString());

        using (UnityWebRequest request = UnityWebRequest.Post(paymentGatewayUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JSON.Parse(request.downloadHandler.text);
                if (response["status"] == "success")
                {
                    statusText.text = "Payment successful!";
                    statusText.color = Color.green;
                    ClearCart();
                    // Proceed to order confirmation
                }
                else
                {
                    statusText.text = $"Payment failed: {response["message"]}";
                    statusText.color = Color.red;
                }
            }
            else
            {
                statusText.text = $"Error: {request.error}";
                statusText.color = Color.red;
            }
        }

        SetLoading(false);
    }

    void SetLoading(bool isLoading)
    {
        loadingIndicator.SetActive(isLoading);
        googlePayButton.interactable = !isLoading;
        paytmButton.interactable = !isLoading;
        phonePeButton.interactable = !isLoading;
    }

    void ClearCart()
    {
        cartItems.Clear();
        PlayerPrefs.DeleteKey("CartItems");
    }
}