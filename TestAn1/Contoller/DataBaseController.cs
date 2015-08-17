using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestAn1
{
    public class DataBaseController
    {
        string databasePath = null;
        SQLiteConnection conn = null;        
        private static DataBaseController linstance = null;
        public static DataBaseController instance()
        {
            if (linstance == null)
                linstance = new DataBaseController();


            return linstance;
        }

        public DataBaseController()
        {
            try
            {
                databasePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "SecretCartManager");
                if(Directory.Exists(databasePath) == false)
                {
                    Directory.CreateDirectory(databasePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }        
        
        public void Open()
        {
            string db = Path.Combine(databasePath, "myData.ucrd");
            try
            {
                conn = new SQLiteConnection(db);
                conn.CreateTable<Users>();
                conn.CreateTable<Cards>();
            }
            catch(SQLiteException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public int GetUserInfo()
        {
            var user = conn.Table<Users>();
            return user.Count();
        }

        public void AddAccount(string uuid , string pwd)
        {
            var nUser = new Users();
            nUser.UUID = uuid;
            nUser.PASSWORD = pwd;
            conn.Insert(nUser);
        }

        public bool Login(string uuid ,string pwd)
        {
            var user = from s in conn.Table<Users>() where s.PASSWORD == pwd && s.UUID == uuid select s;
            if (user.Count() > 0)
                return true;
            else
                return false;
        }

        public void AddCard(string name , string image)
        {
            var card = new Cards();
            card.IMAGE = image;
            card.NAME = name;
            card.REGDATE = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            conn.Insert(card);            
        }
        public List<Cards> GetCardList()
        {
            List<Cards> cList = new List<Cards>();

            var lists = conn.Table<Cards>();
            foreach(Cards c in lists)
            {
                cList.Add(c);
            }

            return cList;
        }

        public byte[] GetCardImage(string name)
        {
            var cardImage = from c in conn.Table<Cards>() where c.NAME == name select c;

            Cards mCard = cardImage.FirstOrDefault();
            return Enumerable.Range(0, mCard.IMAGE.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(mCard.IMAGE.Substring(x, 2), 16)).ToArray();
        }

        public int DeleteCardInfo(string name)
        {
            var cardID = from c in conn.Table<Cards>() where c.NAME == name select c;
            Cards cardData = cardID.FirstOrDefault();
            if (cardData != null)
            {
                int deletedCount = conn.Delete<Cards>(cardData.ID);
                return deletedCount;
            }
            else
                return 0;
        }
    }

    [Table("Users")]
    public class Users
    {
        [PrimaryKey, AutoIncrement , Column("_id")]
        public int ID { get; set; }
        [MaxLength(255), NotNull]
        public string PASSWORD { get; set; }
        [MaxLength(255)]
        public string UUID { get; set; }
    }

    [Table("Cards")]
    public class Cards
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        [NotNull]
        public string IMAGE { get; set; }
        [MaxLength(255) , NotNull]
        public string NAME { get; set; }
        [MaxLength(255), NotNull]
        public string REGDATE { get; set; }
    }
}