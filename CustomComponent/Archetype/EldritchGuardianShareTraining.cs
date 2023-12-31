﻿using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.Blueprints.Classes.Selection;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class EldritchGuardianShareTraining : UnitFactComponentDelegate, IPartyHandler, IGlobalSubscriber, ISubscriber
    {

        // Token: 0x0600E0D8 RID: 57560 RVA: 0x0039C775 File Offset: 0x0039A975
        public override void OnActivate()
        {
            TryAddAll();
        }

        // Token: 0x0600E0D9 RID: 57561 RVA: 0x0039C77D File Offset: 0x0039A97D
        public override void OnDeactivate()
        {
            TryRemoveAll();
        }

        // Token: 0x0600E0DA RID: 57562 RVA: 0x0039C785 File Offset: 0x0039A985
        public override void OnTurnOn()
        {
            base.OnTurnOn();
            TryAddAll();
        }

        private void TryAddAll()
        {
            foreach (var feat in Owner.Descriptor.Progression.Features)
            {
                if (feat.Blueprint.HasGroup(FeatureGroup.CombatFeat))
                {
                    TryAdd(feat.Blueprint, feat.Param);
                }
            }
        }
        // Token: 0x0600E0DB RID: 57563 RVA: 0x0039C794 File Offset: 0x0039A994
        private void TryAdd(BlueprintFeature feat, FeatureParam param)
        {
            foreach (EntityPartRef<UnitEntityData, UnitPartPet> entityPartRef in Owner.Pets)
            {
                UnitEntityData entity = entityPartRef.Entity;
                UnitPartPet unitPartPet = entity?.Get<UnitPartPet>();
                if (unitPartPet != null && !entityPartRef.Entity.Descriptor.Progression.Features.HasFact(feat))
                {
                    entityPartRef.Entity.Descriptor.AddFact(feat, null, param);
                }
            }
        }

        private void TryRemoveAll()
        {
            foreach (var feat in Owner.Descriptor.Progression.Features)
            {
                if (feat.Blueprint.HasGroup(FeatureGroup.CombatFeat))
                {
                    var para = feat.Blueprint as BlueprintParametrizedFeature;
                    if (para?.ParameterType == FeatureParameterType.WeaponCategory) { continue; }
                    TryRemove(feat.Blueprint);
                }
            }
        }
        // Token: 0x0600E0DC RID: 57564 RVA: 0x0039C84C File Offset: 0x0039AA4C
        private void TryRemove(BlueprintFeature feat)
        {
            foreach (EntityPartRef<UnitEntityData, UnitPartPet> entityPartRef in Owner.Pets)
            {
                UnitEntityData entity = entityPartRef.Entity;
                UnitPartPet unitPartPet = entity?.Get<UnitPartPet>();
                if (unitPartPet != null && entityPartRef.Entity.Descriptor.Progression.Features.HasFact(feat))
                {
                    entityPartRef.Entity.Descriptor.Progression.Features.RemoveFact(feat);
                }
            }
        }

        // Token: 0x0600E0DD RID: 57565 RVA: 0x0039C900 File Offset: 0x0039AB00
        public void HandleAddCompanion(UnitEntityData unit)
        {
            if (unit.IsPet && unit.Master == Owner)
            {
                TryAddAll();
            }
        }

        // Token: 0x0600E0DE RID: 57566 RVA: 0x0039C928 File Offset: 0x0039AB28
        public void HandleCompanionActivated(UnitEntityData unit)
        {
        }

        // Token: 0x0600E0DF RID: 57567 RVA: 0x0039C92A File Offset: 0x0039AB2A
        public void HandleCompanionRemoved(UnitEntityData unit, bool stayInGame)
        {
        }

        // Token: 0x0600E0E0 RID: 57568 RVA: 0x0039C92C File Offset: 0x0039AB2C
        public void HandleCapitalModeChanged()
        {
        }
    }
}
