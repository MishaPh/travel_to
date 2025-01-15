using DG.Tweening;
using Geo.Common.Internal;
using Geo.Common.Internal.Boards;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Geo.Common.Public
{
    public interface ITileItem
    {
        void OnHit();
    }

    public sealed class Consts
    {
        public static readonly int MaxDiceValue = 6;
    }

    public sealed class Board : MonoBehaviour, IBoard
    {
        private readonly List<ITileItem> _tiles = new();

        [SerializeField]
        private BoardData _active;

        [SerializeField]
        private TileCollection _tileCollection;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
#if UNITY_EDITOR
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
#endif
            _tiles.Clear();
            foreach (var space in _active.Spaces)
            {
                var tile = Instantiate(_tileCollection.GetTile(space.Space), space.Position, Quaternion.identity, transform);
                _tiles.Add(tile);
            }
        }

        public BoardResult CalculateResult(int from, int dice)
        {
            var lastIndex = from;
            var list = new List<int>(dice);
            for (var i = 0; i < dice; i++)
            {
                lastIndex = (lastIndex + 1) % _active.Spaces.Count;
                list.Add(lastIndex);
            }

            return new BoardResult(dice, list, _active.Spaces[lastIndex]);
        }

        public async Task AnimateMoveAsync(IUnit unit, BoardResult result, CancellationToken token)
        {
            foreach (var index in result.Indexes)
            {
                var tile = _tiles[index];
                await unit.MoveToAsync(_active.Spaces[index].Position);
                tile.OnHit();
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
