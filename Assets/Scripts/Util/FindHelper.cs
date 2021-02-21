using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class FindHelper
    {
        public static T GetOfType<T>(GameObject gameObject) where T : class
        {
            var bb = gameObject.GetComponent<T>();
            if (bb == null)
            {
                if (gameObject.transform.parent == null)
                {
                    return null;
                }
                bb = GetOfType<T>(gameObject.transform.parent.gameObject);
            }

            return bb;
        }
    }
}
