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
    /// Entity for log connections between Picket and Platform with relationships:
    /// Picket (one) -> PlatformPicketsLog (many)
    /// Platform (one) -> PlatformPicketsLog (many)
    /// </summary>
    [XafDisplayName("Pickets of Platform")] // Attribute for display name in application
    [DefaultClassOptions]
    public class PlatformPicketsLog : BaseObject
    {
        public PlatformPicketsLog(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreateTime = DateTime.Now; // Initial data
        }

        /// <summary>
        /// Relationship with Warehouse:
        /// Picket (one) -> PlatformPicketsLog (many) 
        /// </summary>
        Picket _AssignedToPicket;
        [Association("Picket-PlatformPicketsLog")]
        [XafDisplayName("Picket"), ToolTip("Assigned to picket")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [RuleRequiredField] // It can't be empty or null
        public Picket AssignedToPicket
        {
            get => _AssignedToPicket;
            set
            {
                bool modified = SetPropertyValue(nameof(AssignedToPicket), ref _AssignedToPicket, value);
                if (!IsLoading && !IsSaving && value != null && modified && DeleteTime == null)
                {
                    AssignedToPicket.PicketStatus = Status.Used; // Set status of picket = used when create new PlatformPicketsLog entity
                }
            }
        }

        /// <summary>
        /// Relationship with Platform:
        /// Platform (one) -> PlatformPicketsLog (many) 
        /// </summary>
        Platform _AssignedToPlatform;
        [Association("Platform-PlatformPicketsLog")]
        [XafDisplayName("Platform"), ToolTip("Assigned to platform")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [RuleRequiredField] // It can't be empty or null
        public Platform AssignedToPlatform
        {
            get => _AssignedToPlatform;
            set => SetPropertyValue(nameof(AssignedToPlatform), ref _AssignedToPlatform, value);
        }

        /// <summary>
        /// Date and time for connection between Picket and Platform
        /// </summary>
        DateTime _CreateTime;
        [XafDisplayName("Date and time"), ToolTip("Date and time connect Platform-Picket")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [RuleRequiredField] // It can't be empty or null
        public DateTime CreateTime
        {
            get => _CreateTime;
            set => SetPropertyValue(nameof(CreateTime), ref _CreateTime, value);
        }

        /// <summary>
        /// Date and time for disconnection between Picket and Platform
        /// This field can be null when new connection created
        /// </summary>
        DateTime? _DeleteTime;
        [XafDisplayName("Date and time"), ToolTip("Date and time disconnect Platform-Picket")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        public DateTime? DeleteTime
        {
            get => _DeleteTime;
            set
            {
                bool modified = SetPropertyValue(nameof(DeleteTime), ref _DeleteTime, value);
                if (!IsLoading && !IsSaving && value != null && modified)
                {
                    AssignedToPicket.PicketStatus = Status.Unused; // Set status of picket = unused when set DeleteTime value
                }
            }
        }

        /// <summary>
        /// To log user actions for PlatformPicketsLog entity
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