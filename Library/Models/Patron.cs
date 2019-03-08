using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
    public class Patron
    {
        private int Id;
        private string Name;

        public Patron(string name, int id=0)
        {
            Name = name;
            Id = id;
        }

        public int GetId() { return Id;}
        
        public string GetName() { return Name;}

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM patrons;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Patron> GetAll()
        {
            List<Patron> allPatrons = new List<Patron>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM patrons;",conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                int id=rdr.GetInt32(0);
                string name=rdr.GetString(1);
                Patron newPatron = new Patron(name, id);
                allPatrons.Add(newPatron);
            }
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return allPatrons;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO patrons (name) VALUES (@name);", conn);
            MySqlParameter prmName = new MySqlParameter();
            prmName.ParameterName = "@name";
            prmName.Value = Name;
            cmd.Parameters.Add(prmName);
            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public static Patron Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM patrons WHERE id=@id;", conn);
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName="@id";
            prmId.Value=id;
            cmd.Parameters.Add(prmId);
            MySqlDataReader rdr = cmd.ExecuteReader();
            string name="";
            while(rdr.Read())
            {
                name=rdr.GetString(1);
            }
            Patron newPatron=new Patron(name, id);
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return newPatron;
        }

        public void Edit(string name)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE patrons SET name=@name WHERE id=@id;", conn);
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@Id";
            prmId.Value = Id;
            cmd.Parameters.Add(prmId);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }
        
        public void AddBook(int bookId, int copyId, bool checkedOut, DateTime checkoutDate, DateTime dueDate)
        {
            MySqlConnection conn=DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO books_patrons (copy_id, book_id, patron_id, checked_out, checkout_date, due_date) VALUES (@copy_id, @book_id, @patron_id, @checked_out, @checkout_date, @due_date);", conn);
            MySqlParameter prmCopyId = new MySqlParameter();
            prmCopyId.ParameterName = "@copy_id";
            prmCopyId.Value = copyId;
            cmd.Parameters.Add(prmCopyId);
            MySqlParameter prmBookId= new MySqlParameter();
            prmBookId.ParameterName = "@book_id";
            prmBookId.Value = bookId;
            cmd.Parameters.Add(prmBookId);
            MySqlParameter prmPatronId = new MySqlParameter();
            prmPatronId.ParameterName = "@patron_id";
            prmPatronId.Value = Id;
            cmd.Parameters.Add(prmPatronId);
            MySqlParameter prmCheckedOut = new MySqlParameter();
            prmCheckedOut.ParameterName = "@checked_out";
            prmCheckedOut.Value = true;
            cmd.Parameters.Add(prmCheckedOut);
            MySqlParameter prmCheckoutDate = new MySqlParameter();
            prmCheckoutDate.ParameterName = "@checkout_date";
            prmCheckoutDate.Value = checkoutDate;
            cmd.Parameters.Add(prmCheckoutDate);
            MySqlParameter prmDueDate = new MySqlParameter();
            prmDueDate.ParameterName = "@due_date";
            prmDueDate.Value = dueDate;
            cmd.Parameters.Add(prmDueDate);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }

        public List<Book> GetCheckedOutBooks()
        {
            List<Book> allBooks = new List<Book>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT books.* FROM patrons JOIN books_patrons bp ON (patrons.id = bp.patron_id) JOIN books ON (bp.book_id = books.id) WHERE patrons.id = @patronId;", conn);
            MySqlParameter prmPatronId = new MySqlParameter();
            prmPatronId.ParameterName = "@patronId";
            prmPatronId.Value = Id;
            cmd.Parameters.Add(prmPatronId);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string title = rdr.GetString(1);
                Book newBook = new Book(title, id);
                allBooks.Add(newBook);
            }
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return allBooks;
        }

        
    }
}