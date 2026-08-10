using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public Queue<CombatEvent> queue = new Queue<CombatEvent>();
    private readonly int _maxProcessCountPerFrame = 200;
    void Update()
    {
        int _processedCount = 0;
        while (queue.Count > 0 && _processedCount < _maxProcessCountPerFrame)
        {
            CombatEvent combatEvent = queue.Dequeue();
            _processedCount++;
            if (combatEvent.Damage != 0)
            {
                DamageText damageText = Managers.Resource.Instantiate("DamageText", pooling : true).GetComponent<DamageText>();
                Vector3 pos = combatEvent.Receiver.Collider.bounds.center;
                pos.y = combatEvent.Receiver.Collider.bounds.max.y + 1.5f;
                damageText.SetText(pos, combatEvent.Damage, combatEvent.IsCritical, combatEvent.Receiver.CreatureType);
                combatEvent.Receiver.TakeDamage(combatEvent.Damage);
                if (combatEvent.DamageType == DamageType.Normal)
                {
                    GameObject go = Managers.Resource.Instantiate("HitEffect", pooling: true);
                    go.transform.position = combatEvent.Receiver.EffectPos.position;
                }
            }
        }
    }
    public void AddCombatEvent(CombatEvent combatEvent)
    {
        queue.Enqueue(combatEvent);
    }
}
