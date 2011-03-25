using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace DatabaseScriptRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;MultipleActiveResultSets=True", args[0], args[1]);
            string efConnectionString = string.Format("metadata=res://*/Database.csdl|res://*/Database.ssdl|res://*/Database.msl;provider=System.Data.SqlClient;provider connection string=\"{0}\"", connectionString);
            DatabaseEntities db = new DatabaseEntities(efConnectionString);
            
            SchemaChange latestChange = (from changes in db.SchemaChanges
                                      orderby changes.MajorVersion descending
                                      select changes).FirstOrDefault<SchemaChange>();

            if (latestChange == null)
            {
                throw new ApplicationException("The SchemaChange table in your destination database is empty.  It must contain at least 1 baseline record.");
            }
            
            string filePath = args[2];
            if (filePath.Substring(filePath.Length-1) != @"\")
            {
                filePath += @"\";
            }
            

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;

                    DirectoryInfo schemaDir = new DirectoryInfo(filePath + "schema");

                    foreach (FileInfo script in schemaDir.GetFiles("*.sql")
                                .Where(x => Int32.Parse(x.Name.Split('.')[0]) == latestChange.MajorVersion
                                && Int32.Parse(x.Name.Split('.')[1]) > latestChange.MinorVersion))
                    {
                        cmd.CommandText = script.OpenText().ReadToEnd();
                        cmd.CommandText = Regex.Replace(cmd.CommandText, @"(?I)\bGO\b", String.Empty);

                        cmd.ExecuteNonQuery();

                        SchemaChange newChange = new SchemaChange();
                        newChange.MajorVersion = int.Parse(script.Name.Split('.')[0]);
                        newChange.MinorVersion = int.Parse(script.Name.Split('.')[1]);
                        newChange.ScriptName = script.Name;
                        newChange.DateApplied = DateTime.Now;
                        db.SchemaChanges.AddObject(newChange);
                        db.SaveChanges();
                    }

                    RemoveFuncs(cmd);
                    AddFuncs(cmd, filePath);
                    RemoveProcs(cmd);
                    AddProcs(cmd, filePath);
                    RemoveViews(cmd);
                    AddViews(cmd, filePath);
                }
            }
        }

        private static void RemoveProcs(SqlCommand cmd)
        {
            cmd.CommandText = "DECLARE @DropScript varchar(max) = '';"
                + "SELECT @DropScript = @DropScript + 'DROP PROCEDURE [' + schema_name(schema_id)+ '].' + '[' + name + '];' "
                + "FROM sys.procedures;"
                + "exec (@DropScript)";
            cmd.ExecuteNonQuery();
        }

        private static void AddProcs(SqlCommand cmd, string filePath)
        {
            DirectoryInfo procDir = new DirectoryInfo(filePath + "StoredProcedures");
            RunScripts(cmd, procDir.GetFiles("*.sql"));
        }

        private static void RemoveViews(SqlCommand cmd)
        {
            cmd.CommandText = "DECLARE @DropScript varchar(max) = '';"
                + "SELECT @DropScript = @DropScript + 'DROP VIEW [' + schema_name(schema_id)+ '].' + '[' + name + '];' "
                + "FROM sys.views;"
                + "exec (@DropScript)";
            cmd.ExecuteNonQuery();
        }

        private static void AddViews(SqlCommand cmd, string filePath)
        {
            DirectoryInfo viewDir = new DirectoryInfo(filePath + "Views");
            RunScripts(cmd, viewDir.GetFiles("*.sql"));
        }

        private static void RemoveFuncs(SqlCommand cmd)
        {
            cmd.CommandText = "DECLARE @DropScript varchar(max) = '';"
                + "SELECT @DropScript = @DropScript + 'DROP FUNCTION [' + specific_schema + '].' + '[' + specific_name + '];'  "
                + "FROM INFORMATION_SCHEMA.ROUTINES "
                + "WHERE ROUTINE_TYPE = 'FUNCTION';"
                + "exec (@DropScript)";
            cmd.ExecuteNonQuery();
        }

        private static void AddFuncs(SqlCommand cmd, string filePath)
        {
            DirectoryInfo funcDir = new DirectoryInfo(filePath + "Functions");
            RunScripts(cmd, funcDir.GetFiles("*.sql"));
        }

        private static void RunScripts(SqlCommand cmd, FileInfo[] scripts)
        {
            foreach (FileInfo script in scripts)
            {
                Console.WriteLine("Running " + script.Name);
                cmd.CommandText = script.OpenText().ReadToEnd();
                cmd.CommandText = Regex.Replace(cmd.CommandText, "(?I)\\bGO\\b", String.Empty);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
