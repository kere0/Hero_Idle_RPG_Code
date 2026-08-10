using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThunderStrike : BaseSkill
{
    private Coroutine _skillCoroutine;
    public ThunderStrike(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        if (isAuto == true)
        {
            if (skillContext.Target == null)
            {
                Debug.Log("타겟이 없습니다");
                return false;
            }
        }
        return true;
    }
    public override void Execute(SkillContext skillContext, int attack, int value, bool isCritical)
    {
        if (_skillCoroutine != null)
        {
            GameManager.Instance.StopCoroutine(_skillCoroutine);
            _skillCoroutine = null;
        }
        Vector3 pos = skillContext.Caster.Position;
        _skillCoroutine = GameManager.Instance.StartCoroutine(ThunderStrikeCoroutine(pos, attack, value, isCritical));
    }

    private IEnumerator ThunderStrikeCoroutine(Vector3 pos, int attack, int skillValue, bool isCritical)
    {
        int count = 10;
        Vector3 castPos = pos;
        while (count > 0)
        {
            count--;
            Vector3 targetPos = castPos;
            targetPos.x = castPos.x + Random.Range(1f, 15f);
            ThunderStrikeObject go = Managers.Resource.Instantiate("Thunder", pooling: true).GetComponent<ThunderStrikeObject>();
            int totalDamage = attack * skillValue / 100;
            go.Init(targetPos, totalDamage, isCritical);
            Debug.Log(totalDamage + "데미지");
            Debug.Log("번개~~~");
            yield return new WaitForSeconds(0.15f);
        }
        _skillCoroutine = null;
    }
    public override void Reset()
    {
        if (_skillCoroutine != null)
        {
            GameManager.Instance.StopCoroutine(_skillCoroutine);
            _skillCoroutine = null;
        }
    }
}
