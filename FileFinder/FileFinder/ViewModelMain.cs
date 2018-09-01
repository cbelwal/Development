using System;
using System.Windows.Input;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace FileFinder
{
    public class ViewModelMain : INotifyPropertyChanged
    {
        private String _folderName;
        private String _searchText;
        private FileSearch _fileSearch;
        private ObservableCollection<string> _foundFiles;

        public string StatusMessage {get;set;}
        public ObservableCollection<string> FoundFiles
        {
            get
            {
                return _foundFiles;
            }
            private set
            {
                _foundFiles = value;
            }
        }

        public ICommand StartCommand { get; set; }
        public ICommand BrowseCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void OnPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }            
        }

        public ViewModelMain()
        {
            StartCommand = new RelayCommand(OnStartClicked);
            BrowseCommand = new RelayCommand(OnBrowseClicked);
            FolderName = AppSettings.Default.FolderName;
            SearchText = AppSettings.Default.SearchText;
            FoundFiles = new ObservableCollection<string>();
            StatusMessage = Resources.StringResources.Ready;
            _fileSearch = new FileSearch();
            _fileSearch.FolderUpdate += _fileSearch_FolderUpdate;
            _fileSearch.FoundStringInFile += _fileSearch_FoundStringInFile;
        }

        private void _fileSearch_FoundStringInFile(object sender, string e)
        {
            _foundFiles.Add(e);
            OnPropertyChanged("FoundFiles");
        }

        private void _fileSearch_FolderUpdate(object sender, string e)
        {
            StatusMessage = Resources.StringResources.SearchingFolder + e;

            OnPropertyChanged("StatusMessage");
        }

        public String FolderName
        {
            get
            {
                return _folderName;
            }
            set
            {
                _folderName = value;
                OnPropertyChanged("FolderName");
                AppSettings.Default.FolderName = _folderName;
                AppSettings.Default.Save();
            }
        }

        /// <summary>
        /// Text to search
        /// </summary>
        public String SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                AppSettings.Default.SearchText = _searchText;
                AppSettings.Default.Save();
            }
        }

      

        public void OnStartClicked(object obj)
        {
            //Start the Search   
            _fileSearch.SearchInFolder(_folderName,_searchText);
            StatusMessage = Resources.StringResources.Ready;
            OnPropertyChanged("StatusMessage");
        }

        public void OnBrowseClicked(object obj)
        {
            var dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog();
            FolderName = dlg.SelectedPath;            
        }

    }
}
