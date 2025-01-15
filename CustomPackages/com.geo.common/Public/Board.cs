using Geo.Common.Internal;
using Geo.Common.Internal.Boards;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Geo.Common.Public
{
    public sealed class Board : MonoBehaviour, IBoard
    {
        private readonly List<ITileItem> _tiles = new();

        private BoardData _boardData = null;

        private TileCollection _tileCollection;

        [Inject]
        public void Construct(TileCollection tileCollection)
        {
            _tileCollection = tileCollection;
        }

        public void Spawn(BoardData boardData)
        {
            _boardData = boardData;

            Clear();

            foreach (var space in _boardData.Spaces)
            {
                var tile = Instantiate(_tileCollection.GetTile(space.Space), space.Position, Quaternion.identity, transform);
                _tiles.Add(tile);
            }
        }

        private void Clear()
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            _tiles.Clear();
        }

        public BoardResult CalculateResult(int from, int dice)
        {
            var lastIndex = from;
            var indexes = new int[dice];
            for (var i = 0; i < dice; i++)
            {
                lastIndex = (lastIndex + 1) % _boardData.Spaces.Count;
                indexes[i] = lastIndex;
            }

            return new BoardResult(dice, indexes, _boardData.Spaces[lastIndex]);
        }

        public async Task AnimateMoveAsync(IUnit unit, BoardResult result, CancellationToken token)
        {
            foreach (var index in result.Indexes)
            {
                await unit.MoveToAsync(_boardData.Spaces[index].Position);
                _tiles[index].OnHit();

                if (token.IsCancellationRequested)
                    return;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
