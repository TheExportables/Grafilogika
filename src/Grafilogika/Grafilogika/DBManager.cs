using Grafilogika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grafilogika
{
    public static class DBManager
    {
        public static void AddGame(string name, string uploader, string game)
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
        public static void AddUser(string name, string pw, int isadmin)
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
        public static List<Games> GetGamesByUploader(string uploader)
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

        public static Games GetGameByName(string name)
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

        public static List<Games> GetAllGames()
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
        public static Users GetUserByName(string name)
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