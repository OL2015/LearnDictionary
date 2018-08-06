using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace DictionaryUI.ViewModel
{
    public class BookListViewModel : ViewModelBase
    {
        private LearnDictionaryEntities efContext = null;
        private IDictionaryDataService dictionaryDataService;
        private ILogService logService;
        private IOpenViewService openViewService;
        public RelayCommand AddBookCommand { get; private set; }
        public RelayCommand EditBookCommand { get; private set; }
        public RelayCommand DeleteBookCommand { get; private set; }


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

        private ObservableCollection<Book> _books = new System.Collections.ObjectModel.ObservableCollection<Book>();

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                RaisePropertyChanged("Books");
            }
        }
        public BookListViewModel(IDictionaryDataService dictionaryDataService, ILogService logService, IOpenViewService openViewService)
        {
            this.dictionaryDataService = dictionaryDataService;
            this.logService = logService;
            this.openViewService = openViewService;
            AddBookCommand = new RelayCommand(AddBookMethod);
            EditBookCommand = new RelayCommand(EditBookMethod);
            DeleteBookCommand = new RelayCommand(DeleteBookMethod); 
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            efContext.Books.Load();
            Books = efContext.Books.Local;
        }

        private void EditBookMethod()
        {
            try
            {
                Messenger.Default.Send(
                    new NotificationMessage<Book>(this, SelectedBook, "")); 
                openViewService.ShowBookEntryCardDialog();
                
                return;
                //BookEntryCard bookWindow = BookEntryCard.GetBookWindow(efContext, SelectedBook);
                //var res = bookWindow.ShowDialog();
                ////efContext.Entry(SelectedBook).Collection(p => p.Authors).Load();
                //Books = null;
                //efContext.Books.Load();
                //Books = efContext.Books.Local; 
            }
            catch (Exception ex)
            {
                logService.ShowException("Cannot Edit Book", ex);
            }
        }

        private void DeleteBookMethod()
        {
            try
            {
                efContext.Books.Remove(SelectedBook);
                efContext.SaveChanges();
                efContext.Books.Load();
                RaisePropertyChanged("Books");
            }
            catch (Exception ex)
            {
                logService.ShowException("Cannot Delete Book", ex);
            }
        }

        private void AddBookMethod()
        {
            try
            {
                Messenger.Default.Send(
                   new NotificationMessage<Book>(this, new Book(), "")); 
                openViewService.ShowBookEntryCardDialog();
                openViewService.CloseBookEntryCardDialog(); 
                return;
                //BookEntryCard bookWindow = BookEntryCard.GetBookWindow(efContext);
                //bookWindow.ShowDialog();
                //efContext.Books.Load();
                //RaisePropertyChanged("Books");
                //SelectedBook = bookWindow.ABook;
            }
            catch (Exception ex)
            {
                logService.ShowException("Cannot Add Book", ex);
            }
        }

    }
}
