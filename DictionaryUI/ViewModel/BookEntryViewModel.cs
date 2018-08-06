using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DictionaryUI.ViewModel
{
    public class BookEntryViewModel: ViewModelBase
    {
        private LearnDictionaryEntities efContext = null;
        private IDictionaryDataService dictionaryDataService;
        private ILogService logService;
        public RelayCommand SaveBookCommand { get; private set; }
        public RelayCommand CancelEditCommand { get; private set; } 
        public RelayCommand AddAuthorCommand { get; private set; }
        public RelayCommand RemoveAuthorCommand { get; private set; } 
         

        private Book selectedBook;
        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                RaisePropertyChanged("SelectedBook");
            }
        }
        private Author selectedAuthorAdd;
        public Author SelectedAuthorAdd
        {
            get { return selectedAuthorAdd; }
            set
            {
                selectedAuthorAdd = value;
                RaisePropertyChanged("SelectedAuthorAdd");
            }
        }
        private Author selectedAuthorRemove;
        public Author SelectedAuthorRemove
        {
            get { return selectedAuthorRemove; }
            set
            {
                selectedAuthorRemove = value;
                RaisePropertyChanged("SelectedAuthorRemove");
            }
        }
        private ObservableCollection<Author>  allAuthors;
        public ObservableCollection<Author> AllAuthors
        {
            get { return allAuthors; }
            set
            {
                allAuthors = value;
                RaisePropertyChanged("AllAuthors");
            }
        }
        private ObservableCollection<Language> languages;
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                RaisePropertyChanged("Languages");
            }
        }
        private ObservableCollection<Format> formats;
        public ObservableCollection<Format> Formats
        {
            get { return formats; }
            set
            {
                formats = value;
                RaisePropertyChanged("Formats");
            }
        } 

        public BookEntryViewModel(IDictionaryDataService dictionaryDataService, ILogService logService)
        {
            this.dictionaryDataService = dictionaryDataService;
            this.logService = logService;
            Messenger.Default.Register<NotificationMessage<Book>>( this, LoadBookContext());

            SaveBookCommand = new RelayCommand(SaveBookMethod);
            CancelEditCommand = new RelayCommand(CancelEditMethod);
            AddAuthorCommand = new RelayCommand(AddAuthorMethod, () => {return SelectedAuthorAdd != null; });
            RemoveAuthorCommand = new RelayCommand(RemoveAuthorMethod, () => { return SelectedAuthorRemove != null; });  
            efContext = dictionaryDataService.GetEFLearnDictionaryContext(); 
            
        }

        private Action<NotificationMessage<Book>> LoadBookContext()
        {
            return message =>
            {
                SelectedBook = message.Content;
                efContext.Authors.Load();
                AllAuthors = efContext.Authors.Local;
                efContext.Languages.Load();
                Languages = efContext.Languages.Local;
                efContext.Formats.Load();
                Formats = efContext.Formats.Local;
            };
        }

        private void RemoveAuthorMethod()
        {
            SelectedBook.Authors.Remove(SelectedAuthorRemove);
            RaisePropertyChanged("SelectedBook");
        }

        private void AddAuthorMethod()
        {
            if (!SelectedBook.Authors.Contains(SelectedAuthorAdd))
            {
                SelectedBook.Authors.Add(SelectedAuthorAdd);
                RaisePropertyChanged("SelectedBook");
            }
        }

        private void CancelEditMethod()
        {

            //efContext.Entry<Book>(SelectedBook).Reload();
            //efContext.Entry(SelectedBook).Collection(p => p.Authors).Load();
            // todo send message
            return;
        }

        private void SaveBookMethod()
        {
            try
            {
                if (efContext.Entry(SelectedBook).State == System.Data.Entity.EntityState.Detached)
                // if (ABook.EntityState == EntityState.Detached)
                {
                    efContext.Books.Add(SelectedBook);
                }
                efContext.SaveChanges();
                efContext.Entry<Book>(SelectedBook).Reload();
                efContext.Entry(SelectedBook).Collection(p => p.Authors).Load();
                // todo send message
                return;
            }
            catch (Exception ex)
            {
                logService.ShowException("Cannot save Book", ex);
            }
            
        }

    }
}
