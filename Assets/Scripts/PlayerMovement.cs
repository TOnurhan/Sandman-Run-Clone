using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputSystem _inputSystem;
    [SerializeField] private float _dragSpeed;
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private Vector2 _clampVector;
    private Vector2 firstPos, targetPos;
    private bool canMove;

    public void SetInputSystem(InputSystem inputSystem) => _inputSystem = inputSystem; 

    public void Stop() 
    {
        canMove = false;
        _inputSystem.PointerDowned -= OnPointerDown;
        _inputSystem.PointerDragged -= OnPointerDrag;
    }


    public void Initialize()
    {
        canMove = true;
        _inputSystem.PointerDowned += OnPointerDown;
        _inputSystem.PointerDragged += OnPointerDrag;
    }

    private void Start()
    {
        //Initialize();
    }

    private void Update()
    {
        HandleMovement();
    }
    public void HandleMovement()
    {
        if (canMove)
        {
            var newX = Mathf.Clamp(transform.position.x, _clampVector.x, _clampVector.y);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            transform.position += new Vector3(0, 0, _moveSpeed * Time.deltaTime);
        }
    }

    private void OnPointerDown(PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_inputSystem.rectTransform, pointerEventData.position, pointerEventData.pressEventCamera, out firstPos);
    }

    private void OnPointerDrag(PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_inputSystem.rectTransform, pointerEventData.position, pointerEventData.pressEventCamera, out targetPos);

        if (pointerEventData.dragging)
        {
            var dragPos = targetPos - firstPos;
            var direction = new Vector3(dragPos.x / Screen.width, 0, 0);
            transform.position += direction * _dragSpeed * Time.deltaTime;
        }
        
            firstPos = Vector3.Lerp(firstPos, targetPos, 0.1f);
    }
}
