using UnityEngine;
using UnityEngine.SceneManagement;

/* An Item that can be equipped to increase armor/damage. */


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item {
	
    public GameObject itemPrefab;
	public Mesh mesh;
    public Material mat;



    

    // Called when pressed in the inventory
    public override void Use ()
	{
       
         EquipmentManager.instance.Equip(this);	// Equip
         
		 RemoveFromInventory(); // Remove from inventory

        if (SceneManager.GetActiveScene().name == "main_menuv2");
            Inventory.instance.GetComponent<InventoryUI>().backToInventory();
    }






}

