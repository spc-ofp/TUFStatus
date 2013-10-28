using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncDB.DAL.Maps.Ves;

namespace TUFStatus.DAL.Configuration
{
    public class SPCNHibernateHelper
    {
        //public static ISessionFactory CreateSessionFactory(string connectionString)
        //{
        //    NHibernate.Cfg.Configuration config = Fluently.Configure().
        //        Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(connectionString)).
        //        Mappings(m => m.FluentMappings.AddFromAssemblyOf<SyncDB.DAL.Maps.Ves.VesselsMap>()).
        //        CurrentSessionContext<ThreadStaticSessionContext>().
        //        BuildConfiguration();
        //    return config.BuildSessionFactory();
        //}

        public static ISessionFactory CreateSessionFactory(string database)
        {
            string connectionString;

            // need to modify db name, add to installations?
            connectionString = "Server=NOUSQL50\\SQLEXPRESS;Database=" + database + ";Trusted_Connection=True";

            NHibernate.Cfg.Configuration config = Fluently.Configure().
                Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(connectionString)).
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<SyncDB.DAL.Maps.Ves.VesselsMap>()).
                CurrentSessionContext<ThreadStaticSessionContext>().
                BuildConfiguration();
            return config.BuildSessionFactory();
        }
    }
}
