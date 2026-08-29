using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Monster _monsterPrefab;
    [SerializeField] private int _maxMonstersCount = 6;

    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private MonsterController _controller;
    private List<int> _userSlot = new List<int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void SpawnMonsters()
    {
        for(int i = 0; i < _maxMonstersCount; i++)
        {
            int index = GetSlotForMonster();
            var monster = Instantiate(_monsterPrefab, _spawnPoints[index].position, Quaternion.identity, _spawnPoints[index]);
            monster.GetComponent<Monster>().SetupMonster(_controller);
            monster.SetupMonster(_controller);
        }
    }
    private int GetSlotForMonster()
    {
        int slot = 0;
        for(int i = 0; i < _spawnPoints.Count; i++)
        {
            int randomSlot = Random.Range(0, _spawnPoints.Count);
            if(!_userSlot.Contains(randomSlot))
            {
                slot = randomSlot;
                _userSlot.Add(randomSlot);
                return slot;
            }

        }
        return slot;
    }

    // Update is called once per frame
    void Start()
    {
        SpawnMonsters();
    }
}
