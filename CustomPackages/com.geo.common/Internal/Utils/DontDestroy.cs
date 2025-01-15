using UnityEngine;

namespace Geo.Common.Internal
{
    public class DontDestroy : MonoBehaviour
    {
        void Awake()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}
