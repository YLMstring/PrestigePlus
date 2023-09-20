﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class AerialAssault
    {
        private static readonly string FeatName = "AerialAssault";
        private static readonly string FeatGuid = "{96E7DFF8-63D5-4846-920B-0D5C7E7DB823}";

        private static readonly string DisplayName = "AerialAssault.Name";
        private static readonly string Description = "AerialAssault.Description";

        private const string ReleaseAbility = "AerialAssault.ReleaseAbility";
        private static readonly string ReleaseAbilityGuid = "{F8C24B38-13B5-4708-BFA3-E3DC34C84461}";

        private const string Stylebuff = "AerialAssault.Stylebuff";  
        private static readonly string StylebuffGuid = "{E78853A3-7B2C-40B6-831F-824B1423F7F6}";
        public static void Configure()
        {
            var icon = FeatureRefs.ArmyChargeAbilityFeature.Reference.Get().Icon;

            BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(2))
              .Configure();

            var ability = AbilityConfigurator.New(ReleaseAbility, ReleaseAbilityGuid)
                .CopyFrom(
                AbilityRefs.ChargeAbility,
                typeof(AbilityRequirementHasCondition),
                typeof(AbilityCasterHasNoFacts),
                typeof(AbilityIsFullRoundInTurnBased),
                typeof(AbilityRequirementCanMove))
                .AddComponent<TryAboveAttack>()
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.DoubleMove)
                .SetType(AbilityType.Physical)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}