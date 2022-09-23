using DG.Tweening;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    private CanvasGroup _panelCanvas;
    private float _showPanelTime = 0.5f;

    private bool _isShowing = false;

    private void Awake()
    {
        _panelCanvas = GetComponent<CanvasGroup>();
    }

    public void ShowPanel()
    {
        if (!_isShowing)
        {
            _isShowing = true;

            _panelCanvas.blocksRaycasts = true;

            transform.DOScale(1, _showPanelTime)
                     .SetEase(Ease.OutBounce)
                     .SetUpdate(true)
                     .Play();
        }
    }

    public void HidePanel()
    {
        if (_isShowing)
        {
            _isShowing = false;

            _panelCanvas.blocksRaycasts = false;

            transform.DOScale(0, _showPanelTime)
                     .SetEase(Ease.InBack)
                     .SetUpdate(true)
                     .Play();
        }
    }
}
