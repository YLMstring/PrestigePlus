﻿using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.Blueprints.JsonSystem;

namespace PrestigePlus.CustomComponent.Feat
{
    [TypeId("{25ECC6AB-7EAD-4C05-9AA9-059A539A8121}")]
    internal class GuidedHandWis : UnitFactComponentDelegate<GuidedHandWis.ComponentData>, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600C2EE RID: 49902 RVA: 0x0032DE98 File Offset: 0x0032C098
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (Data.cat.Count() == 0)
            {
                Data.cat = PrerequisiteDivineWeapon.GetFavoredWeapon(Owner);
            }
            if (Data.cat.Contains(evt.Weapon.Blueprint.Category))
            {
                evt.AttackBonusStat = StatType.Wisdom;
            }
        }

        // Token: 0x0600C2EF RID: 49903 RVA: 0x0032DF81 File Offset: 0x0032C181
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        public override void OnActivate()
        {
            Data.cat = PrerequisiteDivineWeapon.GetFavoredWeapon(Owner);
        }

        public override void OnDeactivate()
        {
            Data.cat.Clear();
        }
        public class ComponentData
        {
            public List<WeaponCategory> cat = new() { };
        }
    }
}

