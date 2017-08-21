#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
#endregion

namespace kuba9195
{
    class App : IExternalApplication
    {

        public Result OnStartup(UIControlledApplication app)
        {
            

            RibbonPanel panel = app.GetRibbonPanels().Find(c => c.Name == "Add-Ins");
            TaskDialog.Show("Debug", panel.Name);
            AddPushButton(panel);


            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }

        public void AddPushButton(RibbonPanel panel)
        {
            //var i = Environment.CurrentDirectory.LastIndexOf(@"\");
            //string buttonPath = Environment.CurrentDirectory.Substring(0, i);
            string buttonPath = @"C:\Users\kuba9195\Downloads\VS\kuba9195\KubaAddIn\bin\Debug\KubaAddIn.dll";
            TaskDialog.Show("Debug", buttonPath);


            PushButtonData buttonData = new PushButtonData("Add_Parameters_button", "Dodaj parametry", buttonPath, "kuba9195.Command");

            PushButton button = panel.AddItem(buttonData) as PushButton;
            button.LargeImage = new BitmapImage(new Uri(@"C:\Users\kuba9195\Downloads\VS\kuba9195\KubaAddIn\icon.png"));
        }
    }
}
