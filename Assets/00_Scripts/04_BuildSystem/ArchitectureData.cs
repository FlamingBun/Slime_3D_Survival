using JetBrains.Annotations;
using System;
using UnityEngine;

public enum ArchitectureType 
{
    House,
    SanctuarySpace,
}

public enum HouseType
{
    Health
}

public enum SanctuarySpaceType
{
    Speed,
    Health,
    Hunger,
    Thirst,
    Stamina
}

public enum ItemIndex
{
    House,
    Tent,
    ClockTower,
    Market,
    Storage
}

[Serializable]

public class House
{
    public HouseType type;
    public float value;
}

[Serializable]

public class SanctuarySpace
{
    public SanctuarySpaceType type;
    public float value;
}

[Serializable]

public class BuildCostManager
{
    public float buildWoodCost; // 나무 비용
    public float buildStoneCost; // 돌 비용
    public float buildMetalCost; // 금속 비용
}

[CreateAssetMenu(fileName = "Architecture", menuName = "ScriptableObject/Architecture/New Atchitecture")]

public class ArchitectureData : ScriptableObject
{
    [Header("Info")]
    public ArchitectureType architectureType; // 건축물의 타입 (안전지대, 성소 등)
    public ItemIndex index; // 건축물의 인덱스 (House, Tent 등)
    public string architectureName; // 건축물의 이름
    public string archiectureDescription; // 건축물의 설명
    public Sprite image; // 건축물의 이미지

    [Header("House")]
    public House[] Houses; //House 타입의 배열로, 각 공간의 타입과 값을 저장합니다.

    [Header("SanctuarySpace")]
    public SanctuarySpace[] sanctuarySpaces; //SanctuarySpace 타입의 배열로, 각 공간의 타입과 값을 저장합니다.

    [Header("Build")]
    public BuildCostManager buildCosts; // 건축물의 비용을 관리하는 클래스
    public GameObject architecturePrefab; // 건축물 프리팹
}