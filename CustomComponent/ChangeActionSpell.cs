﻿using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;

namespace PrestigePlus.Modify
{
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintBuff), false)]
    [AllowMultipleComponents]
    internal class ChangeActionSpell : UnitFactComponentDelegate
    {
        public BlueprintAbility Ability;
        public CommandType Type;
        public bool CancelFullRound = false;

        // Token: 0x0600C12A RID: 49450 RVA: 0x00325827 File Offset: 0x00323A27
        public override void OnTurnOn()
        {
            AddEntry(Ability);
            var list = Ability?.GetComponent<AbilityVariants>()?.Variants;
            if (list?.Length > 0)
            {
                foreach (var variant in list)
                {
                    AddEntry(variant);
                }
            }
            
        }

        // Token: 0x0600C12B RID: 49451 RVA: 0x0032584B File Offset: 0x00323A4B
        public override void OnTurnOff()
        {
            base.Owner.Ensure<UnitPartAbilityModifiers>().RemoveEntry(base.Fact);
        }

        private void AddEntry(BlueprintAbility ability)
        {
            if (ability == null) { return; }
            var entry = new UnitPartAbilityModifiers.ActionEntry(base.Fact, Type, ability);
            if (CancelFullRound)
            {
                entry.RequireFullRound = false;
            }
            base.Owner.Ensure<UnitPartAbilityModifiers>().AddEntry(entry);
        }
    }
}
