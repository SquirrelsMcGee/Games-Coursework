using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurret : UsableItem
{

    public GameObject turretPrefab;

    public override bool UseItem(int useCost) {

        // Check if player can place the object
        if (PlayerController.Instance.turretPlaceable)
        {
            if (GameController.Instance.achievedScore >= GameController.Instance.usedScore + useCost)
            {
                Instantiate(turretPrefab, PlayerController.Instance.placePosition, PlayerController.Instance.transform.rotation);
                GameController.Instance.UseScore(useCost);
            }
        }
        print("Used Score: " + GameController.Instance.usedScore);
        return false;
    }
}
