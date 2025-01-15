using Geo.Common.Internal.Boards;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Internal.Utils;
using Geo.Common.Public.QuizGames;
using Geo.Common.Public.Screens;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Geo.Common.Public
{

    public sealed class Game : MonoBehaviour
    {
        [SerializeField]
        private Board _board;

        private MainScreen _mainScreen;
        private IQuizManager _quizManager;
        private IImageAssetManager _imageAssetManager;
        private LoadingScreen _loadingScreen;
        private ScreenFactory _screenFactory;
        private IQuizGameFactory _quizHelper;
        private Camera _camera;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private TileReward _rewardPrefab;

        [Inject]
        public void Construct(
            IImageAssetManager imageAssetManager,
            IQuizManager quizManager,
            LoadingScreen loadingScreen,
            ScreenFactory screenFactory,
            IQuizGameFactory quizHelper,
            Camera camera)
        {
            _imageAssetManager = imageAssetManager;
            _quizManager = quizManager;
            _loadingScreen = loadingScreen;
            _screenFactory = screenFactory;
            _quizHelper = quizHelper;
            _camera = camera;
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
            _loadingScreen.FadeIn(0);

            StartMainGame().Forget();
        }

        public async Task StartMainGame()
        {
            _board.Spawn();

            await _quizManager.LoadAllCollectionsAsync();
            await _imageAssetManager.LoadAllCollectionsAsync();

            _mainScreen = await _screenFactory.CreateMainScreen();
            _mainScreen.OnRoll += OnRollClick;
            _mainScreen.SetCoins(_player.Score);

            _loadingScreen.FadeOut();
        }

        private void OnRollClick()
        {
            RollDiceAsync(CancellationToken.None).Forget();
        }

        private async Task RollDiceAsync(CancellationToken token)
        {
            _mainScreen.DisableRollButton();

            var result = _board.CalculateResult(_player.CurrnetIndex, Random.Range(1, Consts.MaxDiceValue + 1));
            _player.SetIndex(result.Indexes[^1]);

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
            await reward.ShowAwait(_camera, coins);
            Destroy(reward.gameObject);

            _mainScreen.AnimateCoins(_player.Score, _player.Score + coins);
            _player.AddScore(coins);
        }

        private async Task PlayQuizAsync(SpaceType spaceType)
        {
            await _loadingScreen.FadeInAsync(0.2f);

            QuizGameBase game = (spaceType == SpaceType.FlagQuiz) ? _quizHelper.CreateQuiz<FlagQuizGame>() : _quizHelper.CreateQuiz<TextQuizGame>();
            await game.LoadAsync();
            _board.Hide();

            var data = _quizManager.GetRandomQuizData(spaceType);
            game.PlayQuiz(data, _board.Show);

            _loadingScreen.FadeOut(0.2f);
        }

    }
}
