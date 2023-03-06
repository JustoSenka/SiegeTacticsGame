using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class HexDatabase
    {
        private IDictionary<int2, HexCell> m_Cells;
        private IList<Entity> m_Entities;

        public HexDatabase()
        {
            m_Cells = new Dictionary<int2, HexCell>();
            m_Entities = new List<Entity>();
        }

        public void Start()
        {

        }

        public HexCell GetHexCell(int2 pos)
        {
            if (!m_Cells.TryGetValue(pos, out HexCell hexCell))
            {
                hexCell = new HexCell(pos);
                m_Cells[pos] = hexCell;
            }

            return hexCell;
        }

        public void UpdateHexCell(HexCell hex)
        {
            m_Cells[hex.Position] = hex;
        }

        public Entity GetEntity(int2 pos)
        {
            return m_Entities.FirstOrDefault(e => e.Cell == pos);
        }

        public void AddNewEntity(Entity obj)
        {
            if (m_Entities.Contains(obj))
            {
                Debug.LogWarning($"Entity is already in HexDatabase: {obj}");
                return;
            }

            m_Entities.Add(obj);

            var hex = GetHexCell(obj.Cell);
            hex.Type = HexType.Unit;
            UpdateHexCell(hex);
        }

        public void RemoveSelectable(Entity obj)
        {
            if (!m_Entities.Contains(obj))
            {
                Debug.LogWarning($"Selectable was not found when in HexDatabase when trying to remove it: {obj}");
                return;
            }

            m_Entities.Remove(obj);

            var hex = GetHexCell(obj.Cell);
            hex.ResetHexType();
            UpdateHexCell(hex);
        }
    }
}
