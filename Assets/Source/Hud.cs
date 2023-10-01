using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _fleetSizeText;
    [SerializeField] TextMeshProUGUI _enemyBasesText;

    [SerializeField] Player _player;

    void Update()
    {
        _fleetSizeText.text = $"{_player.Allies + 1}";
        _enemyBasesText.text = $"{_player.EnemySpawnerTarget}";
    }
}