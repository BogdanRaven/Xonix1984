using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiContext : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TMP_Text _levelsTxt;
    [SerializeField] private TMP_Text _livesTxt;
    [SerializeField] private TMP_Text _timeTxt;
    [SerializeField] private TMP_Text _gameMessageTxt;

    public float DurationGameMessage => durationGameMessage;

    private float durationGameMessage = 1f;

    public void UpdateLivesTxt(int lives)
    {
        _livesTxt.text = "Lives:" + lives;
    }

    public void UpdateLevelTxt(int level)
    {
        level++;
        _levelsTxt.text = "Levels" + level;
    }

    public void UpdateTime(int time)
    {
        _timeTxt.text = "Time:" + time + "";
    }

    public void SetButtonPauseListener(UnityAction unityAction)
    {
        _pauseButton.onClick.RemoveAllListeners();
        _pauseButton.onClick.AddListener(unityAction);
    }

    public void ShowGameMessage(string massage, Color color)
    {
        _gameMessageTxt.gameObject.SetActive(true);
        _gameMessageTxt.text = massage;
        _gameMessageTxt.color = color;
    }

    public void DisableGameMessage()
    {
        _gameMessageTxt.gameObject.SetActive(false);
    }
}