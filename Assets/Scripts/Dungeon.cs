using UnityEngine;


public class Dungeon : MonoBehaviour
{
    public string dungeonName;
    public string dungeonID;
    public enum size { small, medium , big }
    public size dungeonSize = size.small;
    public Monster[] monsters;
    
}

