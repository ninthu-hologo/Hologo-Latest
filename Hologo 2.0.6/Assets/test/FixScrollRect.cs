using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixScrollRect: MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler, IScrollHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
	public ScrollRect MainScroll;
	private bool isAvaliable = false;
	private bool isHover = false;
	private bool oldInteractive = false;

	void Update()
	{
		if (oldInteractive != this.GetComponent<InputField> ().interactable)
		{
			if (this.GetComponent<InputField> ().interactable)
			{
				this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.normalColor;
			}
			else
			{
				this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.disabledColor;
			}
			
			oldInteractive = this.GetComponent<InputField> ().interactable;
		}

		if (this.GetComponent<InputField> ().interactable)
		{
			if (Input.GetMouseButton (0))
			{
				if (!this.isHover)
				{
					this.GetComponent<InputField> ().enabled = false;
				}
			}
		}
	}

	void Awake()
	{
		if (this.GetComponent<InputField>().interactable)
		{
			this.GetComponent<InputField>().enabled = false;
			this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.normalColor;
		}
		else
		{
			this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.disabledColor;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		MainScroll.OnBeginDrag(eventData);

		if (this.GetComponent<InputField> ().interactable)
		{
			this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.normalColor;

			this.GetComponent<InputField> ().enabled = false;
			this.isAvaliable = false;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		MainScroll.OnDrag(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		MainScroll.OnEndDrag(eventData);

		if (this.GetComponent<InputField> ().interactable)
		{
			this.GetComponent<InputField> ().enabled = false;
			this.isAvaliable = true;
		}
	}

	public void OnScroll(PointerEventData data)
	{
		MainScroll.OnScroll(data);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.GetComponent<InputField> ().interactable)
		{
			if(isAvaliable)
			{
				this.GetComponent<InputField>().enabled = true;
				this.GetComponent<InputField> ().Select ();
			}

			this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.normalColor;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.GetComponent<InputField> ().interactable)
		{
			if (eventData.pointerEnter == this.gameObject)
			{
				this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.pressedColor;
				
				this.GetComponent<InputField> ().enabled = false;
				this.isAvaliable = true;
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(this.GetComponent<InputField>().interactable)
		{
			this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.highlightedColor;
		}

		this.isHover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if(this.GetComponent<InputField>().interactable)
		{
			this.GetComponent<Image> ().color = this.GetComponent<InputField> ().colors.normalColor;
		}

		this.isHover = false;
	}
}
