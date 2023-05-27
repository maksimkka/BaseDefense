using Code.EndGame;
using Code.Enemy;
using Code.Ground;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public sealed class s_HeroMove : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HeroData, CurrentGroundData>, Exc<EndGameMarker>> _heroFilter = default;
        private readonly EcsFilterInject<Inc<EnemyData>, Exc<EndGameMarker>> _enemyFilter = default;
        private readonly EcsFilterInject<Inc<WeaponData>, Exc<EndGameMarker>> _weaponFilter = default;
        private readonly EcsPoolInject<CanShootMarker> m_CanShoot = default;
        private readonly EcsCustomInject<FloatingJoystick> _joystick = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _heroFilter.Value)
            {
                ref var hero = ref _heroFilter.Pools.Inc1.Get(entity);
                ref var currentGround = ref _heroFilter.Pools.Inc2.Get(entity);
                Move(ref hero);
                IsCheckDistance(ref hero, ref currentGround);
                Rotate(ref hero);
            }
        }

        private void Move(ref HeroData heroData)
        {
            heroData.HeroRigidBody.velocity = new Vector3(_joystick.Value.Horizontal * heroData.Speed,
                heroData.HeroRigidBody.velocity.y, _joystick.Value.Vertical * heroData.Speed);
            
        }

        private void IsCheckDistance(ref HeroData heroData, ref CurrentGroundData currentGroundData)
        {
            heroData.TargetRotation = null;
            float closestDistance = Mathf.Infinity;
            
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(enemyEntity);
                if (!enemy.EnemyGameObject.gameObject.activeSelf) continue;
                var distance = Vector3.Distance(heroData.HeroGameObject.transform.position,
                    enemy.EnemyGameObject.transform.position);

                if (!currentGroundData.IsBaseGround && distance < closestDistance && distance <= heroData.DetectionDistance)
                {
                    closestDistance = distance;
                    heroData.TargetRotation = enemy.EnemyGameObject.transform;
                }
            }
        }
        
        private void Rotate(ref HeroData heroData)
        {
            if (heroData.TargetRotation == null)
            {
                AddShootMarker(false);
                if (_joystick.Value.Horizontal != 0 || _joystick.Value.Vertical != 0)
                {
                    heroData.HeroGameObject.transform.rotation = Quaternion.LookRotation(new Vector3(
                        heroData.HeroRigidBody.velocity.x, 
                        0,
                        heroData.HeroRigidBody.velocity.z));

                    heroData.Animator.PlayAnimation(heroData.RunAnimationHash);
                }

                else if(_joystick.Value.Horizontal == 0 && _joystick.Value.Vertical == 0)
                {
                    heroData.Animator.PlayAnimation(heroData.IdleAnimationHash);
                }
            }

            else if(heroData.TargetRotation != null)
            {
                var originalRotation = heroData.HeroGameObject.transform.eulerAngles;
                heroData.HeroGameObject.transform.LookAt(new Vector3(
                    heroData.TargetRotation.position.x,
                    heroData.HeroGameObject.transform.position.y,
                    heroData.TargetRotation.position.z));
                
                heroData.HeroGameObject.transform.eulerAngles = new Vector3(
                    originalRotation.x, 
                    heroData.HeroGameObject.transform.eulerAngles.y, 
                    originalRotation.z);
                AddShootMarker(true);
                ChangeAnimation(ref heroData);
            }
        }

        private void AddShootMarker(bool isCanShoot)
        {
            foreach (var entity in _weaponFilter.Value)
            {
                switch (isCanShoot)
                {
                    case true when !m_CanShoot.Value.Has(entity):
                        m_CanShoot.Value.Add(entity);
                        break;
                    case false when m_CanShoot.Value.Has(entity):
                        m_CanShoot.Value.Del(entity);
                        break;
                }
            }
        }

        private void ChangeAnimation(ref HeroData heroData)
        {
            if (_joystick.Value.Horizontal != 0 || _joystick.Value.Vertical != 0)
            {
                heroData.Animator.PlayAnimation(heroData.RiffleWalkAnimationHash);
            }
            else if(_joystick.Value.Horizontal == 0 && _joystick.Value.Vertical == 0)
            {
                heroData.Animator.PlayAnimation(heroData.RiffleIdleAnimation);
            }
        }
    }
}