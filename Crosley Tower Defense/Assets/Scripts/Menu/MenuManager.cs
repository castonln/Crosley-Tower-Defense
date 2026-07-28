using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class MenuManager : MonoBehaviour
{

    private bool initialized = false;
    private bool eventsInitialized = false;

    // Set while we're intentionally calling SignOut(true) ourselves, so the
    // SignedOut event handler below doesn't also fire an extra, redundant
    // SignInAnonymouslyAsync() call on top of the one we trigger explicitly.
    private bool suppressAutoResignIn = false;

    private static MenuManager singleton = null;

    public static MenuManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindFirstObjectByType<MenuManager>();
                singleton.Initialize();
            }
            return singleton;
        }
    }

    private void Initialize()
    {
        if (initialized) { return; }
        initialized = true;
    }

    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
        }
    }

    private void Start()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    public async void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.Open("loading");
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                var options = new InitializationOptions();
                options.SetProfile("default_profile");
                try
                {
                    // Initializes all installed Unity Gaming Services SDKs at once
                    await UnityServices.InitializeAsync();
                    Debug.Log("Unity Services successfully initialized!");

                    // Proceed to authenticate players or load IAP catalogs here
                }
                catch (Exception e)
                {
                    Debug.LogError($"Unity Services failed to initialize: {e.Message}");
                }
            }

            if (!eventsInitialized)
            {
                SetupEvents();
            }

            // Arcade-style leaderboard: this scene only ever contains the
            // leaderboard flow, and players should never resume a previous
            // identity's progress. So every time this scene runs, force a
            // brand new anonymous player before signing in, rather than
            // letting SignInAnonymouslyAsync recover a cached one.
            ClearAnonymousSession();

            // Anonymous sign-in is the only path in. SignInAnonymouslyAsync already
            // resumes an existing session if one exists, or creates a new anonymous
            // identity if it doesn't - which is why we clear the cached session
            // token above first, so there's nothing left for it to resume.
            SignInAnonymouslyAsync();
        }
        catch (Exception exception)
        {
            ShowError(ErrorMenu.Action.StartService, "Failed to connect to the network.", "Retry");
        }
    }

    // Wipes out any previously cached anonymous session so the upcoming
    // SignInAnonymouslyAsync() call is forced to mint a fresh player rather
    // than resuming whoever played last.
    private void ClearAnonymousSession()
    {
        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                // Already signed in from earlier this run (e.g. this scene got
                // reloaded without the app/editor session restarting).
                // SignOut(true) signs out AND deletes the cached session token
                // in one call.
                suppressAutoResignIn = true;
                AuthenticationService.Instance.SignOut(true);
                suppressAutoResignIn = false;
            }
            else if (AuthenticationService.Instance.SessionTokenExists)
            {
                // Not signed in yet this run, but a token from a previous
                // run/editor session is still cached on disk - delete it.
                AuthenticationService.Instance.ClearSessionToken();
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Could not clear previous anonymous session: {e.Message}");
        }
    }

    public async void SignInAnonymouslyAsync()
    {
        PanelManager.Open("loading");
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            // Called directly rather than via the SignedIn event: that event only
            // fires on an actual signed-out -> signed-in transition. If we're
            // already authenticated (e.g. the scene reloaded mid-session), this
            // call completes as a no-op and SignedIn never fires, so we can't
            // rely on it to get us to the leaderboard.
            SignInConfirmAsync();
        }
        catch (AuthenticationException exception)
        {
            ShowError(ErrorMenu.Action.SignIn, "Failed to sign in.", "Retry");
        }
        catch (RequestFailedException exception)
        {
            ShowError(ErrorMenu.Action.SignIn, "Failed to connect to the network.", "Retry");
        }
    }

    private void SetupEvents()
    {
        eventsInitialized = true;

        AuthenticationService.Instance.SignedOut += () =>
        {
            if (suppressAutoResignIn) { return; }

            // There's no auth panel to fall back to, so just try signing
            // back in anonymously.
            SignInAnonymouslyAsync();
        };

        AuthenticationService.Instance.Expired += () =>
        {
            SignInAnonymouslyAsync();
        };
    }

    private void ShowError(ErrorMenu.Action action = ErrorMenu.Action.None, string error = "", string button = "")
    {
        PanelManager.Close("loading");
        ErrorMenu panel = (ErrorMenu)PanelManager.GetSingleton("error");
        panel.Open(action, error, button);
    }

    private async void SignInConfirmAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync("Player");
            }
            PanelManager.CloseAll();
            PanelManager.Open("leaderboards");
        }
        catch
        {

        }
    }

}