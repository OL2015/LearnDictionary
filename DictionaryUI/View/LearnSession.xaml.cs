using DictionaryLogic.ModelProviders.EFModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for LearnSession.xaml
    /// </summary>
    public partial class LearnSession : Window
    {
        public LearnSession()
        {
            InitializeComponent();
                      
        }

        //private void FillWordStatistic()
        //{
        //    foreach(var word in _learningWords)
        //    {
        //        var generalEntries = word.WordMeanings.SelectMany(z => z.LearnStatistics);
        //        int generalCount = generalEntries.Count();
        //        var correctAnswer = generalEntries.Where(z => z.Answer).Count();
        //        word.Rating = generalCount>0 ? ((float)correctAnswer * 100F) / (float)generalCount : 0.0F;
        //    }
        //}
        
    }
}
