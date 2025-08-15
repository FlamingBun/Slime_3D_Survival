using UnityEngine;

[CreateAssetMenu(fileName = "New ResourceData", menuName = "Game/Resource Data")]
public class ResourceData : ScriptableObject
{
    public string resourceName;
    public Sprite icon;
    public ToolType requiredTool;
    public ItemData dropItem;
    public int dropAmount = 1;
    public int maxDurability = 3;
}
