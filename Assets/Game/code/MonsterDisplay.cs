using System;
using TMPro;
using UnityEngine;

public class MonsterDisplay : MonoBehaviour
{
   [SerializeField] private TMP_Text _Text;

    private void OnEnable() 
    {
      MonsterController._monsterPicked += OnMonsterPicked;
    } 
   private void OnDisable()
   {
      MonsterController._monsterPicked -= OnMonsterPicked;
   }


    private void OnMonsterPicked(int count)
   {
        _Text.text = count.ToString();
   } 

        
}
