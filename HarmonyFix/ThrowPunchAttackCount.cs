﻿using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Settings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(RuleCalculateAttacksCount), nameof(RuleCalculateAttacksCount.CalculatePenalizedAttacksCount))]
    internal class ThrowPunchAttackCount
    {
        static bool Prefix(ref RuleCalculateAttacksCount __instance, ref int __result)
        {
            try
            {
                var caster = __instance.Initiator;
                if (caster.HasFact(Buff) && caster.GetThreatHandMelee()?.Weapon?.Blueprint.Category == Kingmaker.Enums.WeaponCategory.UnarmedStrike)
                {
                    int num = 0;
                    foreach (ClassData classData2 in caster.Descriptor.Progression.Classes)
                    {
                        if (classData2.Spellbook != null)
                        {
                            num += Math.Max(classData2.Level + classData2.Spellbook.CasterLevelModifier, 0);
                        }
                    }
                    int num2 = Math.Max(0, num / 5 - ((num % 5 == 0) ? 1 : 0));
                    if (num2 > 3 && caster.Get<UnitPartCompanion>() == null)
                    {
                        num2 = 3;
                    }
                    __result = num2;
                    return false;
                }
                return true;
            }
            catch (Exception ex) { Logger.Error("Failed to ThrowPunchAttackCount.", ex); return true; }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>(MageHandTrick.ThrowPunchbuffGuid);
        //private static BlueprintBuffReference Buff2 = BlueprintTool.GetRef<BlueprintBuffReference>(MageHandTrick.MageHandMythicFeatGuid);
    }
}
