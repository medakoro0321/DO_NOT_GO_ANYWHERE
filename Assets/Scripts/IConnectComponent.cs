using UnityEngine;

namespace IConnectComponent
{
    public static class Components
    {
        /// <summary>
        /// 指定されたGameObject名からコンポーネントを取得
        /// </summary>
        /// <typeparam name="T">取得したいコンポーネントの型</typeparam>
        /// <param name="gameObjectName">GameObjectの名前</param>
        /// <returns>見つかったコンポーネント（見つからない場合はnull）</returns>
        public static T GetComponent<T>(string gameObjectName) where T : Component // 型安全でComponentのみに限定
        {
            GameObject obj = GameObject.Find(gameObjectName);
            
            if (obj == null)
            {
                Debug.LogError($"[Components] GameObject '{gameObjectName}' が見つかりません");
                return null;
            }
            
            T component = obj.GetComponent<T>();

            if (component != null) return component;
            Debug.LogError($"[Components] GameObject '{gameObjectName}' にコンポーネントが見つかりません");
            return null;

        }
        
        /// <summary>
        /// 指定されたGameObjectからコンポーネントを取得
        /// </summary>
        /// <typeparam name="T">取得したいコンポーネントの型</typeparam>
        /// <param name="gameObject">GameObject</param>
        /// <returns>見つかったコンポーネント（見つからない場合はnull）</returns>
        public static T GetComponent<T>(GameObject gameObject) where T : Component
        {
            if (gameObject == null)
            {
                Debug.LogError("[Components] GameObjectがnullです");
                return null;
            }
            
            T component = gameObject.GetComponent<T>();

            if (component != null) return component;
            Debug.LogError($"[Components] GameObject '{gameObject.name}' にコンポーネントが見つかりません");
            return null;

        }
    }
}