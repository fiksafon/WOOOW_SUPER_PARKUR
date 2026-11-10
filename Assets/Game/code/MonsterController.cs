using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
   public static event Action<int> _monsterPicked;

   private int _monstersCount = 0;

   public void MonsterPickedCommand()
   {
        _monstersCount++;
         _monsterPicked?.Invoke(_monstersCount);
         Debug.Log("Monster picked");
   }
}
