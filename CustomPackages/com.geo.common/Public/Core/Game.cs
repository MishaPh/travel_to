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
        private IQuizGameFactory _quizHelper;
        private IUserStorage _playerStorage;

        private Camera _camera;
        private Board _board;
        private Player _player;

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
            IQuizGameFactory quizHelper,
            IUserStorage playerStorage,
            Camera camera,
            Board board,
            Player player)
        {
            _imageAssetManager = imageAssetManager;
            _quizManager = quizManager;
            _loadingScreen = loadingScreen;
            _screenFactory = screenFactory;
            _quizHelper = quizHelper;
            _camera = camera;
            _board = board;
            _player = player;
            _playerStorage = playerStorage;
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
            _loadingScreen.FadeIn(0);

            SetPlayerOnBoard();

            StartMainGame().Forget();
        }

        public async Task StartMainGame()
        {
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
            _mainScreen.OnRoll += OnRollClick;
            _mainScreen.SetCoins(_playerStorage.Coins);
        }

        private void OnRollClick()
        {
            RollDiceAsync(CancellationToken.None).Forget();
        }

        private async Task RollDiceAsync(CancellationToken token)
        {
            _mainScreen.DisableRollButton();

            var result = _board.CalculateResult(_playerStorage.Index, Random.Range(1, Consts.MaxDiceValue + 1));
            _playerStorage.SetIndex(result.Indexes[^1]);

            await _board.AnimateMoveAsync(_player, result, token);
            _player.PlayFinishMoveAsync().Forget();

            if (result.HitInfo.Space == SpaceType.Empty)
            {
                await GiveRewardAsync(10 * Random.Range(10, 100));
            }
            else
            {
                PlayQuizAsync(result.HitInfo.Space).Forget();
            }

            _mainScreen.EnableRollButton();
        }

        private async Task GiveRewardAsync(int coins)
        {
            var reward = Instantiate(_rewardPrefab, _player.transform.position, Quaternion.identity);
            reward.PlayAnimation(_camera, coins);

            await Task.Delay(Mathf.RoundToInt(Mathf.Min(reward.GetAnimationDuratiion(), 0.5f) * 1000));

            _playerStorage.AddCoins(coins);
            _mainScreen.AnimateCoinsTo(_playerStorage.Coins);
        }

        private async Task PlayQuizAsync(SpaceType spaceType)
        {
            await _loadingScreen.FadeInAsync(0.2f);

            QuizGameBase game = (spaceType == SpaceType.FlagQuiz) ? 
                _quizHelper.CreateQuiz<FlagQuizGame>() : 
                _quizHelper.CreateQuiz<TextQuizGame>();

            await game.LoadAsync();
            _board.Hide();
            _loadingScreen.FadeOut();

            var data = _quizManager.GetRandomQuizData(spaceType);
            game.PlayQuiz(data, FinishGame);
        }

        private void FinishGame(QuizGameResult result)
        {
            _board.Show();
            if (!result.Win)
                return;

            _playerStorage.AddCoins(5000);
            _mainScreen.AnimateCoinsTo(_playerStorage.Coins);
        }
#region for Quiz Tests
#if UNITY_EDITOR
        [ContextMenu("PlayTextQuiz")]
        private void PlayTextQuiz()
        {
            PlayQuizAsync(SpaceType.TextQuiz).Forget();
        }

        [ContextMenu("PlayFlagQuiz")]
        private void PlayFlagQuiz()
        {
            PlayQuizAsync(SpaceType.FlagQuiz).Forget();
        }
#endif
#endregion
    }
}