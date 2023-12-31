﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using PrestigePlus.Grapple;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.GrappleFeat
{
    internal class ThroatSlicer
    {
        private static readonly string FeatName = "ThroatSlicer";
        public static readonly string FeatGuid = "{8EA629AA-AB27-4D99-B722-195E71A178F6}";

        private static readonly string DisplayName = "ThroatSlicer.Name";
        private static readonly string Description = "ThroatSlicer.Description";

        private const string ThroatSlicerAbility = "ThroatSlicer.ThroatSlicerAbility";
        private static readonly string ThroatSlicerAbilityGuid = "{D3E807DF-3C92-4621-BF34-CDC1E426C509}";

        private const string ThroatSlicerbuff = "ThroatSlicer.ThroatSlicerbuff";
        public static readonly string ThroatSlicerbuffGuid = "{2D59E73D-3E3A-448A-9ECB-0EAB89AD4009}";
        public static void Configure()
        {
            var icon = FeatureRefs.MadDogThroatCutter.Reference.Get().Icon;

            var BuffThroatSlicer = BuffConfigurator.New(ThroatSlicerbuff, ThroatSlicerbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              //.AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.CoupDeGraceAbility.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard; a.CancelFullRound = true; })
              .Configure();

            var sword = ActionsBuilder.New()
                .ApplyBuff(BuffThroatSlicer, ContextDuration.Fixed(1), toCaster: true)
                .CastSpell(AbilityRefs.CoupDeGraceAbility.ToString())
                .Build();

            var abilityTrick = AbilityConfigurator.New(ThroatSlicerAbility, ThroatSlicerAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                //.SetBuff(BuffThroatSlicer)
                .AddAbilityEffectRunAction(sword)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace)
                .SetRange(AbilityRange.Weapon)
                .AddAbilityRequirementHasItemInHands(type: AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
                    .AddFacts(new() { abilityTrick })
                    //.AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.CoupDeGraceAbility.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard; a.CancelFullRound = true; })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
