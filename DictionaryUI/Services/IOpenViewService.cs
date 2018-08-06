using DictionaryUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI.Services
{
    public interface IOpenViewService
    {
        void OpenBookWindow();
        void OpenWordWindow();
        void OpenWordBrowserToContinueNewView();
        void OpenWordBrowserView(WordBrowserViewModel vm);
        void OpenWordCard(WordCardViewModel vm);
        void OpenAuthorWindow();
        void OpenLearnWordsWindow();
        void OpenLearnSession();
        void OpenLearnCard(LearnWordsViewModel vw);

        void ShowWordEntryCardDialog();
        void CloseWordEntryCardDialog();
        void ShowBookEntryCardDialog();
        void CloseBookEntryCardDialog();
    }
}
