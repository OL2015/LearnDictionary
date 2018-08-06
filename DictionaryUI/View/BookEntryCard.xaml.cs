using DictionaryLogic.ModelProviders.EFModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace DictionaryUI
{
    /// <summary>
    /// Interaction logic for Book.xaml
    /// </summary>
    public partial class BookEntryCard : Window
    {
        //private static BookEntryCard _bookWindow;
        //public static BookEntryCard GetBookWindow(LearnDictionaryEntities context, Book book = null)
        //{
        //    if (_bookWindow == null)
        //    {
        //        _bookWindow = new BookEntryCard(context, book);
        //        _bookWindow.Closed += delegate(object sender, EventArgs e)
        //        {
        //            ReliaseBookWindow();
        //        };

        //    }
        //    return _bookWindow;
        //}

        //private static void ReliaseBookWindow()
        //{
        //    _bookWindow = null;
        //}
        //private LearnDictionaryEntities _context = null;
        //private Book _book = null;
        //public Book ABook
        //{
        //    get { return _book; }
        //    private set
        //    {
        //        _book = value;
        //        this.DataContext = _book;
        //        cbFormat.DataContext = _book;
        //    }
        //}
        public BookEntryCard()
        {
            InitializeComponent();
        }
        //private BookEntryCard(LearnDictionaryEntities context, Book book = null)
        //{
        //    InitializeComponent();
        //    _context = context;
        //    ABook = book;
        //    if (ABook == null)
        //        ABook = new Book() { RegisterDate = DateTime.Now };
        //    _context.Formats.Load();
        //    cbFormat.ItemsSource = _context.Formats.Local;
        //    cbFormat.DisplayMemberPath = "Name";
        //    cbLanguage.ItemsSource = _context.Languages.Local;
        //    cbLanguage.DisplayMemberPath = "ShortName";
        //    _context.Authors.Load();
        //    lstAllAuthors.ItemsSource = _context.Authors.Local;
        //    lstAllAuthors.DisplayMemberPath = "Name";
        //    // lstBookAuthors.ItemsSource = ABook.Authors;
        //    lstBookAuthors.DisplayMemberPath = "Name";
        //    RefreshBookAuthors();
        //}

        //private void RefreshBookAuthors()
        //{
        //    lstBookAuthors.ItemsSource = null;
        //    lstBookAuthors.ItemsSource = ABook.Authors;
        //    tbAuthorString.Text = ABook.AuthorsString;
        //}

        //private void btnSaveBook_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_context.Entry(ABook).State == System.Data.Entity.EntityState.Detached)
        //    // if (ABook.EntityState == EntityState.Detached)
        //    {
        //        _context.Books.Add(ABook);
        //    }
        //    _context.SaveChanges();
        //    DialogResult = true;
        //    this.Close();
        //}

        //private void btnAddAuthor_Click(object sender, RoutedEventArgs e)
        //{
        //    AddAuthorToBook();
        //}

        //protected void OnMouseDoubleClickAddAuthor(object sender, EventArgs args)
        //{
        //    AddAuthorToBook();
        //}

        //private void btnRemoveAuthor_Click(object sender, RoutedEventArgs e)
        //{
        //    RemoveAuthorToBook();
        //}

        //protected void OnMouseDoubleClickRemoveAuthor(object sender, EventArgs args)
        //{
        //    RemoveAuthorToBook();
        //}

        //private void RemoveAuthorToBook()
        //{
        //    try
        //    {
        //        lstBookAuthors.SelectedItems.Cast<Author>().ToList().ForEach(z => ABook.Authors.Remove(z));
        //        RefreshBookAuthors();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private void AddAuthorToBook()
        //{
        //    try
        //    {
        //        lstAllAuthors.SelectedItems.Cast<Author>().ToList().ForEach(z => ABook.Authors.Add(z));
        //        RefreshBookAuthors();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private void btnCancelBook_Click(object sender, RoutedEventArgs e)
        //{
        //    var book = _context.Entry(ABook);
        //    if (book.State == System.Data.Entity.EntityState.Detached)
        //        return;
        //    book.State = System.Data.Entity.EntityState.Detached;
        //    _context.Books.Load(); // query only current book
        //   // book.Collection(p => p.Authors).CurrentValue.Clear();
        //   // book.Reload();
           
        //    //ABook.Authors = book.Collection(d => d.Authors).Query().ToList();

            
        //    return;
        //    if (book.State == System.Data.Entity.EntityState.Modified)
        //    {
        //        book.CurrentValues.SetValues(book.OriginalValues);
        //        book.State = System.Data.Entity.EntityState.Unchanged;
        //    }
        //}
    }
}
