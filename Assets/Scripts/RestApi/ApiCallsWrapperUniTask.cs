using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RestApi
{
    public static class ApiCallsWrapperUniTask
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
            CancellationTokenSource = null;
        }

        public static async void Get(string uri, Action<string> successCallback, Action<string> errorCallback = null)
        {
            using var request = UnityWebRequest.Get(uri);

            request.SendWebRequest();

            if (!await WaitForRequest(request))
            {
                return;
            }

            HandleRequestResult(request, successCallback, errorCallback);
        }

        public static async void Post(string uri, Dictionary<string, string> fields, Action<string> successCallback,
            Action<string> errorCallback = null)
        {
            var form = new WWWForm();
            foreach (var key in fields.Keys)
            {
                form.AddField(key, fields[key]);
            }
            
            using var request = UnityWebRequest.PostWwwForm(uri, form.ToString());

            request.SendWebRequest();

            if (!await WaitForRequest(request))
            {
                return;
            }

            HandleRequestResult(request, successCallback, errorCallback);
        }

        /// <summary>
        /// Returns false if aborted / interrupted; else - true
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static async Task<bool> WaitForRequest(UnityWebRequest request)
        {
            ValidateCancellationToken();

            while (!request.isDone)
            {
                var isCancelled = await UniTask.Yield(cancellationToken: CancellationTokenSource.Token)
                    .SuppressCancellationThrow();
                if (!isCancelled)
                {
                    continue;
                }
                
                request.Abort();
                return false;
            }

            return true;
        }

        private static void HandleRequestResult(UnityWebRequest request, Action<string> successCallback,
            Action<string> errorCallback)
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = request.downloadHandler.text;
                successCallback?.Invoke(json);
                return;
            }

            errorCallback?.Invoke(CachedResultNames[(int) request.result]);
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