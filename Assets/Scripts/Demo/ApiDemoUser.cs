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

        [SerializeField] private string _getUrl;
        [SerializeField] private string _postUrl;

        [Header("POST Fields")]
        [SerializeField] private string _postFieldName;
        [SerializeField] private string _postFieldValue;

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
            if (!ValidateUrl(_getUrl))
            {
                return;
            }
            
            ApiCallsWrapperUniTask.CancelCalls();
            ApiCallsWrapperUniTask.Get(
                _getUrl, 
                json => OnResultMessage?.Invoke(json),
                errorMessage => OnResultMessage?.Invoke(errorMessage)
            );
        }

        public void Post()
        {
            if (!ValidateUrl(_postUrl))
            {
                return;
            }

            var fields = new Dictionary<string, string>()
            {
                {_postFieldName, _postFieldValue}
            };
            
            ApiCallsWrapperUniTask.CancelCalls();
            ApiCallsWrapperUniTask.Post(
                _postUrl,
                fields,
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