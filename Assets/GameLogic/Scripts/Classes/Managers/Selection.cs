using System;

namespace Assets
{
    public class Selection
    {
        public bool DoNotAllowOtherSystemsToChangeSelection { get; set; }

        public event Action<HexCell> HexClicked;
        public event Action<HexCell> HexSelected;
        public event Action<HexCell> HexUnselected;
        public event Action<HexCell> HexSelectionAborted;

        public event Action<Entity> EntityClicked;
        public event Action<Entity> EntitySelected;
        public event Action<Entity> EntityUnselected;
        public event Action<Entity> EntitySelectionAborted;

        public HexCell CurrentlySelectedHex { get; private set; }
        public Entity CurrentlySelectedEntity { get; private set; }

        public Selection() { }

        public void ClickAndSelectEntity(Entity entity)
        {
            if (entity != default)
                EntityClicked?.Invoke(entity);

            SelectEntity(entity);
        }

        public void SelectEntity(Entity entity)
        {
            if (DoNotAllowOtherSystemsToChangeSelection)
            {
                EntitySelectionAborted?.Invoke(entity);
                return;
            }

            if (entity != CurrentlySelectedEntity)
            {
                if (CurrentlySelectedEntity != default)
                    EntityUnselected?.Invoke(CurrentlySelectedEntity);

                CurrentlySelectedEntity = entity;

                if (entity != default)
                    EntitySelected?.Invoke(entity);
            }

            // Unselect hexCell if entity was selected
            if (entity != default)
                ClickAndSelectHexCell(default);
        }

        public void SelectCell(int2 cell) => SelectHexCell(Game.HexDatabase.GetHexCell(cell));
        public void ClickAndSelectCell(int2 cell) => ClickAndSelectHexCell(Game.HexDatabase.GetHexCell(cell));

        public void ClickAndSelectHexCell(HexCell hexCell)
        {
            if (hexCell.IsValid)
                HexClicked?.Invoke(hexCell);

            SelectHexCell(hexCell);
        }

        public void SelectHexCell(HexCell hexCell)
        {
            if (DoNotAllowOtherSystemsToChangeSelection)
            {
                HexSelectionAborted?.Invoke(hexCell);
                return;
            }

            if (hexCell != CurrentlySelectedHex)
            {
                if (CurrentlySelectedHex.IsValid)
                    HexUnselected?.Invoke(CurrentlySelectedHex);

                CurrentlySelectedHex = hexCell;

                if (hexCell.IsValid)
                    HexSelected?.Invoke(hexCell);
            }

            // Unselect entity if hexCell was selected
            if (hexCell.IsValid)
                ClickAndSelectEntity(default);
        }

        public void UnselectAll()
        {
            ClickAndSelectEntity(default);
            ClickAndSelectHexCell(default);
        }
    }
}
