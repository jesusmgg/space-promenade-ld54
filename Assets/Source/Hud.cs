using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _flagshipHpText;
    [SerializeField] TextMeshProUGUI _fleetSizeText;
    [SerializeField] TextMeshProUGUI _enemyBasesText;

    [SerializeField] Player _player;

    void Update()
    {
        if (_player != null)
        {
            _flagshipHpText.text = $"{_player.HitPoints}";
            _fleetSizeText.text = $"{_player.AllyCount + 1}";
            _enemyBasesText.text = $"{_player.TargetSpawnersLeft}";
        }
    }
}