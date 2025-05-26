using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
public class CharacterDat
{
    public string namex;
    public int level;
    public List<int> ownedRambo = new();
    public int curRambo = 0;
}
