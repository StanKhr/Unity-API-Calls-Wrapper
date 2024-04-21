using System;
using RestApi;
using UnityEngine;

namespace Demo
{
    public class ApiDemoUser : MonoBehaviour
    {
        #region Editor Fields

        [SerializeField] private string _url;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            if (string.IsNullOrEmpty(_url))
            {
                Debug.LogError($"URL is null or empty.");
                return;
            }

            ApiCallsWrapper.Get(
                _url, 
                json => Debug.Log($"Success: {json}"),
                errorMessage => Debug.LogError($"Error call: {errorMessage}")
                );
        }

        private void OnDestroy()
        {
            ApiCallsWrapper.CancelCalls();
        }

        #endregion
    }
}