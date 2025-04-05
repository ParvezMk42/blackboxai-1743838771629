using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(string sceneName)
    {
        if (LoadingScreenManager.Instance != null)
        {
            LoadingScreenManager.Instance.LoadScene(sceneName);
        }
        else
        {
            // Fallback if loading screen isn't available
            SceneManager.LoadScene(sceneName);
        }
        
        // Track screen view in analytics
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.TrackScreenView(sceneName);
        }
    }

    public static void LoadSceneWithLoadingScreen(string sceneName)
    {
        PlayerPrefs.SetString("TargetScene", sceneName);
        LoadScene("Loading");
    }

    public static void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}