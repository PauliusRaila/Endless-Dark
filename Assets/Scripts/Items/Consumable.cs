using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/* An Item that can be consumed. So far that just means regaining health */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Consumable")]
public class Consumable : Item {

	public int value;       // How much health?
	public enum Actions { heal , homeTeleport}
	public Actions itemAction;
	

	// This is called when pressed in the inventory
	public override void Use()
	{   		
        dungeonBagManager.instance.Add(this);
    }

	public override void Consume()
	{
		switch (itemAction) {
			case Actions.heal:
				gameManager.instance.localPlayer.GetComponent<PUN_ThirdPersonController>().ChangeHealth(value);
				break;
			case Actions.homeTeleport:
				Debug.Log("Teleport");
				gameManager.instance.localPlayer.GetComponent<PUN_ThirdPersonController>().enabled = false;
				gameManager.instance.localPlayer.transform.position = gameManager.instance.getSpawnPoint(true).position;			
				break;

		}

		if(consumableEffect != null)
			Instantiate(consumableEffect, gameManager.instance.localPlayer.transform.Find("particlePos"));		
	}

}
