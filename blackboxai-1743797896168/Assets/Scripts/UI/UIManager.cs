using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Login Screen References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button googleLoginButton;
    public Button registerButton;
    public Button forgotPasswordButton;
    public TMP_Text statusText;
    public GameObject loadingIndicator;

    private FirebaseAuthManager authManager;

    void Start()
    {
        authManager = FirebaseAuthManager.Instance;
        SetupLoginScreen();
    }

    void SetupLoginScreen()
    {
        // Initialize UI elements
        loginButton.onClick.AddListener(async () => await HandleEmailLogin());
        googleLoginButton.onClick.AddListener(async () => await HandleGoogleLogin());
        registerButton.onClick.AddListener(async () => await HandleRegistration());
        forgotPasswordButton.onClick.AddListener(async () => await HandlePasswordReset());

        // Set up input validation
        emailInput.onValueChanged.AddListener(ValidateLoginForm);
        passwordInput.onValueChanged.AddListener(ValidateLoginForm);
        
        // Initial validation
        ValidateLoginForm(string.Empty);
    }

    void ValidateLoginForm(string _)
    {
        bool isValid = !string.IsNullOrEmpty(emailInput.text) && 
                      !string.IsNullOrEmpty(passwordInput.text) &&
                      passwordInput.text.Length >= 6;
        
        loginButton.interactable = isValid;
    }

    async Task HandleEmailLogin()
    {
        try
        {
            SetLoadingState(true);
            string email = emailInput.text;
            string password = passwordInput.text;

            bool success = await authManager.SignInWithEmailPassword(email, password);
            if (success)
            {
                statusText.text = "Login successful!";
                statusText.color = Color.green;
                // Load next scene (ProductList)
            }
            else
            {
                statusText.text = "Login failed. Please check your credentials.";
                statusText.color = Color.red;
            }
        }
        catch (Exception ex)
        {
            statusText.text = $"Error: {ex.Message}";
            statusText.color = Color.red;
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    async Task HandleGoogleLogin()
    {
        try
        {
            SetLoadingState(true);
            bool success = await authManager.SignInWithGoogle();
            if (success)
            {
                statusText.text = "Google login successful!";
                statusText.color = Color.green;
                // Load next scene (ProductList)
            }
            else
            {
                statusText.text = "Google login failed.";
                statusText.color = Color.red;
            }
        }
        catch (Exception ex)
        {
            statusText.text = $"Error: {ex.Message}";
            statusText.color = Color.red;
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    async Task HandleRegistration()
    {
        try
        {
            SetLoadingState(true);
            string email = emailInput.text;
            string password = passwordInput.text;

            bool success = await authManager.RegisterNewUser(email, password);
            if (success)
            {
                statusText.text = "Registration successful! Please login.";
                statusText.color = Color.green;
            }
            else
            {
                statusText.text = "Registration failed. Please try again.";
                statusText.color = Color.red;
            }
        }
        catch (Exception ex)
        {
            statusText.text = $"Error: {ex.Message}";
            statusText.color = Color.red;
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    async Task HandlePasswordReset()
    {
        try
        {
            SetLoadingState(true);
            string email = emailInput.text;

            if (string.IsNullOrEmpty(email))
            {
                statusText.text = "Please enter your email first.";
                statusText.color = Color.yellow;
                return;
            }

            await authManager.SendPasswordResetEmail(email);
            statusText.text = "Password reset email sent. Check your inbox.";
            statusText.color = Color.green;
        }
        catch (Exception ex)
        {
            statusText.text = $"Error: {ex.Message}";
            statusText.color = Color.red;
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    void SetLoadingState(bool isLoading)
    {
        loadingIndicator.SetActive(isLoading);
        loginButton.interactable = !isLoading;
        googleLoginButton.interactable = !isLoading;
        registerButton.interactable = !isLoading;
        forgotPasswordButton.interactable = !isLoading;
    }

    public void ShowError(string message)
    {
        statusText.text = message;
        statusText.color = Color.red;
    }

    public void ShowSuccess(string message)
    {
        statusText.text = message;
        statusText.color = Color.green;
    }
}
