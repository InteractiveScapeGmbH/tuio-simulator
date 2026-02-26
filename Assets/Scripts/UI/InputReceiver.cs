using System;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Tuio20;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.UI
{
    public class InputReceiver : MonoBehaviour
    {
        [SerializeField] Tuio20Spawner _spawner;
        [SerializeField] private MouseClicker _mouseClicker;
        [SerializeField] private MouseDrager _mouseDrager;
        [SerializeField] private RadialMenu _radialMenuPrefab;
        [SerializeField] private RectTransform _radialParent;

        private RadialMenu _radialMenu;
        
        private void OnEnable()
        {
            _mouseClicker.OnLeftDown += AddPointer;
            _mouseClicker.OnLeftUp += RemovePointer;

            _mouseClicker.OnLeftDoubleClick += AddToken;
            // _mouseClicker.OnRightDoubleClick += SpawnMobile;

            _mouseDrager.OnMove += MovePointer;

            _mouseClicker.OnRightDown += ShowRadialMenu;
            _mouseClicker.OnRightUp += HideRadialMenu;

        }

        private void OnDisable()
        {
            _mouseClicker.OnLeftDown -= AddPointer;
            _mouseClicker.OnLeftUp -= RemovePointer;

            _mouseClicker.OnLeftDoubleClick -= AddToken;
            // _mouseClicker.OnRightDoubleClick -= SpawnMobile;

            _mouseDrager.OnMove -= MovePointer;

            _mouseClicker.OnRightDown -= ShowRadialMenu;
            _mouseClicker.OnRightUp -= HideRadialMenu;
        }

        public void Init(Tuio20Spawner spawner)
        {
            _radialMenu = Instantiate(_radialMenuPrefab, _radialParent);
            _radialMenu.transform.SetAsLastSibling();
            _radialMenu.Hide();
            _spawner = spawner;
        }

        private void HideRadialMenu(PointerEventData obj)
        {
            _radialMenu.Hide();

        }

        private void ShowRadialMenu(PointerEventData pointerEventData)
        {
            _radialMenu.SetPosition(pointerEventData.position);
            _radialMenu.Show();

        }


        private void MovePointer(PointerEventData pointerEventData)
        {
            _spawner.MovePointer(pointerEventData.position, pointerEventData.pointerId);
        }
        
        private void AddToken(Vector2 position)
        {
            _spawner.SpawnToken(position);
        }
        
        private void AddPointer(PointerEventData pointerEventData)
        {
            _spawner.SpawnPointer(pointerEventData.position, pointerEventData.pointerId);
          
        }
        
        private void RemovePointer(PointerEventData pointerEventData)
        {
            _spawner.RemovePointer(pointerEventData.pointerId);   
        }
    }
}