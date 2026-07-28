using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardsMenu : Panel
{
    [SerializeField] private string leaderboardId = "Leaderboard";
    [SerializeField] private int playersPerPage = 7;
    [SerializeField] private LeaderboardsPlayerItem playerItemPrefab = null;
    [SerializeField] private RectTransform playersContainer = null;
    [SerializeField] public TextMeshProUGUI pageText = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button prevButton = null;

    private int currentPage = 1;
    private int totalPages = 0;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ClearPlayersList();
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        base.Initialize();
    }

    public override void Open()
    {
        pageText.text = "-";
        nextButton.interactable = false;
        prevButton.interactable = false;
        base.Open();
        ClearPlayersList();
        currentPage = 1;
        totalPages = 0;

        if (GameSession.IsDataReadyToBeEnteredToLeaderboard)
        {
            // Grab what we need and reset immediately, before any await, so a
            // stray re-open of this panel can't submit the same game twice.
            int score = GameSession.FinalWave; 
            string playerName = GameSession.PlayerName;
            GameSession.Reset();

            SubmitPendingScoreAsync(score, playerName);
        }
        else
        {
            // Opened directly from the main menu - just show the board.
            LoadPlayers(1);
        }
    }

    private async void SubmitPendingScoreAsync(int score, string playerName)
    {
        try
        {
            if (!string.IsNullOrEmpty(playerName))
            {
                // MenuManager stamps a generic "Player" name on every fresh
                // anonymous sign-in before this panel opens - overwrite it
                // with the name entered for this run so the entry is
                // attributed correctly.
                await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            }

            var entry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);

            // entry.Rank is zero-based (top score = rank 0), matching the
            // zero-based Offset used below, so this lands on the exact page
            // that contains the entry we just submitted.
            int page = (entry.Rank / playersPerPage) + 1;
            LoadPlayers(page);
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
            LoadPlayers(1);
        }
    }

    private async void LoadPlayers(int page)
    {
        nextButton.interactable = false;
        prevButton.interactable = false;
        try
        {
            GetScoresOptions options = new GetScoresOptions();
            options.Offset = (page - 1) * playersPerPage;
            options.Limit = playersPerPage;
            var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, options);
            ClearPlayersList();
            for (int i = 0; i < scores.Results.Count; i++)
            {
                LeaderboardsPlayerItem item = Instantiate(playerItemPrefab, playersContainer);
                item.Initialize(scores.Results[i]);
            }
            totalPages = Mathf.Max(1, Mathf.CeilToInt((float)scores.Total / (float)scores.Limit));
            currentPage = page;
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
        pageText.text = currentPage.ToString() + "/" + totalPages.ToString();
        nextButton.interactable = currentPage < totalPages && totalPages > 1;
        prevButton.interactable = currentPage > 1 && totalPages > 1;
    }
    private void NextPage()
    {
        if (currentPage + 1 > totalPages)
        {
            LoadPlayers(1);
        }
        else
        {
            LoadPlayers(currentPage + 1);
        }
    }
    private void PrevPage()
    {
        if (currentPage - 1 <= 0)
        {
            LoadPlayers(totalPages);
        }
        else
        {
            LoadPlayers(currentPage - 1);
        }
    }
    private void ClosePanel()
    {
        Close();
    }

    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

    private void ClearPlayersList()
    {
        LeaderboardsPlayerItem[] items = playersContainer.GetComponentsInChildren<LeaderboardsPlayerItem>();
        if (items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Destroy(items[i].gameObject);
            }
        }
    }
}