﻿using BlueprintCore.Blueprints.References;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class TruthSookerMagic : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleApplyMetamagic>, IRulebookHandler<RuleApplyMetamagic>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleApplyMetamagic>.OnEventAboutToTrigger(RuleApplyMetamagic evt)
        {

        }

        void IRulebookHandler<RuleApplyMetamagic>.OnEventDidTrigger(RuleApplyMetamagic evt)
        {
            var spellbook = Owner.GetSpellbook(CharacterClassRefs.SorcererClass.Reference);
            //var spelllist = SpellListRefs.ClericSpellList.Reference.Get();
            if (spellbook != null && evt.Spellbook == spellbook && SpellList.Contains(evt.Spell))
            {
                evt.Result.SpellLevelCost -= evt.AppliedMetamagics.Count;
                if (evt.BaseLevel + evt.Result.SpellLevelCost < 0)
                {
                    evt.Result.SpellLevelCost = -evt.BaseLevel;
                }
            }
        }

        public override void OnActivate()
        {
            var list = FeatureSelectionRefs.SeekerBloodlineSelection.Reference.Get().m_AllFeatures;
            foreach (var blood in list)
            {
                if (!Owner.HasFact(blood)) { continue; }
                var lines = blood.Get() as BlueprintProgression;
                var comps = lines?.LevelEntries;
                if (comps?.Any() != true) { continue; }
                foreach (var item in comps)
                {
                    if (item.m_Features?.Any() != true) { continue; }
                    foreach (var feature in item.m_Features)
                    {
                        var spells = feature.Get().GetComponents<AddKnownSpell>();
                        if (spells?.Any() != true) { continue; }
                        foreach (var spell in spells)
                        {
                            if (spell.m_Spell == null) { continue; }
                            SpellList.Add(spell.m_Spell);
                        }
                    }
                }
            }
        }

        public override void OnDeactivate()
        {
            SpellList = new() { };
        }

        private List<BlueprintAbility> SpellList = new() { };
    }
}
