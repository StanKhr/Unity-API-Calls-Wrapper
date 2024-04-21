using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace RestApi
{
    public static class ApiCallsWrapper
    {
        #region Fields

        private static string[] _cachedResultNames;

        #endregion
        
        #region Properties

        private static CancellationTokenSource CancellationTokenSource { get; set; }
        private static string[] CachedResultNames =>
            _cachedResultNames ??= Enum.GetNames(typeof(UnityWebRequest.Result));

        #endregion
        
        #region Public Methods

        public static void CancelCalls()
        {
            CancellationTokenSource?.Cancel();
        }

        public static async void Get(string url, Action<string> successCallback, Action<string> errorCallback = null)
        {
            ValidateCancellationToken();

            using var request = UnityWebRequest.Get(url);

            request.SendWebRequest();

            while (!request.isDone)
            {
                var isCancelled = await UniTask.Yield(cancellationToken: CancellationTokenSource.Token)
                    .SuppressCancellationThrow();
                if (!isCancelled)
                {
                    continue;
                }
                
                return;
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = request.downloadHandler.text;
                successCallback?.Invoke(json);
                return;
            }

            errorCallback?.Invoke(CachedResultNames[(int)request.result]);
        }

        #endregion

        #region Methods

        private static void ValidateCancellationToken()
        {
            if (CancellationTokenSource != null)
            {
                return;
            }

            CancellationTokenSource = new CancellationTokenSource();
        }
        
        #endregion
    }
}
