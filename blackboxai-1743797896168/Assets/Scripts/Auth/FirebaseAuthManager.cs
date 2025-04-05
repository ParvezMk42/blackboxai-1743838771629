using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public static FirebaseAuthManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase initialized successfully");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public async Task<bool> SignInWithEmailPassword(string email, string password)
    {
        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            user = result.User;
            Debug.Log($"User signed in successfully: {user.Email}");
            return true;
        }
        catch (FirebaseException ex)
        {
            Debug.LogError($"Sign-in failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SignInWithGoogle()
    {
        try
        {
            // Google Sign-In implementation would go here
            // Requires additional Google Sign-In SDK setup
            Debug.Log("Google Sign-In would be implemented here");
            return false; // Placeholder
        }
        catch (Exception ex)
        {
            Debug.LogError($"Google Sign-In failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterNewUser(string email, string password)
    {
        try
        {
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            user = result.User;
            Debug.Log($"User registered successfully: {user.Email}");
            return true;
        }
        catch (FirebaseException ex)
        {
            Debug.LogError($"Registration failed: {ex.Message}");
            return false;
        }
    }

    public async Task SendPasswordResetEmail(string email)
    {
        try
        {
            await auth.SendPasswordResetEmailAsync(email);
            Debug.Log("Password reset email sent");
        }
        catch (FirebaseException ex)
        {
            Debug.LogError($"Password reset failed: {ex.Message}");
        }
    }

    public void SignOut()
    {
        auth.SignOut();
        user = null;
        Debug.Log("User signed out");
    }

    public bool IsUserLoggedIn()
    {
        return user != null;
    }

    public string GetUserEmail()
    {
        return user?.Email;
    }
}