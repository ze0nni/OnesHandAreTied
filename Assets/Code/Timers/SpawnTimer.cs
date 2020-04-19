using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timers
{
    sealed class SpawnTimer
    {
        readonly int limit;
        readonly float minTimeout;
        readonly float timeoutRange;

        private float delay = 0;

        public SpawnTimer(
            int limit,
            float minTimeout,
            float maxTimeout
        ) {
            if (limit <= 0)
            {
                throw new System.ArgumentOutOfRangeException();
            }
            this.limit = limit;

            if (minTimeout > maxTimeout) {
                throw new System.ArgumentOutOfRangeException();
            }
            this.minTimeout = minTimeout;
            this.timeoutRange = (maxTimeout - minTimeout);
        }

        public bool Tirgger(int objectsCount, float deltaTime) {
            delay -= deltaTime;
            if (objectsCount < limit && delay <= 0) {
                // для чистоты, UnityEngine.Random следовало бы заменить на System.Random и передавать в конструкторе
                delay = minTimeout + UnityEngine.Random.value * timeoutRange;
                return true;
            } else {
                return false;
            }
        }
    }
}
