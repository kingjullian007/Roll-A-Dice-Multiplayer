using UnityEngine;
using Nakama;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Dice dice;
    private NakamaManager nakamaManager;

    private void Awake ()
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

    private IEnumerator Start ()
    {
        yield return new WaitForSeconds(2f);
        nakamaManager = NakamaManager.Instance;
        nakamaManager.GetSocket().ReceivedMatchState += OnReceivedMatchState;

        // Create or join a match
        // create a match here
        nakamaManager.CreateMatch();
        yield return new WaitForSeconds(2f); // Wait for the match to be created
    }

    public void RollDice ()
    {
        dice.RollDice();
    }

    public void OnDiceRolled (int result)
    {
        SendDiceResult(result);
    }

    private async void SendDiceResult (int result)
    {
        if (nakamaManager.GetMatchId() == null) return;

        var state = new Dictionary<string, int> { { "diceResult", result } };
        var jsonState = JsonConvert.SerializeObject(state);
        await nakamaManager.GetSocket().SendMatchStateAsync(nakamaManager.GetMatchId(), 0, jsonState);
    }

    private void OnReceivedMatchState (IMatchState matchState)
    {
        var jsonString = System.Text.Encoding.UTF8.GetString(matchState.State);
        var state = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonString);
        if (state.TryGetValue("diceResult", out var diceResult))
        {
            dice.SetSide(diceResult);
        }
    }
}
