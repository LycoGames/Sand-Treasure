using _Game.Scripts.Control;
using _Game.Scripts.Enums;
using _Game.Scripts.MeshTools;
using _Game.Scripts.UI;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int TotalTreasureCount;
    [SerializeField] private PlayerUpgrades playerUpgrades;
    [SerializeField] private DigZone myDigZone;
    public DigZone MyDigZone => myDigZone;

    public void Initialize(PlayerUpgradesUI playerUpgradesUI)
    {
        playerUpgrades.Initialize(playerUpgradesUI);
    }

    public void DestroyLevel()
    {
        Destroy(this.gameObject);
    }
}