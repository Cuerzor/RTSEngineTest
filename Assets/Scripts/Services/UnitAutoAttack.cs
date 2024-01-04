using System.Linq;
using RTSEngine.Attack;
using RTSEngine.Entities;
using RTSEngine.Game;
using RTSEngine.UnitExtension;
using UnityEngine;

/// <summary>
/// һ����Ϸ��Service������ʹ��λ�Զ�Ѱ�Ҳ��������ˡ�
/// ���뽫�������GameObject����GameManager���������С�
/// </summary>
public class UnitAutoAttack : MonoBehaviour, IPostRunGameService
{
    /// <summary>
    /// ��ʼ������IPostRunGameService����
    /// </summary>
    /// <param name="gameMgr">��Ϸ��������</param>
    public void Init(IGameManager gameMgr)
    {
        this.gameMgr = gameMgr;
        attackMgr = gameMgr.GetService<IAttackManager>();
        unitMgr = gameMgr.GetService<IUnitManager>();
    }
    private void Update()
    {
        var enemy = unitMgr.AllUnits.FirstOrDefault(u => u.FactionID != gameMgr.LocalFactionSlotID);
        foreach (var unit in unitMgr.AllUnits)
        {
            // ���ܹ��������߲��Ǳ���Ӫ
            if (!unit.CanAttack || unit.FactionID != gameMgr.LocalFactionSlotID)
                continue;

            var attackComp = unit.FirstActiveAttackComponent;
            if (unit.IsIdle)
                unit.SetTargetFirst(new RTSEngine.EntityComponent.SetTargetInputData()
                {
                    includeMovement = true,
                    isMoveAttackRequest = true,
                    target = new RTSEngine.EntityComponent.TargetData<IEntity>()
                    {
                        instance = enemy
                    }
                });
        }
    }
    // ������λ��ء�
    protected IGameManager gameMgr { private set; get; }
    protected IAttackManager attackMgr { private set; get; }
    protected IUnitManager unitMgr { private set; get; }

}