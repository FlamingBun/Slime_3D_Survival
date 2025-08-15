using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabaseSO", menuName = "ScriptableObject/Enemy/EnemyDatabase")]
public class EnemyDatabaseSO : ScriptableObject
{
        public List<EnemySO> enemyList;
}
