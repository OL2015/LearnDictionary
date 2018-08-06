using DictionaryLogic.ModelProviders.EFModel;
using DictionaryUI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI.ViewModel
{
    public class WordCardViewModel : ViewModelBase
    {
        private ILogService logService;
        
        private IDictionaryDataService dictionaryDataService;
        private LearnDictionaryEntities efContext = null;
        private ObservableCollection<Book> books = new ObservableCollection<Book>();
        private ObservableCollection<Language> languages = new ObservableCollection<Language>();
       
        public WordValuesSuggestionProvider WordSugesstions
        { get; private set; }


        public Guid Token { get; private set; } = Guid.NewGuid();

        public RelayCommand SpeakCommand { get; private set; }
        public RelayCommand ShowMariamWebsterCommand { get; private set; }
        public RelayCommand SaveWordCommand { get; private set; }
        private string enteredText;
        public string EnteredText
        {
            get { return enteredText; }
            set
            {
                Set(() => EnteredText, ref enteredText, value);
            }
        }

        public String Title
        {
            get
            {
                if ( Word == null || String.IsNullOrEmpty(Word.Value))
                    return "<Word not Defined>";
                return Word.Value;
            }
        } 

        private Word word = null;
        public Word Word
        {
            get { return word; }
            set
            {
                if (value == null)
                {
                    // TryEnterNewWord();
                    return;
                }
                Set(() => Word, ref word, value); 
                RaisePropertyChanged("Title");
                 
            }
        } 

        private WordMeaning selectedFromAllMeaning;
        public WordMeaning SelectedFromAllMeaning
        {
            get { return selectedFromAllMeaning; }
            set
            {
                Set(() => SelectedFromAllMeaning, ref selectedFromAllMeaning, value);
            }
        }

        public WordCardViewModel(IDictionaryDataService dictionaryDataService, ILogService logService)
        {
            this.dictionaryDataService = dictionaryDataService;
            this.logService = logService;
            efContext = dictionaryDataService.GetEFLearnDictionaryContext();
            ShowMariamWebsterCommand = new RelayCommand(ShowMariamWebster);
            SpeakCommand = new RelayCommand(Speak);
            SaveWordCommand = new RelayCommand(SaveChanges);
        }

         
        private void Speak()
        {
            using (SpeechSynthesizer sth = new SpeechSynthesizer())
            {
                var voices = sth.GetInstalledVoices();
                Random random = new Random();
                int ind = random.Next(0, voices.Count);
                sth.SelectVoice(voices[ind].VoiceInfo.Name);
                sth.Speak( Word.Value);

                //string z ="The next part that doesn't just use default pronunciation is the date. We use the special SayAs enumeration to specify that the date should be read out as an actual date and not just a set of numbers, spaces and special characters.";
                // sth.Speak(z);
            }
        }
        private void ShowMariamWebster()
        {
            dictionaryDataService.ShowMariamWebster( Word.Value);
        }
        private void SaveChanges()
        {
            try
            { 
                var we = efContext.Entry(Word);
                if (we.State == System.Data.Entity.EntityState.Modified)
                { 
                    efContext.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {  
                 logService.ShowException("Error saving changes", ex);
            }
        }

    }
}
