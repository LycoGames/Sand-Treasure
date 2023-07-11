using _Game.Scripts.Control;
using _Game.Scripts.Enums;
using _Game.Scripts.MeshTools;
using _Game.Scripts.UI;
using Cinemachine;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int TotalTreasureCount;
    [SerializeField] private PlayerUpgrades playerUpgrades;
    [SerializeField] private DigZone myDigZone;
    [SerializeField] private FossilManager fossilManager;
    [SerializeField] private SandSellArea sandSellArea;
    
    public DigZone MyDigZone => myDigZone;
    public FossilManager FossilManager => fossilManager;
    public void Initialize(PlayerUpgradesUI playerUpgradesUI)
    {
        playerUpgrades.Initialize(playerUpgradesUI);
        fossilManager.Initialize();
        sandSellArea.Initialize();
    }

    public void DestroyLevel()
    {
        Destroy(this.gameObject);
    }
}