using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Hero
{
    public class AnimationSwitcher
    {
        private readonly CancellationTokenSource _tokenSources;
        private readonly Animator _animator;
        private int _hashCodeCurrentPlayedAnimation;
        public AnimationSwitcher(Animator animator, CancellationTokenSource tokenSource)
        {
            _animator = animator;
            _tokenSources = tokenSource;
        }

        public void PlayAnimation(int hashAnimation)
        {
            if(_hashCodeCurrentPlayedAnimation == hashAnimation) return;
            _hashCodeCurrentPlayedAnimation = hashAnimation;

            ResetAllTriggers(_animator);
            _animator.SetTrigger(hashAnimation);
        }

        public async void GetTimeAnimation(int hashFirstAnimation, int hashSecondAnimation)
        {
            PlayAnimation(hashFirstAnimation);
            await UniTask.DelayFrame(1);

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                await UniTask.Yield(cancellationToken: _tokenSources.Token);
            }

            PlayAnimation(hashSecondAnimation);
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