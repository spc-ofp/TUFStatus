using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Criterion;
using TUFStatus.Domain.Cloud.Logs;
using TUFStatus.Domain.Local.Logs;

namespace TUFStatus.DAL.Configuration
{
    public class NHibernateHelper
    {
        private static ISessionFactory _cloudSessionFactory;
        private static ISessionFactory _localSessionFactory;
        //private static string _localFilePath;

        private static ISessionFactory CloudSessionFactory
        {
            get
            {
                if (_cloudSessionFactory == null)
                    InitializeCloudSessionFactory();

                return _cloudSessionFactory;
            }
        }

        private static ISessionFactory LocalSessionFactory
        {
            get
            {
                if (_localSessionFactory == null)
                    InitializeLocalSessionFactory();

                return _localSessionFactory;
            }
        }

        private static void InitializeCloudSessionFactory()
        {

            _cloudSessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard
                    .ConnectionString(c => c
                        .Host(Properties.Settings.Default.cloud_server)       //"rimf.ffa.int" 202.4.229.96
                        .Port(5432)
                        .Database("tuf_status")
                        .Username("ofp_admin")
                        .Password("ofp_admin")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TUFStatus.Domain.Cloud.Logs.ConnectionLogs>())            
                .BuildSessionFactory();

            //.ExposeConfiguration(BuildSchema)
                //.Mappings(m => m.AutoMappings.Add(model))
        }

        public static ISession OpenCloudSession()
        {
            return CloudSessionFactory.OpenSession();
        }

        private static void InitializeLocalSessionFactory()
        {
            string localStatusDBPath = Application.StartupPath + "\\" + Properties.Settings.Default.local_db_file;

            _localSessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .UsingFile(localStatusDBPath))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TUFStatus.Domain.Local.Logs.ActionLogs>())
                .BuildSessionFactory();
        }

        public static ISession OpenLocalSession()
        {
            return LocalSessionFactory.OpenSession();
        }
    }
}