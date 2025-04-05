using UnityEngine;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TrackEvent(string eventName, Dictionary<string, object> parameters = null)
    {
        // Implement your analytics SDK integration here
        Debug.Log($"Tracking event: {eventName}");
        
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                Debug.Log($"{param.Key}: {param.Value}");
            }
        }

        // Example integration with Firebase Analytics:
        // Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameters);
    }

    public void TrackScreenView(string screenName)
    {
        Debug.Log($"Viewing screen: {screenName}");
        // Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventScreenView,
        //     new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterScreenName, screenName));
    }

    public void TrackPurchase(OrderData order)
    {
        var parameters = new Dictionary<string, object>
        {
            { "transaction_id", order.GetHashCode().ToString("X8") },
            { "value", order.totalAmount },
            { "currency", "INR" },
            { "payment_method", order.paymentMethod },
            { "item_count", order.items.Count }
        };

        TrackEvent("purchase_complete", parameters);
    }
}