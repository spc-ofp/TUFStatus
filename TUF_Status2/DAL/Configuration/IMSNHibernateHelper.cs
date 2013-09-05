using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.DAL.Maps;

namespace TUFStatus.DAL.Configuration
{
    class IMSNHibernateHelper
    {
        //private static ISessionFactory _sessionFactory;

        //public static ISessionFactory SessionFactory
        //{
        //    get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory()); }
        //}

        public static ISessionFactory CreateSessionFactory()
        {
            FluentNHibernate.Cfg.FluentConfiguration config = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard
                .ConnectionString(c => c
                .Host("rimf.ffa.int")       //202.4.229.96
                .Port(5432)
                .Database(Program.tufmanInstallation.PortalDatabase())               // "norma_dev"
                .Username("ofp_admin")
                .Password("ofp_admin")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<IMS.DAL.Maps.Ves.VesselCategoriesMap>());

            //.ExposeConfiguration(con =>
            //    {
            //        con.SetProperty("adonet.batch_size", "1");
            //    })

            return config.BuildSessionFactory();

        }
    }
}
