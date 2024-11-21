using System;
using TuioSimulator.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace TuioSimulator.UI
{
    public class InterfaceManager : MonoBehaviour
    {
        [SerializeField] private MouseClicker _background;
        [SerializeField] private SimulatorMenu _defaultMenu;

        private void OnEnable()
        {
            _background.OnLeftClick += CloseDefaultMenu;
            _background.OnRightClick += OpenDefaultMenu;
        }

        private void OnDisable()
        {
            _background.OnLeftClick -= CloseDefaultMenu;
            _background.OnRightClick -= OpenDefaultMenu;
        }

        private void CloseDefaultMenu(Vector2 position)
        {
            _defaultMenu.Close();
        }

        private void OpenDefaultMenu(Vector2 position)
        {
            _defaultMenu.Open(position);
        }
    }
}