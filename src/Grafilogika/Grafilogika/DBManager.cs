using Grafilogika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grafilogika
{
    public class DBManager
    {
        public void AddGame(string name, string uploader, string game)
        {
            Games addThis = new Games();

            using (var dbCtx = new GrafilogikaDBEntities())
            {              
                addThis.Name = name;
                addThis.Uploader = uploader;
                addThis.Wins = 0;
                addThis.Rating = 0;
                addThis.Game = game;
                dbCtx.Games.Add(addThis);

                dbCtx.SaveChanges();
            }
        }

        //EZ LEHET NEM IS KELL
        public void AddUser(string name, string pw, int isadmin)
        {
            Users addThis = new Users();

            using (var dbCtx = new GrafilogikaDBEntities())
            {
                addThis.Name = name;
                //TODO pw mentés nem így
                addThis.Password = pw;
                addThis.Wins = 0;
                addThis.Mistakes = 0;
                addThis.Isadmin = isadmin;
                dbCtx.Users.Add(addThis);

                dbCtx.SaveChanges();
            }
        }

        #region GameQuery
        public List<Games> GetGamesByUploader(string uploader)
        {
            List<Games> result = null;

            using (var dbCtx = new GrafilogikaDBEntities())
            {
                result = (from g in dbCtx.Games
                         where g.Uploader == uploader
                         select g).ToList();
            }

            return result;
        }

        public Games GetGameByName(string name)
        {
            Games result = null;

            using (var dbCtx = new GrafilogikaDBEntities())
            {
                result = (from g in dbCtx.Games
                          where g.Name == name
                          select g).First();
            }

            return result;
        }

        public List<Games> GetAllGames()
        {
            List<Games> result = null;

            using (var dbCtx = new GrafilogikaDBEntities())
            {
                result = (from g in dbCtx.Games
                          select g).ToList();
            }

            return result;
        }
        #endregion

        #region UserQuery
        public Users GetUserByName(string name)
        {
            Users result = null;

            using (var dbCtx = new GrafilogikaDBEntities())
            {
                result = (from u in dbCtx.Users
                          where u.Name == name
                          select u).First();
            }

            return result;
        }
        #endregion

    }
}