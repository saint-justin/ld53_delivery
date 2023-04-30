using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private EventSystem _eventSystem;

	[SerializeField]
	private bool _gridSnap;

	[SerializeField][Range(0f, 0.99f)]
	private float _smoothing;

	[SerializeField]
	private Color _validColor;

	[SerializeField]
	private Color _invalidColor;

	public bool HasItem { get { return _item != null; } }

	private GraphicRaycaster _raycaster;

	private Image _image;

	private ItemUI _item;

	private GameObject _prevHit;
	private GameObject _currentHit;

	private bool _wasValid;


	public void PickItem(ItemUI item)
	{
		_item = item;

		//_item.transform.SetParent(transform);

		_item.transform.position = transform.position;

		_item.PickItem();

		_image.enabled = false;

		Debug.Log("Pick Item");

		_wasValid = false;
		_item.SetColor(_invalidColor);
	}


	public void PlaceItem(SlotUI slot)
	{
		if (_item.CheckPlacement(out bool onGrid, out Vector3 snapPosition))
		{
			_item.PlaceItem(snapPosition);

			_item = null;

			_image.enabled = true;
		}

		Debug.Log("Place Item");
	}


	public void DeleteItem()
	{
		if (_item != null)
		{
			Destroy(_item.gameObject);

			_item = null;

			_image.enabled = true;
		}
	}


	private void Start()
	{
		_raycaster = _canvas.GetComponent<GraphicRaycaster>();
		_image = GetComponent<Image>();

		Cursor.visible = false;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 position);
		transform.localPosition = position;
	}


	private void Update()
	{
		// Move the item towards the current mouse position
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out Vector2 position);
		transform.localPosition = Vector3.Lerp(transform.localPosition, position, 1f - _smoothing);


		if (_item != null)
		{
			// Need to temporarilly move the item to the cursor location to perform the raycasts
			Vector3 prevPosition = _item.transform.position;
			_item.transform.position = transform.position;

			bool isValid = _item.CheckPlacement(out bool onGrid, out Vector3 snapPostition);

			if (isValid != _wasValid)
			{
				Debug.Log(snapPostition);

				_wasValid = isValid;

				if (isValid)
				{
					_item.SetColor(_validColor);
				}
				else
				{
					_item.SetColor(_invalidColor);
				}
			}


			if (onGrid && _gridSnap)
			{
				_item.transform.position = Vector3.Lerp(prevPosition, snapPostition, 1f - _smoothing);
				_item.SetRaycastDebug(snapPostition);
			}
		}
	}
}
