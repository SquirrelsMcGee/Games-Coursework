using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a turret when UseItem is called, extension of UsableItem
/// </summary>
public class SpawnTurret : UsableItem
{

    [Header("Prefab Objects")]
    [Tooltip("Turret to create")]
    public GameObject turretPrefab;

    /// <summary>
    /// Spawns a turret is the player is able to
    /// </summary>
    /// <param name="useCost"></param>
    /// <returns>false</returns>
    public override bool UseItem(int useCost) {

        // Check if player can place the object
        if (PlayerController.Instance.turretPlaceable)
        {
            // Calculate modified use cost from Item base cost + number of that item created
            int _useCost = useCost + Inventory.Instance.selectedItem.GetCreated();

            // Check if the player can build a turret
            bool canBuild = (GameController.Instance.achievedScore >= GameController.Instance.usedScore + Inventory.Instance.selectedItem.useCost);
            if (canBuild)
            {
                // Create the turret
                Instantiate(turretPrefab, PlayerController.Instance.placePosition, PlayerController.Instance.transform.rotation);

                // Increase the used score
                GameController.Instance.UseScore(_useCost);

                // Increase the number of created turrets
                Inventory.Instance.selectedItem.IncrementCreated();
            }
        }
        print("Used Score: " + GameController.Instance.usedScore);
        return false;
    }
}
