using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Linq;
using DictionaryLogic;
using System.Data;
using DictionaryLogic.ModelProviders;
using DictionaryLogic.ModelProviders.EFModel;

namespace DictionaryLogic
{
    public class DictionaryFacade
    {
        private static DictionaryFacade facade;
        //private SqlConnection connection;
        public string ConnectionString { get; set; }
        public string ConnectionStringEF { get; set; }

        //private List<BookLanguage> bookLanguages = null;
        //private List<BookFormat> bookFormats = null;
        LearnDictionaryEntities _learnDictionaryEntities = null;

        public static DictionaryFacade GetFacade()
        {
            if (facade == null)
                facade = new DictionaryFacade();
            return facade;
        }

        private DictionaryFacade()
        {
            ConnectionString = "";
        }
        public bool TestConnection()
        {
            return DictionaryFacade.TestConnection(this.ConnectionString);
        }

        public static bool TestConnection(string connStr)
        {
            //connString = connStr;
            using (var connection = new SqlConnection(connStr))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception)
                { }
            }
            return false;
        }

        public LearnDictionaryEntities GetEFLearnDictionaryContext(string connName)
        {
            // if (_learnDictionaryEntities == null || _learnDictionaryEntities.Connection.ConnectionString != ConnectionStringEF)
            // _learnDictionaryEntities = new EF_BookEntities(ConnectionStringEF);
            // if (_learnDictionaryEntities == null  )
            // _learnDictionaryEntities = LearnDictionaryEntities.getLearnDictionaryEntities();
            if (_learnDictionaryEntities == null)
                _learnDictionaryEntities = new LearnDictionaryEntities();
            return _learnDictionaryEntities;
        }



        //public EntityModel GetEntityModel()
        //{
        //    if (_entityModel == null || _entityModel.connectionString != ConnectionString)
        //        _entityModel = new EntityModel(ConnectionString);
        //    return _entityModel;
        //}

        //public DataSetModel GetDataSetModel()
        //{
        //    if (_dataSetModel == null)
        //        _dataSetModel = new DataSetModel(ConnectionString);
        //    return _dataSetModel;
        //}

        //public TypedDataSetModel GetTypedDataSetModel()
        //{
        //    if (_typedDataSetModel == null)
        //        _typedDataSetModel = new TypedDataSetModel(ConnectionString);
        //    return _typedDataSetModel;
        //}

        //public ArrayList GetAuthors()
        //{
        //    using (connection = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT NumberOfPages FROM Book", connection);
        //        ArrayList rowList = new ArrayList();

        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            for (int i = 0; i < reader.FieldCount; i++)
        //            {
        //                if (reader.IsDBNull(i))
        //                    rowList.Add(reader.GetName(i) + ": No data");
        //                else
        //                    rowList.Add(reader.GetName(i) + ": " + reader[i]);
        //            }
        //        }
        //        reader.Close();
        //        return rowList;
        //    }
        //}

        //public string GetAuthors(string name)
        //{
        //    var commandStr = "SELECT Authors FROM Book WHERE Name = @Name;";
        //    using (connection = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = new SqlCommand(commandStr, connection);
        //        cmd.Parameters.AddWithValue("@Name", name);
        //        connection.Open();
        //        return (string)cmd.ExecuteScalar();
        //    }
        //}

        //public List<Author> GetAuthors(int bookID)
        //{
        //    List<Author> listAuthors = new List<Author>();
        //    var commandStr = "select * from Author where Author_ID in (select Author_ID from AuthorBook where Book_ID = @Book_ID);";
        //    using (connection = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = new SqlCommand(commandStr, connection);
        //        cmd.Parameters.AddWithValue("@Book_ID", bookID);
        //        connection.Open();
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                Author author = new Author()
        //                {
        //                    Author_ID = (int)reader["Author_ID"],
        //                    Name = reader["Name"].ToString()
        //                };
        //                listAuthors.Add(author);
        //            }
        //        }
        //    }
        //    return listAuthors;
        //}

        //public List<object> SelectAll()
        //{
        //    List<object> pages = new List<object>();
        //    using (connection = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM Book", connection);
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            //vyborka po indexatoru
        //            object p = reader["NumberOfPages"];
        //            object authors = reader["Authors"];

        //            pages.Add((object)authors);
        //        }
        //        reader.Close();
        //        return pages;
        //    }
        //}

        //public int CreateBook(string authors, string name, int language, int pages, int format)
        //{
        //    var commandStr = "INSERT Book VALUES ('@authors', '@name', @language, @pages, @format;";
        //    using (connection = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = connection.CreateCommand();
        //        cmd.CommandText = commandStr;
        //        cmd.Parameters.AddWithValue("@authors", authors);
        //        cmd.Parameters.AddWithValue("@name", name);
        //        cmd.Parameters.AddWithValue("@language", language);
        //        cmd.Parameters.AddWithValue("@pages", pages);
        //        cmd.Parameters.AddWithValue("@format", format);
        //        connection.Open();
        //        int rowAffected = cmd.ExecuteNonQuery();
        //        return rowAffected;
        //    }
        //}

        //public List<BookFormat> Formats 
        //{
        //    get
        //    {
        //        if (bookFormats == null)
        //        {
        //            bookFormats = new List<BookFormat>();
        //            var commandStr = "SELECT * FROM Format;";
        //            using (connection = new SqlConnection(connString))
        //            {
        //                SqlCommand cmd = new SqlCommand(commandStr, connection);
        //                connection.Open();
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        BookFormat format = new BookFormat();
        //                        format.Format_ID = Int32.Parse(reader["Format_ID"].ToString());
        //                        format.Name = reader["Name"].ToString();
        //                        bookFormats.Add(format);
        //                    }
        //                }
        //            }
        //        }
        //        return bookFormats;
        //    }
        //}

        //public List<BookLanguage> Languages
        //{
        //    get
        //    {
        //        if (bookLanguages == null)
        //        {
        //            bookLanguages = new List<BookLanguage>();
        //            var commandStr = "SELECT * FROM Language;";
        //            using (connection = new SqlConnection(connString))
        //            {
        //                SqlCommand cmd = new SqlCommand(commandStr, connection);
        //                connection.Open();
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        BookLanguage language = new BookLanguage();
        //                        language.Language_ID = Int32.Parse(reader["Language_ID"].ToString());
        //                        language.ShortName = reader["ShortName"].ToString();
        //                        bookLanguages.Add(language);
        //                    }
        //                }
        //            }
        //        }
        //        return bookLanguages;
        //    }
        //}

        //public List<Book> GetBooks()
        //{
        //    List<Book> books = new List<Book>();
        //    var commandStr = "SELECT * FROM Book;";
        //    using (connection = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = new SqlCommand(commandStr, connection);
        //        connection.Open();
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                Book book = new Book()
        //                {
        //                    Book_ID = (int)reader["Book_ID"],
        //                    Name = reader["Name"].ToString(),
        //                    Language_ID = (int)reader["Language_ID"],
        //                    NumberOfPages = (int)reader["NumberOfPages"],
        //                    Format_ID = (int)reader["Format_ID"]
        //                };
        //                book.LanguageName = Languages.First(z => z.Language_ID == book.Language_ID).ShortName;
        //                book.FormatName = Formats.First(f => f.Format_ID == book.Format_ID).Name;
        //                book.authors = GetAuthors(book.Book_ID);
        //                books.Add(book);
        //            }
        //        }
        //    }
        //    return books;
        //}

        //public DataTable GetBooksTable()
        //{
        //    var commandStr = "SELECT * FROM Book;";
        //    DataTable books = new BookTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(commandStr, connString);
        //    adapter.Fill(books);
        //    books.Columns.Add("FormatName");
        //    books.Columns.Add("LanguageName");
        //    books.Columns.Add("AuthorsString");
        //    foreach (DataRow row in books.Rows)
        //    {
        //        row["FormatName"] = this.bookFormats.FirstOrDefault(z => z.Format_ID == (int)row["Format_ID"]).Name;
        //        row["LanguageName"] = this.bookFormats.FirstOrDefault(z => z.Format_ID == (int)row["Language_ID"]).Name;
        //        row["AuthorsString"] = Book.GetAuthorsString(GetAuthors((int)row["Book_ID"])); 
        //    }
        //    return books;
        //}
    }
}
