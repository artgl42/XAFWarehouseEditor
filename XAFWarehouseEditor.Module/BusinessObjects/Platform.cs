using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace XAFWarehouseEditor.Module.BusinessObjects
{
    /// <summary>
    /// Entity for all platforms with relationships:
    /// Platform (one) -> PlatformPicketsLog (many)
    /// Platform (one) -> PlatformCargoLog (many)
    /// </summary>
    [XafDisplayName("Platforms")] // Attribute for display name in application
    [DefaultClassOptions]
    public class Platform : BaseObject
    {
        public Platform(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        /// <summary>
        /// Number (string format) of platform 101-104, 105, 201-203... etc
        /// </summary>
        private string _PlatformNumber;
        [XafDisplayName("Platform number"), ToolTip("Platform number")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [RuleRequiredField] // It can't be empty or null
        public string PlatformNumber
        {
            get => _PlatformNumber;
            set => SetPropertyValue(nameof(PlatformNumber), ref _PlatformNumber, value);
        }

        /// <summary>
        /// Relationship with PlatformPicketsLog:
        /// Platform (one) -> PlatformPicketsLog (many) 
        /// </summary>
        [Association("Platform-PlatformPicketsLog")]
        public XPCollection<PlatformPicketsLog> PicketHistory
        {
            get => GetCollection<PlatformPicketsLog>(nameof(PicketHistory));       
        }

        /// <summary>
        /// Relationship with PlatformCargoLog:
        /// Platform (one) -> PlatformCargoLog (many) 
        /// </summary>
        [Association("Platform-PlatformCargoLog")]
        public XPCollection<PlatformCargoLog> PlatformHistory
        {
            get => GetCollection<PlatformCargoLog>(nameof(PlatformHistory));           
        }

        /// <summary>
        /// To log user actions for Platform entity
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