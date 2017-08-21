/*
 * Created by SharpDevelop.
 * User: kuba9195
 * Date: 2017-01-18
 * Time: 14:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xaml;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;

namespace kuba9195
{
    /// <summary>
    /// Interaction logic for AddParametersForm.xaml
    /// </summary>
    public partial class AddParametersForm
    {
        public SharedParameters SharedParameters { get; set; }

        public AddParametersForm(SharedParameters sp)
        {
            this.SharedParameters = sp;
            this.DataContext = this;
            this.SharedParameters = sp;
            InitializeComponent();
        }


        void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;


            if (openFileDialog.ShowDialog() == true)
            {
                SharedParameters.SharedParametersFilePath = Path.GetFullPath(openFileDialog.FileName);
                //				TaskDialog.Show("Debug",Path.GetFullPath(openFileDialog.FileName));

            }

        }

        void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SharedParameters.ActiveDefinitionGruop = (sender as System.Windows.Controls.ComboBox).SelectedItem as DefinitionGroup;

        }

        void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var definitions = (sender as System.Windows.Controls.ListView).SelectedItems;
            List<Definition> definitionList = new List<Definition>();
            foreach (var def in definitions)
            {
                Definition definition = def as Definition;
                definitionList.Add(definition);
            }
            SharedParameters.ActiveDefinitions = definitionList;

        }

        void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SharedParameters.ActiveBuiltInCategory = (BuiltInCategory)(sender as System.Windows.Controls.ComboBox).SelectedItem;

        }

        void button2_Click(object sender, RoutedEventArgs e)
        {
            //			SharedParameters.Debug();
            SharedParameters.SetNewParametersToInstances();
            this.Close();

        }
    }
}
