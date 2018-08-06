using DictionaryUI.View;
using DictionaryUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DictionaryUI.Services
{
    public class OpenViewService : IOpenViewService
    {
        WordBrowserView wordEntryCard = null;
        BookEntryCard bookEntryCard = null;
        public void OpenBookWindow()
        {
            BooksList booksWindow = new BooksList();
            booksWindow.Show();
        }

        public void OpenWordWindow()
        {
            AllWordsWindow wordWindow = new AllWordsWindow();
            wordWindow.Show();
        }

        public void OpenAuthorWindow()
        {
            AuthorsWindow authorWindow = new AuthorsWindow();
            authorWindow.Show();
        }

        public void OpenWordBrowserView(WordBrowserViewModel vm )
        {
            WordBrowserView view = new WordBrowserView();
            view.DataContext = vm;
            view.Show();
        }

        public void OpenWordBrowserToContinueNewView()
        {
            WordBrowserView view = new WordBrowserView();
            var dataservice = ViewModelLocator.getDictionaryDataService();
            var logService = ViewModelLocator.getLogService();

            WordBrowserViewModel vm = new WordBrowserViewModel(dataservice, logService );
            vm.RestoreLastContext();
            view.DataContext = vm;
            view.Show();
        }

        public void OpenLearnWordsWindow()
        {
            LearnWords learnWords = new LearnWords();
            learnWords.Show();
        }

        public void OpenWordCard(WordCardViewModel wm)
        {
            var wc = new WordCard();
            wc.DataContext = wm;
            Window window = new Window
            {
                Title = wm.Word.Value,
                Content = wc 
            };

            window.Show();
        }

        public void OpenLearnSession()
        {
            LearnSession learnSession = new LearnSession();
            learnSession.Show();            
        }

        public void OpenLearnCard(LearnWordsViewModel vm)
        {
            LearnCard learnCard = new LearnCard();
            learnCard.DataContext = vm;
            learnCard.Show();
        }


        public void ShowWordEntryCardDialog()
        {
            if (wordEntryCard != null)
                throw new Exception("WordEntryCard already opened");
            wordEntryCard = new WordBrowserView();
            wordEntryCard.ShowDialog();
        }

        public void CloseWordEntryCardDialog()
        {
            if (wordEntryCard == null)
                throw new Exception("WordEntryCard is null and cannot be closed");
            wordEntryCard.Close();
        }


        public void ShowBookEntryCardDialog()
        {
            if (bookEntryCard != null)
                throw new Exception("BookEntryCard already opened");
            bookEntryCard = new BookEntryCard();
            bookEntryCard.ShowDialog();
        }

        public void CloseBookEntryCardDialog()
        {
            if (bookEntryCard == null)
                throw new Exception("BookEntryCard is null and cannot be closed");
            bookEntryCard.Close();
        }

        public void ShowAllWordsDialog()
        {
            if (bookEntryCard != null)
                throw new Exception("BookEntryCard already opened");
            bookEntryCard = new BookEntryCard();
            bookEntryCard.ShowDialog();
        }

       
    }
}
