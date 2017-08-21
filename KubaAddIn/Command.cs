#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace kuba9195
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            try
            {
                using (Transaction transaction = new Transaction(doc))
                {
                    SharedParameters sp = new SharedParameters(app, uiapp, "G:\\#WORK\\V\\3D\\#PROCHEM_HVAC_plik_parametrow.txt");
                    AddParametersForm form = new AddParametersForm(sp);
                    form.Show();
                }
            }
            catch (Exception)
            {
                throw;
            }


            return Result.Succeeded;
        }
    }
}
