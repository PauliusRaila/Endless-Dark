using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Monster", menuName = "Endless Dark/Monster")]
public class Monster : ScriptableObject
{
    public string monsterName;
    public Image monsterIcon;
    public GameObject prefab;

}

