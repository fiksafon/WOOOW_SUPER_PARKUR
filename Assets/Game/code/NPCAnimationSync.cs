
using UnityEngine;

[ExecuteAlways] // Позволяет скрипту работать даже в режиме редактора при превью
public class NPCAnimationSync : MonoBehaviour
{
    public Animator bodyAnimator;
    public Animator hairAnimator;
    public Animator outfitAnimator;
    public Animator weaponAnimator;

    void Update()
    {
        if (bodyAnimator == null) return;

        // Получаем текущее состояние анимации тела
        AnimatorStateInfo stateInfo = bodyAnimator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = stateInfo.normalizedTime;
        int stateHash = stateInfo.fullPathHash;
        float speed = bodyAnimator.speed;

        // Синхронизируем волосы, если они есть
        SyncAnimator(hairAnimator, stateHash, normalizedTime, speed);
        
        // Синхронизируем одежду
        SyncAnimator(outfitAnimator, stateHash, normalizedTime, speed);
        
        // Синхронизируем оружие
        SyncAnimator(weaponAnimator, stateHash, normalizedTime, speed);
    }

    void SyncAnimator(Animator targetAnimator, int stateHash, float normalizedTime, float speed)
    {
        if (targetAnimator != null && targetAnimator.runtimeAnimatorController != null)
        {
            // Проигрываем ту же анимацию на том же кадре
            targetAnimator.Play(stateHash, 0, normalizedTime);
            targetAnimator.speed = speed;
        }
    }
}