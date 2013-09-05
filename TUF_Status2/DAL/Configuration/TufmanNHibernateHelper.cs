using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUFMAN.DAL.Maps.Ves;

namespace TUFStatus.DAL.Configuration
{
    public class TufmanNHibernateHelper
    {
        public static ISessionFactory CreateSessionFactory(string connectionString)
        {
            NHibernate.Cfg.Configuration config = Fluently.Configure().
                Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(connectionString)).
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<TUFMAN.DAL.Maps.Ves.VesselCategoriesMap>()).
                CurrentSessionContext<ThreadStaticSessionContext>().
                BuildConfiguration();
            return config.BuildSessionFactory();
        }

        public static ISessionFactory CreateSessionFactory()
        {
            string connectionString;

            connectionString = Program.tufmanInstallation.GetConnectionString();
            //connectionString = ""; //"Server=NOUSQL50\\SQLEXPRESS;Database=tufman_ws;Trusted_Connection=True"

            NHibernate.Cfg.Configuration config = Fluently.Configure().
                Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(connectionString)).
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<TUFMAN.DAL.Maps.Ves.VesselCategoriesMap>()).
                CurrentSessionContext<ThreadStaticSessionContext>().
                BuildConfiguration();
            return config.BuildSessionFactory();
        }
    }
}