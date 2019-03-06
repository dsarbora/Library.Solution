using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
    public class Author
    {
        private string Name;
        private int Id;

        public Author(string name, int id=0)
        {
            Name = name;
            Id = id;
        }

        public string GetName() { return Name; }
        public int GetId() { return Id; }
        
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO authors (name) VALUES (@name);", conn);
            MySqlParameter prmName = new MySqlParameter();
            prmName.ParameterName = "@name";
            prmName.Value = Name;
            cmd.Parameters.Add(prmName);
            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
        }

        public static List<Author> GetAll()
        {
            List<Author> allAuthors = new List<Author>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM authors", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                string name = rdr.GetString(1);
                int id = rdr.GetInt32(0);
                Author newAuthor = new Author(name, id);
                allAuthors.Add(newAuthor);
            }
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return allAuthors;
        }

        public static Author Find(int id)
        {
            
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM authors WHERE id=@id;", conn);
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@id";
            prmId.Value = id;
            cmd.Parameters.Add(prmId);
            string name = "";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                name = rdr.GetString(1);
            }
            Author foundAuthor = new Author(name, id);
            conn.Close();
            if(conn!=null)
            {
                conn.Dispose();
            }
            return foundAuthor;
        }

        public void Update(string newName)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE authors SET name=@newName WHERE id=@id;", conn);
            MySqlParameter prmNewName = new MySqlParameter();
            prmNewName.ParameterName = "@newName";
            prmNewName.Value = newName;
            cmd.Parameters.Add(prmNewName);
            MySqlParameter prmId = new MySqlParameter();
            prmId.ParameterName = "@id";
            prmId.Value = Id;
            cmd.Parameters.Add(prmId);
            cmd.ExecuteNonQuery();
            Name = newName;
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
            MySqlCommand cmd = new MySqlCommand("DELETE FROM authors; DELETE FROM books_authors;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherAuthor)
        {
            if(!(otherAuthor is Author))
            {
                return false;
            }
            else
            {  
                Author newAuthor = (Author)otherAuthor;
                bool idEquality = this.GetId().Equals(newAuthor.GetId());
                bool nameEquality = this.GetName().Equals(newAuthor.GetName());
                return (nameEquality && idEquality);
            }
        }

    }
}