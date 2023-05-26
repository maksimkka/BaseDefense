using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Hero
{
    public class RegenerateHP : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_HeroData>> heroFilter = default;
        public void Run(IEcsSystems systems)
        {
            
        }
    }
}