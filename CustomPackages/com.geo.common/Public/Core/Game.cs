using Geo.Common.Internal;
using Geo.Common.Internal.Boards;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Internal.Utils;
using Geo.Common.Public.QuizGames;
using Geo.Common.Public.Screens;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Geo.Common.Public.Core
{
    public sealed class Game : MonoBehaviour
    {
        private MainScreen _mainScreen;
        private IQuizManager _quizManager;
        private IImageAssetManager _imageAssetManager;
        private LoadingScreen _loadingScreen;
        private ScreenFactory _screenFactory;
        private IQuizGameFactory _quizGameFactory;
        private IUserStorage _playerStorage;

        private Player _player;
        private Board _board;

        [SerializeField]
        private TileReward _rewardPrefab;
        [SerializeField]
        private BoardData _boardData;

        [Inject]
        public void Construct(
            IImageAssetManager imageAssetManager,
            IQuizManager quizManager,
            LoadingScreen loadingScreen,
            ScreenFactory screenFactory,
            IQuizGameFactory quizFactory,
            IUserStorage playerStorage,
            Board board,
            Player player)
        {
            _imageAssetManager = imageAssetManager;
            _quizManager = quizManager;
            _loadingScreen = loadingScreen;
            _screenFactory = screenFactory;
            _quizGameFactory = quizFactory;
            _board = board;
            _player = player;
            _playerStorage = playerStorage;
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
            
            StartMainGame().Forget();
            SetPlayerOnBoard();
        }

        public async Task StartMainGame()
        {
            _loadingScreen.FadeIn(0);
            await _quizManager.LoadAllCollectionsAsync();
            await _imageAssetManager.LoadAllCollectionsAsync();

            _board.Spawn(_boardData);

            await CreateMainScreen();

            _loadingScreen.FadeOut();
        }

        private void SetPlayerOnBoard()
        {
            var indexOnBoard = _playerStorage.Index % _boardData.Spaces.Count;
            _playerStorage.SetIndex(indexOnBoard);
            _player.TeleportTo(_boardData.Spaces[indexOnBoard].Position);
        }

        private async Task CreateMainScreen()
        {
            _mainScreen = await _screenFactory.CreateMainScreen();
            _mainScreen.Initialize(_playerStorage.Coins, OnRollClick);
        }

        private int GetRandomDiceValue()
        {
            return Random.Range(1, Consts.MaxDiceValue + 1);
        }

        private void OnRollClick()
        {
            var result = _boardData.CalculateResult(_playerStorage.Index, GetRandomDiceValue());
            _playerStorage.SetIndex(result.Indexes[^1]);

            ExecuteRollDiceAsync(result, CancellationToken.None).Forget();
        }

        private async Task ExecuteRollDiceAsync(BoardResult result, CancellationToken token)
        {
            _mainScreen.DisableRollButton();

            await _board.AnimateMoveAsync(_player, result, token);

            _player.PlayFinishMoveAsync().Forget();

            await ExecuteTileActionAsync(result.Info.Space, token);

            _mainScreen.EnableRollButton();
        }

        private async Task ExecuteTileActionAsync(SpaceType type, CancellationToken token)
        {
            if (type == SpaceType.Empty)
            {
                _playerStorage.AddCoins(Consts.EmptyTileReward);
                await PlayRewardAnimationAsync(_player.transform.position, Consts.EmptyTileReward);
            }
            else
            {
                await PlayQuizAsync(type, token);
            }
        }

        private async Task PlayRewardAnimationAsync(Vector3 positin, int coins)
        {
            var reward = Instantiate(_rewardPrefab, positin, Quaternion.identity);
            reward.PlayAnimation(coins);
            var waitBeforeContinue = reward.GetAnimationDuratiion() * 0.5f;
            await Task.Delay(waitBeforeContinue.SecondsToTicks());
            _mainScreen.SetCoins(_playerStorage.Coins);
        }

        private async Task PlayQuizAsync(SpaceType spaceType, CancellationToken token)
        {
            await _loadingScreen.FadeInAsync(0.2f);

            var game = _quizGameFactory.CreateQuiz(spaceType);

            await game.LoadAsync();

            _board.Hide();
            _mainScreen.Hide();
            _loadingScreen.FadeOut();

            var data = _quizManager.GetRandomQuizData(spaceType);
            game.PlayQuiz(data, OnFinishGame);
        }

        private void OnFinishGame(QuizGameResult result)
        {
            _board.Show();
            _mainScreen.Show();

            if (!result.Win)
                return;

            _playerStorage.AddCoins(Consts.QuizReward);
            _mainScreen.SetCoins(_playerStorage.Coins);
        }

        #region for Quiz Tests
#if UNITY_EDITOR
        [ContextMenu("PlayTextQuiz")]
        private void PlayTextQuiz()
        {
            if (!Application.isPlaying) 
            { 
                Debug.LogWarning("It's working only in play");
                return;
            }

            PlayQuizAsync(SpaceType.TextQuiz, CancellationToken.None).Forget();
        }

        [ContextMenu("PlayFlagQuiz")]
        private void PlayFlagQuiz()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("It's working only in play");
                return;
            }

            PlayQuizAsync(SpaceType.FlagQuiz, CancellationToken.None).Forget();
        }
#endif
#endregion
    }
}