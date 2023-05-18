using _Game.Scripts.Control;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;
using UnityEngine;

public class Level : MonoBehaviour
{
    
    public int TotalTreasureCount;
    [SerializeField] private PlayerUpgrades playerUpgrades;
    

    public void Initialize(PlayerUpgradesUI playerUpgradesUI)
    {
        print(playerUpgrades);
        playerUpgrades.Initialize(playerUpgradesUI);
    }

    public void DestroyLevel()
    {
        Destroy(this.gameObject);
    }
}
