﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Grapple;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.MythicFeat
{
    internal class GiganticAssault
    {
        private static readonly string FeatName = "GiganticAssault";
        public static readonly string FeatGuid = "{D47DC15C-3A96-4358-A652-DB9E632009A7}";

        private static readonly string DisplayName = "GiganticAssault.Name";
        private static readonly string Description = "GiganticAssault.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.ArmyChargeAbilityFeature.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(MammothRider.CombinedMightGuid)
                    .AddComponent<GiganticLimit>()
                    .AddBuffExtraEffects(checkedBuff: BuffRefs.MountedBuff.ToString(), extraEffectBuff: BuffRefs.ChargeBuff.ToString())
                    .Configure();
        }

        public static void Configure2()
        {
            BuffConfigurator.For(BuffRefs.ChargeBuff)
                    .AddCMBBonus(value: 2)
                    .Configure();
        }
    }
}

