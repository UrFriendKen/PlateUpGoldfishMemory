﻿using Kitchen;
using KitchenData;
using KitchenMods;
using MessagePack;
using Unity.Collections;
using Unity.Entities;

namespace KitchenGoldfishMemory.Views
{
    public class GroupMenuPhaseView : UpdatableObjectView<GroupMenuPhaseView.ViewData>
    {
        public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
        {
            EntityQuery Views;

            protected override void Initialise()
            {
                base.Initialise();
                Views = GetEntityQuery(typeof(CCustomerIndicator), typeof(CIndicator), typeof(CLinkedView));
            }

            protected override void OnUpdate()
            {
                using NativeArray<CIndicator> indicators = Views.ToComponentDataArray<CIndicator>(Allocator.Temp);
                using NativeArray<CLinkedView> views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                for (int i = 0; i < indicators.Length; i++)
                {
                    CIndicator indicator = indicators[i];
                    if (indicator.Source == default || !Require(indicator.Source, out CGroupMealPhase phase))
                        continue;

                    SendUpdate(views[i], new ViewData()
                    {
                        MenuPhase = phase.Phase,
                        IsRepeatPhase = Has<CGroupHasRepeatedOrder>(indicator.Source)
                    });
                }
            }
        }

        [MessagePackObject(false)]
        public class ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(0)] public MenuPhase MenuPhase;
            [Key(1)] public bool IsRepeatPhase;

            public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<GroupMenuPhaseView>();

            public bool IsChangedFrom(ViewData check)
            {
                return MenuPhase != check.MenuPhase ||
                    IsRepeatPhase != check.IsRepeatPhase;
            }
        }

        public MenuPhase MenuPhase { get; protected set; } = MenuPhase.Main;

        public bool IsRepeatPhase { get; protected set; } = false;

        protected override void UpdateData(ViewData data)
        {
            MenuPhase = data.MenuPhase;
            IsRepeatPhase = data.IsRepeatPhase;
        }
    }
}
