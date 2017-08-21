using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace kuba9195
{
    /// <summary>
    /// Description of SharedParameters.
    /// </summary>
    public class SharedParameters : INotifyPropertyChanged
    {
        public static string DefaultSharedParameterPath = @"G:\#WORK\V\3D\#PROCHEM_HVAC_plik_parametrow.txt";
        public Autodesk.Revit.ApplicationServices.Application App { get; set; }
        public Autodesk.Revit.UI.UIApplication UIApp { get; set; }

        private DefinitionFile _sharedParameterFile;
        public DefinitionFile SharedParameterFile
        {
            get
            {
                return _sharedParameterFile;
            }
            set
            {
                _sharedParameterFile = value;
                OnPropertyChanged("SharedParameterFile");
            }
        }

        private string _sharedParametersFilePath = "";
        public string SharedParametersFilePath
        {
            get
            {
                return Path.GetFullPath(_sharedParametersFilePath);
            }
            set
            {

                if (!string.IsNullOrEmpty(value) && IfSharedParameterFileIsCorect(value))
                {
                    _sharedParametersFilePath = value;
                    App.SharedParametersFilename = value;
                    SharedParameterFile = App.OpenSharedParameterFile();
                }
                else
                {
                    if (string.IsNullOrEmpty(App.SharedParametersFilename))
                    {
                        _sharedParametersFilePath = Path.GetFullPath(DefaultSharedParameterPath);
                        App.SharedParametersFilename = DefaultSharedParameterPath;
                        SharedParameterFile = App.OpenSharedParameterFile();
                    }

                    SharedParameterFile = App.OpenSharedParameterFile();
                }
                OnPropertyChanged("SharedParametersFilePath");
            }
        }

        private DefinitionGroup _activeDefinitionGruop;
        public DefinitionGroup ActiveDefinitionGruop
        {
            get
            {
                return _activeDefinitionGruop;
            }
            set
            {
                _activeDefinitionGruop = value;
                OnPropertyChanged("ActiveDefinitionGruop");
            }
        }

        private List<Definition> _activeDefinitions;
        public List<Definition> ActiveDefinitions
        {
            get
            {
                return _activeDefinitions;
            }
            set
            {
                _activeDefinitions = value;
                OnPropertyChanged("ActiveDefinitions");
            }
        }

        public DefinitionGroups ParameterGroups
        {
            get
            {
                if (SharedParameterFile != null)
                    return SharedParameterFile.Groups;
                else
                    return null;
            }
        }
        public List<BuiltInCategory> FamilyCategory
        {
            get
            {
                List<BuiltInCategory> unsortedList = BuiltInCategory.GetValues(typeof(BuiltInCategory)).Cast<BuiltInCategory>().ToList();
                unsortedList.Sort(new BuiltInCategoryComparer());
                return unsortedList;
            }
        }
        private BuiltInCategory _activeBuiltInCategory;
        public BuiltInCategory ActiveBuiltInCategory
        {
            get
            {
                return _activeBuiltInCategory;
            }
            set
            {
                _activeBuiltInCategory = value;
                OnPropertyChanged("ActiveBuiltInCategory");
            }
        }

        public SharedParameters(Autodesk.Revit.ApplicationServices.Application app, Autodesk.Revit.UI.UIApplication uiapp, string sharedParametersPath)
        {
            this.App = app;
            this.UIApp = uiapp;
            this.SharedParametersFilePath = sharedParametersPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public Definitions GetParametersDefinitionByGrup(DefinitionGroup paramGroup)
        {
            return paramGroup.Definitions;
        }

        public List<bool> SetNewParametersToInstances()
        {
            List<bool> instanceBindingOK = new List<bool>();

            if (SharedParameterFile != null && ActiveDefinitions != null)
            {
                CategorySet myCategories = App.Create.NewCategorySet();

                Category myCategory = UIApp.ActiveUIDocument.Document.Settings.Categories.get_Item(ActiveBuiltInCategory);

                myCategories.Insert(myCategory);

                InstanceBinding instanceBinding = App.Create.NewInstanceBinding(myCategories);

                BindingMap bindingMap = UIApp.ActiveUIDocument.Document.ParameterBindings;

                foreach (Definition definition in ActiveDefinitions)
                {
                    bool IsOk = bindingMap.Insert(definition, instanceBinding, BuiltInParameterGroup.PG_TEXT);
                    instanceBindingOK.Add(IsOk);
                }

                UIApp.ActiveUIDocument.SaveAs();

            }
            else
            {
                instanceBindingOK.Add(false);
            }
            return instanceBindingOK;
        }
        public void Debug()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(App.ToString());
            sb.AppendLine();
            sb.Append(UIApp.ToString());
            sb.AppendLine();
            sb.Append(SharedParameterFile.Filename.ToString());
            sb.AppendLine();
            sb.Append(SharedParametersFilePath.ToString());
            sb.AppendLine();
            sb.Append(ActiveDefinitionGruop.Name.ToString());
            sb.AppendLine();
            sb.Append(ActiveDefinitions.Count.ToString());
            sb.AppendLine();
            sb.Append(ActiveBuiltInCategory.ToString());
            sb.AppendLine();



            TaskDialog.Show("Debug", sb.ToString());
        }

        //TO DO
        private bool IfSharedParameterFileIsCorect(string path)
        {
            return true;
        }

    }
    public class BuiltInCategoryComparer : IComparer<BuiltInCategory>
    {
        public int Compare(BuiltInCategory x, BuiltInCategory y)
        {
            return x.ToString().CompareTo(y.ToString());
        }
    }
}
