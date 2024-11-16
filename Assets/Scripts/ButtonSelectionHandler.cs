using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _verticalMoveAmount = 1f;
    [SerializeField] private float _moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float _scaleAmount = 1.1f;

    private Vector3 _startPos;
    private Vector3 _startScale;

    private void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
    }

    private IEnumerator MoveButton(bool startingAnimation)
    {
        Vector3 _endPos;
        Vector3 _endScale;

        float elapsedTime = 0f;
        while (elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _moveTime;

            if(startingAnimation)
            {
                _endPos = _startPos + new Vector3(0f, _verticalMoveAmount, 0f);
                _endScale = _startScale * _scaleAmount;
            }
            else
            {
                _endPos = _startPos;
                _endScale = _startScale;
            }

            // actually calculate the lerped amounts
            transform.position = Vector3.Lerp(transform.position, _endPos, t);
            transform.localScale = Vector3.Lerp(transform.localScale, _endScale, t);

            transform.position = _endPos;
            transform.localScale = _endScale;

            yield return null;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // select the button
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // deselect the button
        if (eventData.selectedObject == gameObject)
        {
            eventData.selectedObject = null;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
         if (MenuManager.Instance != null)
        {
            MenuManager.Instance.TriggerButtonAnimation(gameObject, true); // Starting animation
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
         if (MenuManager.Instance != null)
        {
            MenuManager.Instance.TriggerButtonAnimation(gameObject, false); // Starting animation
        }
    }
}
