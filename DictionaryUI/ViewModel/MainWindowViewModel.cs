using System;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;

namespace DictionaryUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        private IOpenViewService openViewService;
        public RelayCommand ContinueNewWordsCommand { get; private set; }
        public RelayCommand OpenBooksWindowCommand { get; private set; }
        public RelayCommand OpenWordsWindowCommand { get; private set; }
        public RelayCommand OpenAuthorWindowCommand { get; private set; }
        public RelayCommand OpenLearnWordsWindowCommand { get; private set; }
        public RelayCommand OpenEnlistServersWindowCommand { get; private set; }
        public RelayCommand ServerNameChangedCommand { get; private set; }

        private System.Data.DataRowView selectedServer;
        public System.Data.DataRowView SelectedServer
        {
            get { return selectedServer; }
            set
            { 
                Set(()=> SelectedServer, ref selectedServer, value);
                //selectedServer = value;
                //RaisePropertyChanged("DataServers");
            }
        }

        private int selectedServerIndex;
        public int SelectedServerIndex
        {
            get { return selectedServerIndex; }
            set
            {
                Set(() => SelectedServerIndex, ref selectedServerIndex, value);
                //selectedServer = value;
                //RaisePropertyChanged("DataServers");
            }
        }

        private DataView  dataServers  ;
        public DataView DataServers
        {
            get { return dataServers; }
            set
            {
                dataServers = value;
                RaisePropertyChanged("DataServers");
            }
        }

        public MainWindowViewModel(IOpenViewService openViewService)
        {
            this.openViewService = openViewService;
            ContinueNewWordsCommand = new RelayCommand(ContinueNewWords);
            OpenBooksWindowCommand = new RelayCommand(OpenBooksWindow);
            OpenWordsWindowCommand = new RelayCommand(OpenWordsWindow);
            OpenAuthorWindowCommand = new RelayCommand(OpenAuthorWindow);
            OpenLearnWordsWindowCommand = new RelayCommand(OpenLearnWords);
            OpenEnlistServersWindowCommand = new RelayCommand(EnlistServers);
            ServerNameChangedCommand = new RelayCommand(ServerNameChanged);
        }

        private void ContinueNewWords()
        {
            try
            {
                openViewService.OpenWordBrowserToContinueNewView();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ServerNameChanged()
        {
            MessageBox.Show($"{SelectedServer.Row["ServerName"]} chosen");
        }

        private async void EnlistServers()
        {  
            System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
            System.Data.DataTable  dataTable = await Task<System.Data.DataTable>.Run(() =>
         {
             return instance.GetDataSources();

         }).ConfigureAwait(true);
           
            //DataServers.Clear();
            //foreach (var r in dataTable.Rows)
            //    DataServers.Add( (DataRow)r);
            dataTable.Rows.Add(dataTable.NewRow());
            DataServers = dataTable.DefaultView;
            SelectedServer = DataServers.Table.DefaultView[1];
            //SelectedServerIndex = DataServers.Rows.Count > 0 ? 0 : -1;
        }

        private void OpenLearnWords()
        {
            openViewService.OpenLearnWordsWindow();

        }

        private void OpenAuthorWindow()
        {
            openViewService.OpenAuthorWindow();
        }

        private void OpenWordsWindow()
        {
            openViewService.OpenWordWindow();
        }

        private void OpenBooksWindow()
        {
            openViewService.OpenBookWindow();
        }
    }
}