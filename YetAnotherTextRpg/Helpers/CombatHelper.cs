using System;
using System.Collections.Generic;
using System.Text;
using YetAnotherTextRpg.Models;

namespace YetAnotherTextRpg.Helpers
{
    public enum  AttackResultCode
    {
        Missed,
        ArmorBlocked,
        DidDamage
    }

    public class AttackResult
    {
        public AttackResultCode Result { get; set; }
        public int DamageDone { get; set; }
    }

    public static class CombatHelper
    {
        public static AttackResult ResolveAttack(CombatProfile attacker, CombatProfile defender)
        {
            var attackRoll = DiceHelper.RollD6() + DiceHelper.RollD6() + attacker.Attack;

            if (attackRoll < defender.Defense)
                return new AttackResult { Result = AttackResultCode.Missed, DamageDone = 0 };

            var attackDamage = DiceHelper.RollD6() + DiceHelper.RollD6() + attacker.Strength;
            var damageDone = attackDamage - defender.Armor;

            if (damageDone <= 0)
                return new AttackResult { Result = AttackResultCode.ArmorBlocked, DamageDone = 0 };
            else
                return new AttackResult { Result = AttackResultCode.DidDamage, DamageDone = damageDone };

        }
    }
}
