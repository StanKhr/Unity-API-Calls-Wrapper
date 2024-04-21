using System;
using Demo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ApiDemoUserPresenter : MonoBehaviour
    {
        #region Editor Fields

        [SerializeField] private ApiDemoUser _apiDemoUser;

        [Header("Views")]
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private Button _getButton;
        [SerializeField] private Button _postButton;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            _getButton.onClick.AddListener(GetButtonClickedCallback);
            _postButton.onClick.AddListener(PostButtonClickedCallback);
            _apiDemoUser.OnResultMessage += ResultMessageCallback;
        }

        private void OnDestroy()
        {
            _getButton.onClick.RemoveListener(GetButtonClickedCallback);
            _postButton.onClick.RemoveListener(PostButtonClickedCallback);
            _apiDemoUser.OnResultMessage -= ResultMessageCallback;
        }

        #endregion

        #region Methods

        private void GetButtonClickedCallback()
        {
            _apiDemoUser.Get();
        }
        
        private void PostButtonClickedCallback()
        {
            _apiDemoUser.Post();
        }

        private void ResultMessageCallback(string context)
        {
            _resultText.text = context;
        }

        #endregion
    }
}