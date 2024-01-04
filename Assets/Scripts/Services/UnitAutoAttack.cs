using System.Linq;
using RTSEngine.Attack;
using RTSEngine.Entities;
using RTSEngine.Game;
using RTSEngine.UnitExtension;
using UnityEngine;

/// <summary>
/// 一个游戏的Service，用于使单位自动寻找并攻击敌人。
/// 必须将该组件的GameObject放在GameManager的子物体中。
/// </summary>
public class UnitAutoAttack : MonoBehaviour, IPostRunGameService
{
    /// <summary>
    /// 初始化。被IPostRunGameService需求。
    /// </summary>
    /// <param name="gameMgr">游戏管理器。</param>
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
            // 不能攻击，或者不是本阵营
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
    // 创建单位相关。
    protected IGameManager gameMgr { private set; get; }
    protected IAttackManager attackMgr { private set; get; }
    protected IUnitManager unitMgr { private set; get; }

}