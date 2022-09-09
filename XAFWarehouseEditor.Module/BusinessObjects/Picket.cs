using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace XAFWarehouseEditor.Module.BusinessObjects
{
    /// <summary>
    /// Entity for all pickets with relationships:
    /// Warehouse (one) -> Picket (many) 
    /// Picket (one) -> PlatformPicketsLog (many)
    /// </summary>
    [XafDisplayName("Pickets")] // Attribute for display name in application
    [DefaultClassOptions]
    public class Picket : BaseObject
    {
        public Picket(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            PicketStatus = Status.Unused; // Initial data
        }

        /// <summary>
        /// Number of picket 101, 102, 103... etc
        /// </summary>
        uint _PicketNumber;
        [XafDisplayName("Picket number"), ToolTip("Picket number")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [RuleRequiredField, RuleUniqueValue] // It can't be empty or null and must be unique value
        public uint PicketNumber
        {
            get => _PicketNumber;
            set => SetPropertyValue(nameof(PicketNumber), ref _PicketNumber, value);
        }

        /// <summary>
        /// Relationship with Warehouse:
        /// Warehouse (one) -> Picket (many) 
        /// </summary>
        Warehouse _AssignedToWarehouse;
        [Association("Warehouse-Pickets")]
        [RuleRequiredField(DefaultContexts.Save)] // It can't be empty or null
        [XafDisplayName("Warehouse"), ToolTip("Assigned to warehouse")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        public Warehouse AssignedToWarehouse
        {
            get => _AssignedToWarehouse;
            set => SetPropertyValue(nameof(AssignedToWarehouse), ref _AssignedToWarehouse, value);
        }

        /// <summary>
        /// Status values for picket (enum: Used, Unused) 
        /// </summary>
        private Status _PicketStatus;
        [XafDisplayName("Picket status"), ToolTip("Picket status")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        public Status PicketStatus
        {
            get => _PicketStatus;
            set => SetPropertyValue(nameof(PicketStatus), ref _PicketStatus, value);
        }

        /// <summary>
        /// Relationships with PlatformPicketsLog entities
        /// </summary>
        [Association("Picket-PlatformPicketsLog")]
        public XPCollection<PlatformPicketsLog> PicketHistory
        {
            get => GetCollection<PlatformPicketsLog>(nameof(PicketHistory));            
        }


        /// <summary>
        /// To log user actions for Picket entity
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

    /// <summary>
    /// Status values for Picket
    /// </summary>
    public enum Status
    {
        Unused = 0,
        Used = 1
    }
}