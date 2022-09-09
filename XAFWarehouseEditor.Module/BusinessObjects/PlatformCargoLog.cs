using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace XAFWarehouseEditor.Module.BusinessObjects
{
    /// <summary>
    /// Entity for log movement of cargo on platforms with relationships:
    /// Platform (one) -> PlatformCargoLog (many)
    /// </summary>
    [XafDisplayName("Cargo on the platform")] // Attribute for display name in application
    [DefaultClassOptions]
    public class PlatformCargoLog : BaseObject
    {
        public PlatformCargoLog(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        /// <summary>
        /// Relationship with PlatformCargoLog:
        /// Platform (one) -> PlatformCargoLog (many) 
        /// </summary>
        Platform _AssignedToPlatform;
        [Association("Platform-PlatformCargoLog")]
        [XafDisplayName("Platform"), ToolTip("Assigned to platform")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [RuleRequiredField] // It can't be empty or null
        public Platform AssignedToPlatform
        {
            get => _AssignedToPlatform;
            set => SetPropertyValue(nameof(AssignedToPlatform), ref _AssignedToPlatform, value);
        }

        uint _Cargo;
        [XafDisplayName("Cargo (tons)"), ToolTip("Cargo on the platform")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [RuleRequiredField] // It can't be empty or null
        public uint Cargo
        {
            get => _Cargo;
            set => SetPropertyValue(nameof(Cargo), ref _Cargo, value);
        }

        /// <summary>
        /// Date and time when cargo arrived on platform
        /// </summary>
        DateTime _CreateTime;
        [XafDisplayName("Date and time"), ToolTip("Date and time cargo arrived on platform")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [RuleRequiredField] // It can't be empty or null
        public DateTime CreateTime
        {
            get => _CreateTime;
            set => SetPropertyValue(nameof(CreateTime), ref _CreateTime, value);
        }

        /// <summary>
        /// Date and time when cargo left platform
        /// This field can be null when cargo arrived on platform
        /// </summary>
        DateTime? _DeleteTime;
        [XafDisplayName("Date and time"), ToolTip("Date and time cargo left platform")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        public DateTime? DeleteTime
        {
            get => _DeleteTime;
            set => SetPropertyValue(nameof(DeleteTime), ref _DeleteTime, value);
        }

        /// <summary>
        /// To log user actions for PlatformCargoLog entity
        /// </summary>
        private XPCollection<AuditDataItemPersistent> auditTrail;
        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)] // AllowAdd, AllowRemove - to deny (add and remove) user to change collection in UI
        public XPCollection<AuditDataItemPersistent> AuditTrail
        {
            get
            {
                if (auditTrail == null)
                {
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditTrail;
            }
        }
    }
}