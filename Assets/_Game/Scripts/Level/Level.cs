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
    
    public DigZone MyDigZone => myDigZone;
    public FossilManager FossilManager => fossilManager;
    public void Initialize(PlayerUpgradesUI playerUpgradesUI,CinemachineVirtualCamera virtualCamera)
    {
        playerUpgrades.Initialize(playerUpgradesUI);
        fossilManager.Initialize(virtualCamera);
    }

    public void DestroyLevel()
    {
        Destroy(this.gameObject);
    }
}