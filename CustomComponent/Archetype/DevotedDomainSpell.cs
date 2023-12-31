﻿using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.Utility;
using Kingmaker.Enums;
using PrestigePlus.CustomComponent.PrestigeClass;
using BlueprintCore.Utils;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class DevotedDomainSpell : UnitFactComponentDelegate<DevotedDomainSpell.ComponentData>, ILevelUpCompleteUIHandler, IGlobalSubscriber, ISubscriber
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
        {
            if (Data.list.Count != 0) return;
            var list = FeatureSelectionRefs.SecondDomainsSelection.Reference.Get().m_AllFeatures;
            foreach (var prog in list)
            {
                var pro = prog?.Get()?.ToReference<BlueprintProgressionReference>();
                if (pro != null && Owner.HasFact(pro))
                {
                    Logger.Info(pro.NameSafe());
                    GetSpell(pro);
                }
            }
        }
        public override void OnDeactivate()
        {
            foreach (var feat in Data.list)
            {
                Owner.Descriptor.Progression.Features.RemoveFact(feat);
            }
        }
        private void GetSpell(BlueprintProgression pro)
        {
            var comps = pro.LevelEntries[0]?.Features?.First()?.GetComponents<AddFeatureOnClassLevel>();
            if (comps == null) { return; }
            foreach (var comp in comps)
            {
                if (comp.Feature?.GetComponent<AddSpecialSpellList>() != null)
                {
                    Data.list.Add(comp.Feature);
                    Owner.Descriptor.Progression.Features.RemoveFact(pro);
                    Owner.Descriptor.Progression.Features.RemoveFact(pro.LevelEntries[0].Features.First());
                    Owner.Descriptor.AddFact(comp.Feature);
                    return;
                }
            }
        }

        public class ComponentData
        {
            public List<BlueprintFeature> list = new() { };
        }
    }
}
