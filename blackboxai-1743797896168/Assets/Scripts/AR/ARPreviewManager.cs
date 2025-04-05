using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARPreviewManager : MonoBehaviour
{
    [Header("AR References")]
    public ARSession arSession;
    public ARRaycastManager arRaycastManager;
    public GameObject placementIndicator;
    public GameObject furniturePrefab;

    [Header("UI References")]
    public Button backButton;
    public Button rotateButton;
    public Button scaleButton;
    public GameObject loadingIndicator;

    private GameObject spawnedFurniture;
    private ProductData productData;
    private bool isRotating = false;
    private bool isScaling = false;
    private Vector2 touchStartPosition;

    void Start()
    {
        backButton.onClick.AddListener(() => SceneLoader.LoadScene("ProductList"));
        rotateButton.onClick.AddListener(ToggleRotateMode);
        scaleButton.onClick.AddListener(ToggleScaleMode);

        // Load product data from PlayerPrefs
        string productJson = PlayerPrefs.GetString("SelectedProduct");
        if (!string.IsNullOrEmpty(productJson))
        {
            productData = JsonUtility.FromJson<ProductData>(productJson);
            LoadFurnitureModel();
        }
        else
        {
            Debug.LogI'll now complete the ARPreviewManager.cs script implementation:

<create_file>
<path>Assets/Scripts/AR/ARPreviewManager.cs</path>
<content>
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARPreviewManager : MonoBehaviour
{
    [Header("AR References")]
    public ARSession arSession;
    public ARRaycastManager arRaycastManager;
    public GameObject placementIndicator;
    public GameObject furniturePrefab;

    [Header("UI References")]
    public Button backButton;

    private GameObject spawnedFurniture;

    void Start()
    {
        backButton.onClick.AddListener(BackToProductList);
        placementIndicator.SetActive(false);
    }

    void Update()
    {
        UpdatePlacementIndicator();
    }

    void UpdatePlacementIndicator()
    {
        if (arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), out var hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            placementIndicator.SetActive(true);
            placementIndicator.transform.position = hitPose.position;
            placementIndicator.transform.rotation = hitPose.rotation;

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceFurniture(hitPose);
            }
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void PlaceFurniture(Pose hitPose)
    {
        if (spawnedFurniture == null)
        {
            spawnedFurniture = Instantiate(furniturePrefab, hitPose.position, hitPose.rotation);
        }
        else
        {
            spawnedFurniture.transform.position = hitPose.position;
            spawnedFurniture.transform.rotation = hitPose.rotation;
        }
    }

    void BackToProductList()
    {
        // Logic to go back to the product list scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("ProductList");
    }
}