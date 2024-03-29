﻿using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class DestructiveSpellComp : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>
    {
        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            var spell = evt.AbilityData.Blueprint;
            if (spell.Type == AbilityType.Spell && !spell.IsFullRoundAction && HasDamage(spell) && spell.GetComponent<SpellListComponent>() != null)
            {
                evt.AddBonusDC(4);
            }
        }

        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }

        public static bool HasDamage(BlueprintAbility spell)
        {
            if (spell.m_DisplayName == AbilityRefs.Implosion.Reference.Get().m_DisplayName)
            {
                return true;
            }
            
            var stuff = spell.AbilityAndVariants()
                    .SelectMany(s => s.AbilityAndStickyTouch());

            return stuff.Any(s => s.FlattenAllActions()
                        .OfType<ContextActionDealDamage>()?
                        .Any(a => a.m_Type == ContextActionDealDamage.Type.Damage) ?? false)
                    || stuff.Any(s => s.FlattenAllActions()
                            .OfType<ContextActionSpawnAreaEffect>()
                            .Where(a => a.AreaEffect.FlattenAllActions()
                                .OfType<ContextActionDealDamage>()
                                .Any(a => a.m_Type == ContextActionDealDamage.Type.Damage))
                            .Any())
                    || stuff.Any(s => s.FlattenAllActions()
                            .OfType<ContextActionCastSpell>()
                            .Where(a => HasDamage(a.m_Spell) == true)
                            .Any());
        }
        void IRulebookHandler<RuleCalculateDamage>.OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            var ability = evt.Reason?.Ability;
            if (ability == null) return;
            var spell = ability.Blueprint;
            if (spell.Type == AbilityType.Spell && !spell.IsFullRoundAction && HasDamage(spell))
            {
                foreach (BaseDamage baseDamage in evt.DamageBundle)
                {
                    if (!baseDamage.Precision)
                    {
                        int bonus = baseDamage.Dice.ModifiedValue.Rolls;
                        baseDamage.AddModifier(bonus, base.Fact);
                    }
                }
            }
        }

        void IRulebookHandler<RuleCalculateDamage>.OnEventDidTrigger(RuleCalculateDamage evt)
        {
            
        }
    }
}
