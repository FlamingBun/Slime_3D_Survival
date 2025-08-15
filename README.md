# 14조 - 잠에서 깨어나니, 여긴 슬라임만이 지배하는 세계였다.

# 🎮 게임 시연 영상
[![Video Label](http://img.youtube.com/vi/ufxCjNZIOT4/0.jpg)](https://www.youtube.com/watch?v=ufxCjNZIOT4)

# 🎮 3D Survival

W,A,S,D : 이동<br>
스페이스 바 : 점프<br>
TAB : 인벤토리창 열기/닫기<br>
B : 건물빌드창<br>
좌클릭 : 자원채취 / 건물배치<br>
우클릭, ESC : 건물프리뷰중 취소<br>
F : NPC상호작용<br>
O : 퀘스트 목록<br>
1,2,3 : 장착 아이템 변경<br>

프로젝트 개요<br>
장르 : 3D 서바이벌<br>
주요 매커니즘 : 3D 서바이벌 게임은 플레이어가 자원을 수집하고 기지를 건설하며 생존을 위해 적과 싸우는 게임입니다. <br>
구현 내용 : 플레이어는 자원을 수집하고 기지를 건설하며, 생존 요소와 날씨 변화에 대응해야 합니다.<br>
적 AI와의 전투, 퀘스트 진행, 몰입감 있는 사운드를 통해 깊이 있는 생존 경험을 제공합니다.<br>

<br/><br/>

# 🤼‍♂️ 팀 구성 및 역할

![image](https://github.com/user-attachments/assets/112387f0-843b-4b3d-88dd-d3aeb7c84ded)


<br/><br/>

# 🛠️ 개발환경

- `Unity 2022.3.17f`
- `C#`
- `GitHub`

<br/><br/>

# 🗓 개발 일정
![개발일정](https://github.com/user-attachments/assets/cb9eb93c-7592-4c17-bded-ba28e4be30c0)


<br/><br/>

# 📂 프로젝트 구조

```
Assets/
Assets/
├── 00_Scripts/                           # 주요 게임 스크립트
│
│   ├── 00_Player/                        # 플레이어 관련
│   │   ├── CharacterManager.cs
│   │   ├── Condition.cs
│   │   ├── Player.cs
│   │   ├── PlayerCombat.cs
│   │   ├── PlayerCondition.cs
│   │   ├── PlayerController.cs
│   │   ├── PlayerEquipment.cs
│   │   ├── PlayerInputActions.cs
│   │   ├── PlayerInteractor.cs
│   │   └── UICondition.cs
│
│   ├── 01_Npc/                           # NPC 및 대화 시스템
│   │   ├── DialogueUI.cs
│   │   ├── NPC.cs
│   │   ├── NPCData.cs
│   │   └── NPCInteractor.cs
│
│   ├── 02_Quest/                         # 퀘스트 시스템
│   │   ├── Quest.cs
│   │   ├── QuestListWindow.cs
│   │   ├── QuestManager.cs
│   │   ├── QuestUI.cs
│   │   └── RewardWindow.cs
│
│   ├── 03_Inventory/                     # 인벤토리 시스템
│   │   ├── InventoryManager.cs
│   │   ├── InventoryTabType.cs
│   │   └── InventoryUI.cs
│
│   ├── 04_BuildSystem/                   # 건축 시스템
│   │   ├── ArchitectureData.cs
│   │   ├── BuildManager.cs
│   │   ├── BuildSystemUI.cs
│   │   └── RemoverManager.cs
│
│   ├── 05_Data/                          # ScriptableObject 자산
│   │   └── Equip/
│   │       ├── Axe.asset
│   │       ├── AxeData.cs
│   │       ├── Pick.asset
│   │       ├── PickData.cs
│   │       ├── Sword.asset
│   │       └── SwordData.cs
│   │   └── Metal/
│   │       └── MetalData.cs
│   │   └── Stone/
│   │       └── StoneData.cs
│   │   └── Wood/
│   │       └── WoodData.cs
│
│   ├── 06_Enemy/                         # 적 AI, 상태머신, 무기
│   │   ├── EnemyAnimationController.cs
│   │   ├── EnemyCondition.cs
│   │   ├── EnemyController.cs
│   │   ├── EnemyDatabaseSO.cs
│   │   ├── EnemyEnums.cs
│   │   ├── EnemyManager.cs
│   │   ├── EnemySO.cs
│   │   ├── EnemySpawner.cs
│   │   ├── EnemyState.cs
│   │   ├── EnemyStateMachine.cs
│   │   ├── PoolManager.cs
│   │   └── NormalEnemy/
│   │       ├── EnemyAttackState.cs
│   │       ├── EnemyDieState.cs
│   │       ├── EnemyHitState.cs
│   │       ├── EnemyIdleState.cs
│   │       ├── EnemyReturnState.cs
│   │       ├── EnemyTraceState.cs
│   │       └── EnemyWanderState.cs
│   │   └── EnemySettings/
│   │       └── EnemySettings.cs
│   │   └── EnemyWeapon/
│   │       ├── EnemyMeleeWeaponSO.cs
│   │       ├── EnemyRangedWeaponSO.cs
│   │       └── EnemyWeaponSO.cs
│
│   ├── 07_Item/                          # 아이템 시스템
│   │   ├── ScriptableObject/
│   │   │   ├── CraftingRecipe.cs
│   │   │   ├── ItemData.cs
│   │   │   ├── ItemType.cs
│   │   │   ├── ResourceData.cs
│   │   │   ├── ResourceObject.cs
│   │   │   └── ToolType.cs
│   │   ├── ItemSlot.cs
│   │   └── ItemWorld.cs
│
│   ├── 08_Environment/                   # 환경 시스템
│   │   ├── DayNightCycle.cs
│   │   └── WeatherSystem.cs
│
│   ├── 09_Architecture/                  # 설치 가능한 건축물
│   │   ├── ClockTower.cs
│   │   ├── Market.cs
│   │   ├── SafeSpace.cs
│   │   ├── Storage.cs
│   │   └── Tent.cs
│
│   ├── 10_Buff/                          # 버프 시스템
│   │   ├── Buff.cs
│   │   └── BuffUI.cs
│
│   └── 11_Crafting/                      # 제작 시스템
│       ├── CraftingManager.cs
│       └── CraftingUI.cs
├── 01_Prefabs/ # 프리팹 모음
│ ├── ArchitectureObject/ # 건축물 프리팹
│ ├── Enemy/ # 적 프리팹
│ ├── Item/ # 아이템 드롭 오브젝트
│ ├── Manager/ # 싱글톤/매니저 오브젝트
│ ├── NPC/ # NPC 프리팹
│ ├── Player/ # 플레이어 프리팹
│ └── UI/ # UI 관련 프리팹
│
├── 02_ScriptableObjects/ # ScriptableObject 데이터들
│ ├── ArchitectureObject/
│ ├── Enemy/
│ └── Quest/
│
├── 03_Animation/ # 애니메이션 파일들
├── 04_Materials/ # 머티리얼 모음
├── 06_InputAction/ # Input System Action 자산
├── 07_Scenes/ # 게임 씬들
├── 08_Image/ # UI/아이템 이미지
```

<br/><br/>

# 🎯 주요 기능

- 캐릭터 이동 및 점프 구현
- 체력/스태미너/배고픔/갈증/온도 구현
- 자원획득 / 자원리스폰 기능
- 인벤토리/제작/건물빌딩 UI/기능
- NPC상호작용 및 퀘스트 기능
- 몬스터와 전투기능

<br/><br/>

# 🧨 트러블 슈팅
![image](https://github.com/user-attachments/assets/4e31b1b3-d087-4add-b5b3-c2e9d5bc8d43)

<br/>

![image](https://github.com/user-attachments/assets/fbff6c7e-c2ed-42cd-83e1-68c38bbdb6a6)

<br/>

![image](https://github.com/user-attachments/assets/1aae094e-d3f5-48bb-aebe-dee98b955163)

<br/>

![image](https://github.com/user-attachments/assets/929c4bf1-f75d-4b3c-8dd2-33ab64bdece5)


<br/><br/>
