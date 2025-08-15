using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public List<Quest> quests;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        foreach (Quest quest in quests)
        {
            quest.Init();
        }
    }
}
