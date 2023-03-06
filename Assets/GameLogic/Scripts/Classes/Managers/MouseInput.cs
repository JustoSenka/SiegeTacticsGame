using System;
using UnityEngine;

namespace Assets
{
    public class MouseInput
    {
        private bool IsUnderCell { get; set; }
        private HexCell HexUnderMouse { get; set; }

        private bool IsUnderEntity { get; set; }
        private Entity EntityUnderMouse { get; set; }

        public event Action<HexCell> HexPressedDown;
        public event Action MouseReleased;

        public event Action<HexCell> HexUnderMouseChanged;
        public event Action<Entity> EntityUnderMouseChanged;

        private HexCell m_MouseDownOnHex; // Used to remember which hex user pressed down but didn't release yet

        private Vector3 m_OldMousePosition;
        private bool m_IsDraggingMouse;

        public MouseInput() { }

        public void Update()
        {
            if (IsOrWasDraggingMouse())
                return;

            var clickedOnEntity = HandleClickingOnEntitys();

            // If did not click on selectable, look if clicked on any hex
            // If we already know that selectable was clicked, don't look if clicked on any hexes
            if (!clickedOnEntity)
                HandleClickingAndReleasingOnHexTiles();

            // This will not be in mobile version
            HandleHoveringHighlight();
        }

        private void HandleClickingAndReleasingOnHexTiles()
        {
            // Mouse down ont hex
            if (Input.GetKeyDown(KeyCode.Mouse0) && HexUnderMouse.IsValid)
            {
                m_MouseDownOnHex = HexUnderMouse;
                HexPressedDown?.Invoke(HexUnderMouse);
            }

            // Mouse released on hex
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                // If released on valid hex and did not drag the mouse, select the hex (unselect old one if needed)
                if (HexUnderMouse.IsValid && m_MouseDownOnHex == HexUnderMouse)
                    Game.Selection.ClickAndSelectHexCell(HexUnderMouse);

                else // If released mouse, but not on any hex, unselect old hex
                    Game.Selection.UnselectAll();

                // Clear the state hex being pressed down
                m_MouseDownOnHex = default;
            }
        }

        private bool HandleClickingOnEntitys()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (EntityUnderMouse != null)
                {
                    Game.Selection.ClickAndSelectEntity(EntityUnderMouse);

                    // else do nothing since same item was again selected
                    // Even if same item was selected, still return true so Hex selection code doesn't run
                    return true;
                }
            }
            return false;
        }

        private bool IsOrWasDraggingMouse()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
                MouseReleased?.Invoke();

            if (Input.GetKeyDown(KeyCode.Mouse0))
                m_OldMousePosition = Input.mousePosition;

            // return if was dragging frame before and now release
            if (m_IsDraggingMouse && Input.GetKeyUp(KeyCode.Mouse0))
                return true;

            // Return if still dragging mouse
            m_IsDraggingMouse = Input.GetKey(KeyCode.Mouse0) && Vector3.Distance(Input.mousePosition, m_OldMousePosition) > 3;
            if (m_IsDraggingMouse)
                return true;

            return false;
        }

        private void HandleHoveringHighlight()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(Layer.Ground, Layer.Entity)))
            {
                var selectableBehaviour = hit.collider.gameObject.transform.parent.gameObject.GetComponent<Entity>(); // unit behaviour
                if (selectableBehaviour == null) // Only select hex and deselect selectable if it was selected before
                {
                    var pos = HexUtility.WorldPointToHex(hit.point, 1);

                    IsUnderCell = true;
                    IsUnderEntity = false;

                    SetEntityUnderMouse(default);
                    SetHexUnderMouse(pos);
                }
                else // Select selectable and hex it is in
                {
                    IsUnderEntity = true;
                    SetEntityUnderMouse(selectableBehaviour.Cell);

                    IsUnderCell = true;
                    SetHexUnderMouse(selectableBehaviour.Cell);
                }
            }
            else // Reset all selections to default
            {
                IsUnderEntity = false;
                SetEntityUnderMouse(default);

                IsUnderCell = false;
                SetHexUnderMouse(default);
            }
        }

        private void SetHexUnderMouse(int2 pos)
        {
            var newHex = IsUnderCell ? Game.HexDatabase.GetHexCell(pos) : default;
            if (newHex != HexUnderMouse)
            {
                HexUnderMouse = newHex;
                HexUnderMouseChanged?.Invoke(HexUnderMouse);
            }
        }

        private void SetEntityUnderMouse(int2 pos)
        {
            var newEntity = IsUnderEntity ? Game.HexDatabase.GetEntity(pos) : default;
            if (newEntity != EntityUnderMouse)
            {
                EntityUnderMouse = newEntity;
                EntityUnderMouseChanged?.Invoke(EntityUnderMouse);
            }
        }
    }
}
