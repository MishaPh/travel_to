using System.Threading;
using System.Threading.Tasks;

namespace Geo.Common.Internal.Boards
{
    public interface IBoard
    {
        void Spawn(BoardData boardData);
        Task AnimateMoveAsync(IPlayer unit, BoardResult result, CancellationToken token);
    }
}
