using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace XAFWarehouseEditor.Module.BusinessObjects
{
    /// <summary>
    /// Entity for all warehouses with relationships:
    /// Warehouse (one) -> Picket (many) 
    /// </summary>
    [XafDisplayName("Warehouses")] // Attribute for display name in application
    [DefaultClassOptions]
    public class Warehouse : BaseObject
    {
        public Warehouse(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        /// <summary>
        /// Name for warehouse "Warehouse 1", "Warehouse 2"... etc
        /// </summary>
        string _WarehouseName;
        [XafDisplayName("Warehouse"), ToolTip("Warehouse")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [Index(0)] // Arranged fields in automatically generated Views
        [VisibleInListView(true)] // Display field in ListView
        [VisibleInDetailView(true)] // Display field in DetailView
        [Size(SizeAttribute.DefaultStringMappingFieldSize)] // Size of field
        [RuleRequiredField(DefaultContexts.Save)] // It can't be empty or null
        public string WarehouseName
        {
            get => _WarehouseName;
            set => SetPropertyValue(nameof(WarehouseName), ref _WarehouseName, value);
        }

        /// <summary>
        /// Relationships with Pickets entities
        /// </summary>
        [Association("Warehouse-Pickets")]
        public XPCollection<Picket> Pickets
        {
            get  => GetCollection<Picket>(nameof(Pickets));            
        }

        /// <summary>
        /// Display number od pickets for each warehouse
        /// </summary>
        [NonPersistent] // Column "PicketCount" has not been created in database
        [XafDisplayName("Pickets"), ToolTip("Number of pickets")] // XafDisplayName, ToolTip - attributes for display name and tooltop in application
        [Index(1)] // Arranged fields in automatically generated Views
        [VisibleInListView(true)] // Display field in ListView
        [VisibleInDetailView(false)] // Display field in DetailView
        public int PicketCount
        {
            get => Pickets.Count;
        }

        /// <summary>
        /// To log user actions for Warehouse entity
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