using UnityEngine;

namespace Geo.Common.Public.Screens
{
    public abstract class ScreenBase : MonoBehaviour
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
