using System.Threading;
using System.Threading.Tasks;

namespace Geo.Common.Internal.Boards
{
    public interface IBoard
    {
        void Spawn(BoardData boardData);
        BoardResult CalculateResult(int from, int by);
        Task AnimateMoveAsync(IUnit unit, BoardResult result, CancellationToken token);
    }
}
