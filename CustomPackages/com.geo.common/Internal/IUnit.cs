using System.Threading.Tasks;
using UnityEngine;

namespace Geo.Common.Internal
{
    public interface IUnit
    {
        void TeleportTo(Vector3 position);
        Task MoveToAsync(Vector3 position, float duration = 0.3f);
        Task PlayFinishMoveAsync();
    }
}
