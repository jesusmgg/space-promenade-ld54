using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _flagshipHpText;
    [SerializeField] TextMeshProUGUI _fleetSizeText;
    [SerializeField] TextMeshProUGUI _enemyBasesText;

    [SerializeField] GameObject _startEnginesPanel;
    [SerializeField] GameObject _restartPanel;

    [SerializeField] Player _player;

    bool _enginesStarted;

    void Update()
    {
        if (_player != null)
        {
            _flagshipHpText.text = $"{_player.HitPoints}";
            _fleetSizeText.text = $"{_player.AllyCount + 1}";
            _enemyBasesText.text = $"{_player.TargetSpawnersLeft}";

            if (!_enginesStarted)
            {
                if (_player.HasEngineOn)
                {
                    _enginesStarted = true;
                    _startEnginesPanel.gameObject.SetActive(false);
                }
            }
        }
        else if (_startEnginesPanel.gameObject.activeSelf == false)
        {
            _restartPanel.gameObject.SetActive(true);
        }
    }
}