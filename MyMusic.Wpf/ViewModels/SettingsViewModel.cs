using MyMusic.Wpf.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MyMusic.Wpf.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private string _rootPath;
        private Settings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public SettingsViewModel(Settings settings)
        {
            _settings = settings;
            RootPath = _settings.RootPath;
            SelectDirectoryCommand = new DelegateCommand(SelectDirectory);
            OkayCommand = new DelegateCommand<Window>(ExecuteOkay);
            CancelCommand = new DelegateCommand<Window>(ExecuteCancel);
        }

        /// <summary>
        /// Gets or sets the select directory command.
        /// </summary>
        public ICommand SelectDirectoryCommand { get; set; }

        /// <summary>
        /// Gets or sets the okay command.
        /// </summary>
        public ICommand OkayCommand { get; set; }

        /// <summary>
        /// Gets or sets the cancel command.
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Gets or sets the root path.
        /// </summary>
        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                SetProperty(ref _rootPath, value);
            }
        }

        private void ExecuteCancel(Window obj)
        {
            if (obj != null)
            {
                obj.DialogResult = false;
                obj.Close();
            }
        }

        private void ExecuteOkay(Window obj)
        {
            if (obj != null)
            {
                obj.DialogResult = true;
                _settings.RootPath = RootPath;
                obj.Close();
            }
        }

        private void SelectDirectory()
        {
            using FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                RootPath = dialog.SelectedPath;
            }
        }
    }
}
