using DictionaryUI.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;

namespace DictionaryUI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ILogService, LogService>();
            //SimpleIoc.Default.Register<ITranslationService, TranslationServiceYandex>();
SimpleIoc.Default.Register<ITranslationService, TranslationServiceGlosbe>();

            SimpleIoc.Default.Register<IOpenViewService, OpenViewService>();
            SimpleIoc.Default.Register<IDictionaryDataService, DictionaryDataService>();


            SimpleIoc.Default.Register<TranslationLookUpViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<AuthorsWindowViewModel>();
            SimpleIoc.Default.Register<BookListViewModel>();
            SimpleIoc.Default.Register<BookEntryViewModel>();
            SimpleIoc.Default.Register<LearnWordsViewModel>();
            //SimpleIoc.Default.Register<LearnSessionViewModel>();
            SimpleIoc.Default.Register<LearnCardViewModel>();
            SimpleIoc.Default.Register<AllWordsViewModel>();
            SimpleIoc.Default.Register<WordEntryCardViewModel>();
            SimpleIoc.Default.Register<WordBrowserViewModel>();
            ServiceLocator.Current.GetInstance<BookEntryViewModel>();
        }
        public static IDictionaryDataService getDictionaryDataService()
        {
            return SimpleIoc.Default.GetInstance<IDictionaryDataService>();
        }

        public static ILogService getLogService()
        {
            return SimpleIoc.Default.GetInstance<ILogService>();
        }
        public static ITranslationService getTranslationService()
        {
            return SimpleIoc.Default.GetInstance<ITranslationService>();
        }
        //public static ITranslationService getNewTranslationService()
        //{
        //    Type t = getTranslationService().GetType();
        //    ITranslationService z;
        //    z = t.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ITranslationService;
        //    return z; 
        //}

        /// <summary>
        /// Gets the TranslationLookUp property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public TranslationLookUpViewModel TranslationLookUp
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TranslationLookUpViewModel>();
            }
        }

        /// <summary>
        /// Gets the TranslationLookUp property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainWindowViewModel MainWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        /// <summary>
        /// Gets the AuthorsWindowViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AuthorsWindowViewModel AuthorsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthorsWindowViewModel>();
            }
        }

        /// <summary>
        /// Gets the BookListViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public BookListViewModel BookListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BookListViewModel>();
            }
        }

        /// <summary>
        /// Gets the BookEntryViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public BookEntryViewModel BookEntryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BookEntryViewModel>();
            }
        }

        /// <summary>
        /// Gets the LearnWordsViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LearnWordsViewModel LearnWordsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LearnWordsViewModel>();
            }
        }

        ///// <summary>
        ///// Gets the LearnSessionViewModel property.
        ///// </summary>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        //    "CA1822:MarkMembersAsStatic",
        //    Justification = "This non-static member is needed for data binding purposes.")]
        //public LearnSessionViewModel LearnSessionViewModel
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<LearnSessionViewModel>();
        //    }
        //}

        /// <summary>
        /// Gets the LearnCardViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LearnCardViewModel LearnCardViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LearnCardViewModel>();
            }
        }

        /// <summary>
        /// Gets the AllWordsViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AllWordsViewModel AllWordsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AllWordsViewModel>();
            }
        }

        /// <summary>
        /// Gets the WordEntryCardViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public WordEntryCardViewModel WordEntryCardViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WordEntryCardViewModel>();
            }
        }

        /// <summary>
        /// Gets the WordBrowserViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public WordBrowserViewModel WordBrowserViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WordBrowserViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}

