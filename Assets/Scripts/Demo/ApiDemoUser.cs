using System;
using System.Collections.Generic;
using RestApi;
using UnityEngine;

namespace Demo
{
    public class ApiDemoUser : MonoBehaviour
    {
        #region Events

        public event Action<string> OnResultMessage; 

        #endregion
        
        #region Editor Fields

        [SerializeField] private string _getUri;
        [SerializeField] private string _postUri;

        [Header("POST Fields")]
        [SerializeField] private string _postFieldName;
        [SerializeField] private string _postFieldValue;

        #endregion

        #region Fields

        private static readonly Dictionary<string, string> CachedPostFieldsDictionary = new();

        #endregion

        #region Unity Callbacks

        private void OnDestroy()
        {
            ApiCallsWrapperUniTask.CancelCalls();
        }

        #endregion

        #region Methods

        public void Get()
        {
            if (!ValidateUrl(_getUri))
            {
                return;
            }
            
            ApiCallsWrapperUniTask.CancelCalls();
            ApiCallsWrapperUniTask.Get(
                _getUri, 
                json => OnResultMessage?.Invoke(json),
                errorMessage => OnResultMessage?.Invoke(errorMessage)
            );
        }

        public void Post()
        {
            if (!ValidateUrl(_postUri))
            {
                return;
            }

            CachedPostFieldsDictionary.Clear();
            CachedPostFieldsDictionary.Add(_postFieldName, _postFieldValue);
            
            ApiCallsWrapperUniTask.CancelCalls();
            ApiCallsWrapperUniTask.Post(
                _postUri,
                CachedPostFieldsDictionary,
                json => OnResultMessage?.Invoke(json),
                errorMessage => OnResultMessage?.Invoke(errorMessage)
            );
        }

        private bool ValidateUrl(string url)
        {
            var invalidUrl = string.IsNullOrEmpty(url);
            if (invalidUrl)
            {
                OnResultMessage?.Invoke("URL is null or empty.");
            }
            
            return !invalidUrl;
        }

        #endregion
    }
}