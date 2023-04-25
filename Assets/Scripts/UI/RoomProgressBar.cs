using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomProgressBar : MonoBehaviour
{
    [SerializeField] private List<Room> _rooms;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _percentage;
    [SerializeField] private Slider _slider;

    private Room _currentRoom;

    private void Start()
    {
        _slider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        foreach (Room room in _rooms)
        {
            room.PlayerEnterRoom += OnPlayerEnterRoom;
        }
    }

    private void OnPlayerEnterRoom(Room room)
    {
        if(_currentRoom != null)
        {
            _currentRoom.DonePercentageChanged -= SetSliderValue;
        }
        _currentRoom = room;
        _slider.gameObject.SetActive(true);
        SetSliderValue();
        _fillImage.color = _currentRoom.Color;
        _currentRoom.DonePercentageChanged += SetSliderValue;
    }

    private void SetSliderValue()
    {
        float percent = Mathf.Round(_currentRoom.DonePercentage * 100);
        _percentage.text = percent.ToString() + "%";
        _slider.value = _currentRoom.DonePercentage;
    }
}
