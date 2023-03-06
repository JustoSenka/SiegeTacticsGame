using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class TurnOrderDisplay : MonoBehaviour
    {
        public RectTransform ParentPanel;
        public RectTransform TurnOrderIndicatorPrefab;

        public int MaxTurnOrderIndicatorCount = 8;

        private Dictionary<Entity, TurnOrderIndicator> m_Dictionary;

        public void Start()
        {
            m_Dictionary = new Dictionary<Entity, TurnOrderIndicator>();
            UpdateTurnQueue();

            //TurnManager.TurnQueueChanged += UpdateTurnQueue;
            //TurnManager.TurnQueueElementRemoved += OnElementRemoved;
        }

        private void UpdateTurnQueue()
        {
            /*
            var queue = TurnManager.GetCurrentTurnQueue();
            var index = 0;

            foreach (var entity in queue)
            {
                if (!m_Dictionary.ContainsKey(entity))
                {
                    var go = Instantiate(TurnOrderIndicatorPrefab, Vector3.zero, Quaternion.identity, ParentPanel);
                    m_Dictionary[entity] = go.GetComponent<TurnOrderIndicator>();
                }

                var turnIndicator = m_Dictionary[entity];
                turnIndicator.turnOrder = index;

                index++;
            }

            foreach (var (entity, turnIndicator) in m_Dictionary)
            {
                UpdateSingleTurnIndicatorObject(entity, turnIndicator, m_Dictionary.Count);
            }
            */
        }

        private void UpdateSingleTurnIndicatorObject(Entity entity, TurnOrderIndicator turnIndicator, int queueLength)
        {
            turnIndicator.text.text = entity.Cell.ToString();

            var spaceForOneElement = turnIndicator.rect.rect.width + 35;

            var indicatorCount = Math.Min(queueLength, MaxTurnOrderIndicatorCount);
            var xOffset = (1 - indicatorCount) * spaceForOneElement / 2 + turnIndicator.turnOrder * spaceForOneElement;
            turnIndicator.rect.anchoredPosition = new Vector3(xOffset, 0);
        }

        private void OnElementRemoved(Entity obj)
        {
            if (!m_Dictionary.ContainsKey(obj))
                return;

            Destroy(m_Dictionary[obj].gameObject);
            m_Dictionary.Remove(obj);
        }
    }
}
