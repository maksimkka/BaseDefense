using System;
using System.Linq;
using Code.Logger;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Hero
{
    public class HeroAnimation
    {
        // private readonly int _idleAnimation = Animator.StringToHash("DynIdle");
        // private readonly int _runAnimation = Animator.StringToHash("Running");
        // private readonly int _riffleWalkAnimation = Animator.StringToHash("RiffleWalk");
        private readonly Animator _animator;
        private int _hashCodeCurrentPlayedAnimation;
        public HeroAnimation(Animator animator)
        {
            _animator = animator;
        }

        // public void IdleAnimation()
        // {
        //     TriggerAnimation(_idleAnimation);
        // }
        // public void RunAnimation()
        // {
        //     TriggerAnimation(_runAnimation);
        // }
        //
        // public void RiffleWalkAnimation()
        // {
        //     TriggerAnimation(_riffleWalkAnimation);
        // }
        
        public void PlayAnimation(int hashAnimation)
        {
            if(_hashCodeCurrentPlayedAnimation == hashAnimation) return;
            _hashCodeCurrentPlayedAnimation = hashAnimation;

            ResetAllTriggers(_animator);
            _animator.SetTrigger(hashAnimation);
        }

        // public async void GetTimeAnimation(int hashFirstAnimation, int hashSecondAnimation)
        // {
        //     // AnimationClip clip = _animator.runtimeAnimatorController.animationClips
        //     //     .FirstOrDefault(a => a.GetHashCode() == hashFirstAnimation);
        //
        //     PlayAnimation(hashFirstAnimation);
        //     var delay = _animator.GetCurrentAnimatorStateInfo(0).length;
        //     $"{Time.frameCount} _____ {delay} ___ {_animator.GetCurrentAnimatorClipInfo(0).Length.ToString()}".Colored(Color.cyan).Log();
        //     //await UniTask.Delay(TimeSpan.FromSeconds(delay));
        //     await UniTask.Delay(TimeSpan.FromSeconds(delay));
        //     $"22222222222222 {Time.frameCount}".Colored(Color.cyan).Log();
        //     //var delay = clip != null ? clip.length : (float?)null;
        //     //if (delay != null) await UniTask.Delay(TimeSpan.FromSeconds((float)delay));
        //     
        //     PlayAnimation(hashSecondAnimation);
        //     
        //     //if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashFirstAnimation)
        //     //{
        //     // Получаем время выполнения анимации по хэшу
        //     //float animationTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //     //Debug.Log("Время выполнения анимации: " + animationTime);
        //     //}
        // }
        
        public async void GetTimeAnimation(int hashFirstAnimation, int hashSecondAnimation)
        {
            // Проигрываем первую анимацию
            //_animator.SetTrigger("FirstAnimation");
            PlayAnimation(hashFirstAnimation);
            $"{Time.frameCount} 11111111".Colored(Color.cyan).Log();

            // Ожидаем окончания анимации
            await UniTask.DelayFrame(1); // Подождать 1 кадр, чтобы убедиться, что анимация началась

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                await UniTask.Yield();
            }

            $"{Time.frameCount} 222222222".Colored(Color.cyan).Log();
            // Запускаем вторую анимацию
            PlayAnimation(hashSecondAnimation);
            //_animator.SetTrigger("SecondAnimation");
        }
        
        
        private void ResetAllTriggers(Animator animator)
        {
            var parameters = animator.parameters;
            for (var i = 0; i < animator.parameters.Length; i++)
            {
                if (parameters[i].type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(parameters[i].name);
                }
            }
        }
    }
}