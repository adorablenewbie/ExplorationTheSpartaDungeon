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

    public void AddPermanentBuff(BuffData buffData) // ���߿� �����͸� �� �� ���������ΰ� �Ͻ����ΰ� �Ǻ��� ���⼭ �غ���. (equiptool, UIInventory���� ���� ���ǽ� ����)
    {
        if (buffData == null) return;                       //���� ����Ʈ�� �ִ� ������ ù��° ����Ʈ�� ����Ǵ� ������ �ֱ⿡ ���ľ���
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
