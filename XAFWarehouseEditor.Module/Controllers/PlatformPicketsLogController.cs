using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using XAFWarehouseEditor.Module.BusinessObjects;

namespace XAFWarehouseEditor.Module.Controllers
{
    /// <summary>
    /// This controller is used for disconnect picket from platform for PlatformPicketsLog
    /// Action "Unused" - set DeleteTime to today of PlatformPicketsLog entity
    /// </summary>
    public class PlatformPicketsLogController : ViewController
    {
        SimpleAction Unused;

        public PlatformPicketsLogController()
        {
            TargetViewType = ViewType.ListView; // Set which View is used for action button
            TargetObjectType = typeof(PlatformPicketsLog); // Set which Entity is used
            Unused = new SimpleAction(this, "Unused", PredefinedCategory.Edit) // Create and initial the Action "Unused"
            {
                Caption = "Unused today",
                ConfirmationMessage = "Are you sure you want to disconnect the picket from the platform?",
                ImageName = "Today",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject
            };
            Unused.Execute += Unused_Execute;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            Unused.Execute -= Unused_Execute;
            base.OnDeactivated();
        }

        /// <summary>
        /// When action button clicked -> set DeleteTime to today of PlatformPicketsLog entity
        /// </summary>
        private void Unused_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // How to create and show error message in application
            //MessageOptions messageOptions = new MessageOptions()
            //{
            //    Duration = 5000,
            //    Type = InformationType.Error,
            //    Message = "PlatformPicketsLogController error message!"
            //};
            //Application.ShowViewStrategy.ShowMessage(messageOptions);

            var currentObject = e.CurrentObject as PlatformPicketsLog;
            if (currentObject != null)
            {
                currentObject.DeleteTime = DateTime.Now;
            }

            if (ObjectSpace.IsModified)
            {
                ObjectSpace.CommitChanges(); // Save changes in database
            }
        }
    }
}
