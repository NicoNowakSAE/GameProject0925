using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData) => AudioManager.Instance.PlaySound("ButtonClick");
    public void OnPointerEnter(PointerEventData eventData) => AudioManager.Instance.PlaySound("ButtonHover");
}
