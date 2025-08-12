using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuffManager : MonoBehaviour
{
    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = PlayerManager.Instance.Player.controller;
        condition = PlayerManager.Instance.Player.condition;
    }
    public void MakePermanentBuffUI(GameObject inputPrefab)
    {
        GameObject buffUI = Instantiate(inputPrefab, transform);
        buffUI.GetComponent<UIBuff>().isPermanent = true;

    }
    public void MakeTempBuffUI(GameObject inputPrefab, float inputDuration)
    {
        GameObject buffUI = Instantiate(inputPrefab, transform);
        buffUI.GetComponent<UIBuff>().remainDuration = inputDuration;
        buffUI.GetComponent<UIBuff>().duration = inputDuration;
    }

    public void AddPermanentBuff(BuffData buffData) // 나중에 리팩터링 할 때 영구구적인가 일시적인가 판별도 여기서 해보자. (equiptool, UIInventory에서 각자 조건식 있음)
    {
        if (buffData == null) return;                       //다중 이펙트가 있는 버프는 첫번째 이펙트만 적용되는 문제가 있기에 고쳐야함
        if (buffData.effects[0].type == BuffType.SpeedUp)
        {
            controller.SpeedUp(buffData.effects[1].value);
        }
        else if (buffData.effects[0].type == BuffType.JumpUp)
        {
            controller.JumpUp(buffData.effects[1].value);
        }
        else if (buffData.effects[0].type == BuffType.DoubleJump)
        {
           controller.onDoubleJump = buffData.effects[0].isOn;
        }
        else if (buffData.effects[0].type == BuffType.Invincible)
        {
            condition.uiCondition.health.SetConditionInvincible();
        }
        MakePermanentBuffUI(buffData.buffUI);
    }

    public void AddTemporaryBuff(BuffData buffData, float inputDuration)
    {
        if (buffData == null) return;
        if (buffData.effects[0].type == BuffType.SpeedUp)
        {
            StartCoroutine(controller.SpeedUpCoroutine(buffData.effects[0].value, inputDuration));
        }
        else if (buffData.effects[0].type == BuffType.JumpUp)
        {
            StartCoroutine(controller.JumpUpCoroutine(buffData.effects[0].value, inputDuration));
        }
        else if (buffData.effects[0].type == BuffType.DoubleJump)
        {
            StartCoroutine(controller.DoubleJumpCoroutine(inputDuration));
        }
        else if (buffData.effects[0].type == BuffType.Invincible)
        {
            StartCoroutine(condition.uiCondition.health.SetConditionInvincibleCoroutine(inputDuration));
        }
        MakeTempBuffUI(buffData.buffUI, inputDuration);
    }

}
