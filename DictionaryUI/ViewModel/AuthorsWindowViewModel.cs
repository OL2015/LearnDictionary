using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DictionaryUI.ViewModel
{

    public class AuthorsWindowViewModel : ViewModelBase
    {
        private LearnDictionaryEntities efContext = null;
        private IDictionaryDataService dictionaryDataService;
        private ILogService logService;
        public RelayCommand AddAuthorCommand { get; private set; }
        public RelayCommand DeleteAuthorCommand { get; private set; }
        public RelayCommand SaveAuthorsCommand { get; private set; }
       
        private Author selectedAuthor;
        public Author SelectedAuthor
        {
            get { return selectedAuthor; }
            set
            {
                this.Set("SelectedAuthor", ref selectedAuthor, value); 
            }
        }

        private ObservableCollection<Author> _authors = new System.Collections.ObjectModel.ObservableCollection<Author>();

        public ObservableCollection<Author> Authors
        {
            get { return _authors; }
            set
            {
                this.Set("Authors", ref _authors, value);  
            }
        }

        public AuthorsWindowViewModel(IDictionaryDataService dictionaryDataService, ILogService logService)
        {
            this.dictionaryDataService = dictionaryDataService;
            this.logService = logService;
            AddAuthorCommand = new RelayCommand(AddAuthorMethod);
            DeleteAuthorCommand = new RelayCommand(DeleteAuthorMethod);
            SaveAuthorsCommand = new RelayCommand(SaveAuthorsMethod); 
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            efContext.Authors.Load();
            Authors = efContext.Authors.Local;
           // SelectedAuthor = Authors.ElementAt(0);
        }
 

        private void SaveAuthorsMethod()
        {
            try
            {
                int selectedID = SelectedAuthor !=null? SelectedAuthor.Author_ID:-1;
                efContext.SaveChanges();
                efContext.Authors.Load();
                Authors = null;
                Authors = efContext.Authors.Local;
                SelectedAuthor = Authors.FirstOrDefault(z => z.Author_ID == selectedID);
            }
            catch (Exception ex)
            {
                logService.ShowException("Cannot save Authors", ex);
            }
        }

        private void DeleteAuthorMethod()
        {
            try
            {
                if (SelectedAuthor.Books.Count > 0)
                { 
                    logService.ShowMessage("Cannot Delete Author Having books" );
                    return;
                }
                Authors.Remove(SelectedAuthor);
                if (Authors.Count > 0)
                    SelectedAuthor = Authors.ElementAt(0);
                else
                    SelectedAuthor = null;
            }
            catch (Exception ex)
            {
                logService.ShowException("Cannot Delete Author", ex);
            }
        }

        private void AddAuthorMethod()
        {
            try
            {
                Author newAuthor = new Author();
                Authors.Add(newAuthor);
                this.SelectedAuthor = newAuthor;
            }
            catch (Exception ex)
            {
                logService.ShowException("Cannot Add new Author", ex);
            }
        }


    }
}
