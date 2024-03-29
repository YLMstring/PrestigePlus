﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Properties;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Root;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using PrestigePlus.Modify;
using BlueprintCore.Conditions.Builder.BasicEx;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using PrestigePlus.CustomComponent.PrestigeClass;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.CustomComponent.BasePrestigeEnhance;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class EnchantingCourtesan
    {
        private const string ArchetypeName = "EnchantingCourtesan";
        public static readonly string ArchetypeGuid = "{5EFD5A02-D3F0-4A1E-9E3A-D9C1368DB3FA}";
        internal const string ArchetypeDisplayName = "EnchantingCourtesan.Name";
        private const string ArchetypeDescription = "EnchantingCourtesan.Description";

        private const string ClassProgressName = "EnchantingCourtesanPrestige";
        private static readonly string ClassProgressGuid = "{9B060568-C3B7-4970-BDC3-69B934F8A2EA}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, EnchantingTouchFeature(), ChooseGoodEvilFeat(), SpellbookReplace.spellupgradeGuid)
                .AddToLevelEntry(2, SeductiveIntuitionFeature())
                .AddToLevelEntry(3, HiddenSpellFeature())
                .AddToLevelEntry(4, SeductiveIntuitionGuid)
                .AddToLevelEntry(5)
                .AddToLevelEntry(6, SeductiveIntuitionGuid, HiddenSpellGuid)
                .AddToLevelEntry(7)
                .AddToLevelEntry(8, SeductiveIntuitionGuid, OverwhelmingConfigure())
                .AddToLevelEntry(9, HiddenSpellGuid)
                .AddToLevelEntry(10, SeductiveIntuitionGuid, EcstasyConfigure())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] {  }))
                ///.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SeekerArrowGuid, PhaseArrowGuid, HailArrowGuid, DeathArrowGuid }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();

            DeludingTouchFeature();

            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(6)
                .SetHitDie(DiceType.D6)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(StatProgressionRefs.BABLow.ToString())
                .SetFortitudeSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetReflexSave(StatProgressionRefs.SavesPrestigeLow.ToString())
                .SetWillSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillMobility, StatType.SkillThievery, StatType.SkillStealth, StatType.SkillKnowledgeWorld, StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteParametrizedSpellSchoolFeature(ParametrizedFeatureRefs.SpellFocus.ToString(), SpellSchool.Divination, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteParametrizedSpellSchoolFeature(ParametrizedFeatureRefs.SpellFocus.ToString(), SpellSchool.Enchantment, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteStatValue(StatType.SkillPerception, 5)
                .AddPrerequisiteStatValue(StatType.SkillPersuasion, 5)
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeArcana, 2)
                .AddPrerequisiteStatValue(StatType.SkillUseMagicDevice, 2)
                .AddComponent<PrerequisiteSpellLevel>(c => { c.RequiredSpellLevel = 2; })
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string MasterSpy = "EnchantingCourtesan.MasterSpy";
        public static readonly string MasterSpyGuid = "{C07AE528-7C26-40D9-9573-D0C351760E4C}";

        internal const string MasterSpyDisplayName = "EnchantingCourtesanMasterSpy.Name";
        private const string MasterSpyDescription = "EnchantingCourtesanMasterSpy.Description";

        public static BlueprintProgression MasterSpyFeat()
        {
            var icon = FeatureRefs.MasterSpyFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(MasterSpy, MasterSpyGuid)
              .SetDisplayName(MasterSpyDisplayName)
              .SetDescription(MasterSpyDescription)
              .SetIcon(icon)
              //.AddPrerequisiteFeaturesFromList(new() { FeatureRefs.Deceitful.ToString(), FeatureRefs.IronWill.ToString() }, 2)
              .AddPrerequisiteFeature(FeatureRefs.Deceitful.ToString())
              .AddPrerequisiteFeature(FeatureRefs.IronWill.ToString())
              .SetIsClassFeature(true)
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(1, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(2, MaskAlignmentFeature())
              .AddToLevelEntry(4, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(5, FeatureRefs.SlipperyMind.ToString())
              .AddToLevelEntry(7, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(9, HiddenMindFeature())
              .AddToLevelEntry(10, FeatureRefs.SneakAttack.ToString())
              .Configure();
        }

        private const string ChooseGoodEvil = "EnchantingCourtesan.ChooseGoodEvil";
        private static readonly string ChooseGoodEvilGuid = "{24BD6A83-4AE4-46C6-8C53-C0CFC4D29C91}";
        public static BlueprintFeatureSelection ChooseGoodEvilFeat()
        {
            var icon = FeatureRefs.MasterSpyFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(ChooseGoodEvil, ChooseGoodEvilGuid)
              .SetDisplayName(MasterSpyDisplayName)
              .SetDescription(MasterSpyDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(MasterSpyFeat())
              .AddToAllFeatures(MaskAlignmentGuid)
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string SeductiveIntuition = "EnchantingCourtesanSeductiveIntuition";
        private static readonly string SeductiveIntuitionGuid = "{408692DA-8EFD-4A49-BA7E-51578894B5E5}";

        internal const string SeductiveIntuitionDisplayName = "EnchantingCourtesanSeductiveIntuition.Name";
        private const string SeductiveIntuitionDescription = "EnchantingCourtesanSeductiveIntuition.Description";
        public static BlueprintFeature SeductiveIntuitionFeature()
        {
            var icon = FeatureRefs.CharmDomainGreaterFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(SeductiveIntuition, SeductiveIntuitionGuid)
              .SetDisplayName(SeductiveIntuitionDisplayName)
              .SetDescription(SeductiveIntuitionDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.CheckBluff, ContextValues.Rank(), ModifierDescriptor.Competence)
              .AddContextStatBonus(StatType.CheckDiplomacy, ContextValues.Rank(), ModifierDescriptor.Competence)
              .AddContextStatBonus(StatType.SkillPerception, ContextValues.Rank(), ModifierDescriptor.Competence)
              .AddContextStatBonus(StatType.SkillThievery, ContextValues.Rank(), ModifierDescriptor.Competence)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(SeductiveIntuitionGuid))
              .SetRanks(10)
              .Configure();
        }

        private const string HiddenSpell = "EnchantingCourtesanHiddenSpell";
        private static readonly string HiddenSpellGuid = "{89EFD710-16C2-43E3-B075-BCEAA6BB6C23}";

        internal const string HiddenSpellDisplayName = "EnchantingCourtesanHiddenSpell.Name";
        private const string HiddenSpellDescription = "EnchantingCourtesanHiddenSpell.Description";
        public static BlueprintFeature HiddenSpellFeature()
        {
            var icon = FeatureRefs.SelectiveSpellFeat.Reference.Get().Icon;
            return FeatureConfigurator.New(HiddenSpell, HiddenSpellGuid)
              .SetDisplayName(HiddenSpellDisplayName)
              .SetDescription(HiddenSpellDescription)
              .SetIcon(icon)
              .AddComponent<HiddenSpellComp>()
              .SetRanks(10)
              .Configure();
        }

        private const string HiddenMind = "EnchantingCourtesanHiddenMind";
        public static readonly string HiddenMindGuid = "{01661653-5877-4A6B-B4DF-EE8D9C26AD47}";

        internal const string HiddenMindDisplayName = "EnchantingCourtesanHiddenMind.Name";
        private const string HiddenMindDescription = "EnchantingCourtesanHiddenMind.Description";
        public static BlueprintFeature HiddenMindFeature()
        {
            var icon = AbilityRefs.MindBlank.Reference.Get().Icon;
            return FeatureConfigurator.New(HiddenMind, HiddenMindGuid)
              .SetDisplayName(HiddenMindDisplayName)
              .SetDescription(HiddenMindDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(8, modifierDescriptor: ModifierDescriptor.Resistance, spellDescriptor: SpellDescriptor.MindAffecting)
              .AddFacts(new() { FeatureRefs.DivinationImmunityFeature.ToString() })
              .Configure();
        }

        private const string EnchantingTouch = "EnchantingCourtesanEnchantingTouch";
        public static readonly string EnchantingTouchGuid = "{EBB40FD1-AB07-4894-B691-4E85CB90DC96}";

        internal const string EnchantingTouchDisplayName = "EnchantingCourtesanEnchantingTouch.Name";
        private const string EnchantingTouchDescription = "EnchantingCourtesanEnchantingTouch.Description";
        public static BlueprintFeature EnchantingTouchFeature()
        {
            var icon = AbilityRefs.HolyWhisper.Reference.Get().Icon;
            return FeatureConfigurator.New(EnchantingTouch, EnchantingTouchGuid)
              .SetDisplayName(EnchantingTouchDisplayName)
              .SetDescription(EnchantingTouchDescription)
              .SetIcon(icon)
              .AddComponent<EnchantingTouchComp>()
              .Configure();
        }

        private const string DeludingTouch = "EnchantingCourtesanDeludingTouch";
        public static readonly string DeludingTouchGuid = "{604769DB-A407-4F43-87E9-066B66FAB9F7}";

        internal const string DeludingTouchDisplayName = "EnchantingCourtesanDeludingTouch.Name";
        private const string DeludingTouchDescription = "EnchantingCourtesanDeludingTouch.Description";
        public static BlueprintFeature DeludingTouchFeature()
        {
            var icon = AbilityRefs.HolyWhisper.Reference.Get().Icon;
            return FeatureConfigurator.New(DeludingTouch, DeludingTouchGuid, FeatureGroup.MythicAbility)
              .SetDisplayName(DeludingTouchDisplayName)
              .SetDescription(DeludingTouchDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(EnchantingTouchGuid)
              .Configure();
        }

        private const string MaskAlignment = "EnchantingCourtesanMaskAlignment";
        private static readonly string MaskAlignmentGuid = "{EE2BF850-589D-49A1-98FB-2B11D0920860}";

        internal const string MaskAlignmentDisplayName = "EnchantingCourtesanMaskAlignment.Name";
        private const string MaskAlignmentDescription = "EnchantingCourtesanMaskAlignment.Description";
        public static BlueprintFeature MaskAlignmentFeature()
        {
            return FeatureConfigurator.New(MaskAlignment, MaskAlignmentGuid)
              .SetDisplayName(MaskAlignmentDisplayName)
              .SetDescription(MaskAlignmentDescription)
              .AddUndetectableAlignment()
              .Configure();
        }

        private static readonly string OverwhelmingName = "EnchantingCourtesanOverwhelming";
        public static readonly string OverwhelmingGuid = "{870DC3E9-3D71-450E-9013-4D9F795CFEFA}";

        private static readonly string OverwhelmingDisplayName = "EnchantingCourtesanOverwhelming.Name";
        private static readonly string OverwhelmingDescription = "EnchantingCourtesanOverwhelming.Description";

        private const string OverwhelmingBuff = "Overwhelming.OverwhelmingBuff";
        public static readonly string OverwhelmingBuffGuid = "{F7C9B2BF-864C-4916-9109-56762E012D2D}";
        public static BlueprintFeature OverwhelmingConfigure()
        {
            var icon = AbilityRefs.HealCast.Reference.Get().Icon;

            BuffConfigurator.New(OverwhelmingBuff, OverwhelmingBuffGuid)
                .SetDisplayName(OverwhelmingDisplayName)
                .SetDescription(OverwhelmingDescription)
                .SetIcon(icon)
                .AddModifyD20(ActionsBuilder.New().RemoveSelf().Build(), rule: RuleType.SavingThrow, rollsAmount: 1)
                .Configure();

            return FeatureConfigurator.New(OverwhelmingName, OverwhelmingGuid)
                    .SetDisplayName(OverwhelmingDisplayName)
                    .SetDescription(OverwhelmingDescription)
                    .SetIcon(icon)
                    .AddComponent<OverwhelmingTouchComp>()
                    .Configure();
        }

        private static readonly string EcstasyName = "EnchantingCourtesanEcstasy";
        public static readonly string EcstasyGuid = "{49AC4C66-95CD-45AA-B923-0CE563C2CFB5}";

        private static readonly string EcstasyDisplayName = "EnchantingCourtesanEcstasy.Name";
        private static readonly string EcstasyDescription = "EnchantingCourtesanEcstasy.Description";

        private static readonly string Ecstasy1DisplayName = "EnchantingCourtesanEcstasy1.Name";
        private static readonly string Ecstasy1Description = "EnchantingCourtesanEcstasy1.Description";

        private static readonly string Ecstasy2DisplayName = "EnchantingCourtesanEcstasy2.Name";
        private static readonly string Ecstasy2Description = "EnchantingCourtesanEcstasy2.Description";

        private const string EcstasyAbility = "EnchantingCourtesan.EcstasyAbility";
        private static readonly string EcstasyAbilityGuid = "{19DA9656-42E1-4572-98F5-722060B5ECB9}";

        private const string EcstasyBuff = "EnchantingCourtesan.EcstasyBuff";
        private static readonly string EcstasyBuffGuid = "{ED5C140F-9C2C-4119-8427-AFC5C7FD3675}";

        private const string EcstasyBuff2 = "EnchantingCourtesan.EcstasyBuff2";
        private static readonly string EcstasyBuff2Guid = "{1C21BC0A-7FE4-4630-81AB-F6F766E4A666}";
        public static BlueprintFeature EcstasyConfigure()
        {
            var icon = AbilityRefs.WavesOfEctasy.Reference.Get().Icon;

            var buff1 = BuffConfigurator.New(EcstasyBuff, EcstasyBuffGuid)
                .SetDisplayName(Ecstasy1DisplayName)
                .SetDescription(Ecstasy1Description)
                .SetIcon(icon)
                .AddComponent<SuppressBuffs>(c => { c.Descriptor = SpellDescriptor.NegativeEmotion; })
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .AddFacts(new() { BuffRefs.Stunned.ToString() })
                .Configure();

            var buff2 = BuffConfigurator.New(EcstasyBuff2, EcstasyBuff2Guid)
                .SetDisplayName(Ecstasy2DisplayName)
                .SetDescription(Ecstasy2Description)
                .SetIcon(icon)
                .AddComponent<SuppressBuffs>(c => { c.Descriptor = SpellDescriptor.NegativeEmotion; })
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .AddFacts(new() { BuffRefs.Staggered.ToString() })
                .Configure();

            var shoot = ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Fortitude, customDC: ContextValues.Rank(), useDCFromContextSavingThrow: true,
            onResult: ActionsBuilder.New()
                        .ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(buff1, ContextDuration.FixedDice(DiceType.D4)).Build(),
                                    succeed: ActionsBuilder.New().ApplyBuff(buff2, ContextDuration.FixedDice(DiceType.D4)).Build())
                        .Build())
                    .Build();

            var ability = AbilityConfigurator.New(EcstasyAbility, EcstasyAbilityGuid)
                .SetDisplayName(EcstasyDisplayName)
                .SetDescription(EcstasyDescription)
                .SetIcon(icon)
                .AddComponent<AbilityTargetMayPrecise>()
                .AllowTargeting(false, true, true, true)
                .AddAbilityDeliverTouch(false, null, ComponentMerge.Fail, ItemWeaponRefs.TouchItem.ToString())
                .AddAbilityEffectRunAction(shoot)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(stat: StatType.Wisdom).WithBonusValueProgression(20, false))
                .AddHideDCFromTooltip()
                .SetType(AbilityType.Extraordinary)
                .SetRange(AbilityRange.Touch)
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .Configure();

            return FeatureConfigurator.New(EcstasyName, EcstasyGuid)
                    .SetDisplayName(EcstasyDisplayName)
                    .SetDescription(EcstasyDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
