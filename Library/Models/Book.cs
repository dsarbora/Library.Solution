using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
    public class Book
    {
        private int Id;
        private string Title;
        private int NewCopies;

        public Book(string title, int id=0, int newCopies=0)
        {
            Title = title;
            Id = id;
            NewCopies = newCopies;
        }

        public string GetTitle(){ return Title; }
        public int GetId(){ return Id; }
        public void SetCopies(int newCopies){NewCopies=newCopies;}
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO books (title) VALUES (@title);",conn);
            MySqlParameter prmTitle = new MySqlParameter();
            prmTitle.ParameterName = "@title";
            prmTitle.Value = Title;
            cmd.Parameters.Add(prmTitle);
            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }

        public void AddCopies()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO copies (book_id) VALUES (@bookId)", conn);
            MySqlParameter prmBookId = new MySqlParameter();
            prmBookId.ParameterName = "@bookId";
            prmBookId.Value = Id;
            cmd.Parameters.Add(prmBookId);
            for(int i=0; i<NewCopies; i++)
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }    
        }

        public static Book Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM books WHERE id=@id;", conn);
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@id";
            prmId.Value = id;
            cmd.Parameters.Add(prmId);
            string title = "";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                title = rdr.GetString(1);
            }
            Book foundBook = new Book(title, id);
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return foundBook;
        }

        public static List<Book> GetAll()
        {
            List<Book> allBooks = new List<Book> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM books", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                string title = rdr.GetString(1);
                int id = rdr.GetInt32(0);
                Book newBook = new Book(title, id);
                allBooks.Add(newBook);
            }
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return allBooks;
        }

        public List<int> GetAllCopies()
        {
            List<int> allCopies = new List<int>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM copies WHERE book_id=@bookId;", conn);
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@bookId";
            prmId.Value = Id;
            cmd.Parameters.Add(prmId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int copyId = rdr.GetInt32(0);
                allCopies.Add(copyId);
            }
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return allCopies;
        }

        public void Update(string newTitle)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE books SET title=@newTitle WHERE id=@id;", conn);
            MySqlParameter prmNewTitle = new MySqlParameter();
            prmNewTitle.ParameterName = "@newTitle";
            prmNewTitle.Value = newTitle;
            cmd.Parameters.Add(prmNewTitle);
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@id";
            prmId.Value = Id;
            cmd.Parameters.Add(prmId);
            cmd.ExecuteNonQuery();
            Title = newTitle;
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM books; DELETE FROM copies; DELETE FROM books_authors; DELETE FROM books_patrons;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public void DeleteCopy(int copyId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM copies WHERE id=@copyId;", conn);
            MySqlParameter prmCopyId = new MySqlParameter();
            prmCopyId.ParameterName = "@copyId";
            prmCopyId.Value = copyId;
            cmd.Parameters.Add(prmCopyId);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherBook)
        {
            if(!(otherBook is Book))
            {
                return false;
            }
            else
            {  
                Book newBook = (Book)otherBook;
                bool idEquality = this.GetId().Equals(newBook.GetId());
                bool titleEquality = this.GetTitle().Equals(newBook.GetTitle());
                return (titleEquality && idEquality);
            }
        }



        
    }
}
