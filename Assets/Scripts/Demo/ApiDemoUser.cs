using System;
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

        [SerializeField] private string _url;

        #endregion

        #region Unity Callbacks

        private void OnDestroy()
        {
            ApiCallsWrapper.CancelCalls();
        }

        #endregion

        #region Methods

        public void Get()
        {
            if (string.IsNullOrEmpty(_url))
            {
                OnResultMessage?.Invoke("URL is null or empty.");
                return;
            }
            
            ApiCallsWrapper.Get(
                _url, 
                json => OnResultMessage?.Invoke(json),
                errorMessage => OnResultMessage?.Invoke(errorMessage)
            );
        }

        public void Post()
        {
            
        }

        #endregion
    }
}