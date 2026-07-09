using UnityEngine;
[CreateAssetMenu(fileName = "RelentlessAttacksEffect", menuName = "Special Effects/Relentless Attacks Effect")]
public class RelentlessAttacksEffect : SpecialEffect
{
    // every x number of turns, gain y more attacks
    public int maxPhysicalAttacks = 10; // ex: cap at 10 attacks
    public int maxMagicalAttacks = 10; // ex: cap at 10 attacks
    public override void ApplyEffect(SpecialEffectContext context)
    {
        if (context.turnNumber % turnThreshold == 0) // every x turns
        {
            if (context.enemy.EnemyStats.physicalAttackAmount < maxPhysicalAttacks && context.enemy.EnemyStats.physicalAttackAmount >= 1)
            {
                context.enemy.EnemyStats.physicalAttackAmount += 1; // gain 1 more attack
            } // cap at 10 attackscontext.enemy.EnemyStats.physicalAttackAmount += 1; // gain 1 more attack
            
            if (context.enemy.EnemyStats.magicalAttackAmount < maxMagicalAttacks && context.enemy.EnemyStats.magicalAttackAmount >= 1)
            {
                context.enemy.EnemyStats.magicalAttackAmount += 1; // gain 1 more attack
            } // cap at 10 attackscontext.enemy.EnemyStats.magicalAttackAmount += 1; // gain 1 more attack
        }
    }
}
