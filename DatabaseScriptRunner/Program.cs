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
            DatabaseEntities db = new DatabaseEntities();
            SchemaChange latestChange = (from changes in db.SchemaChanges
                                      orderby changes.MajorVersion descending
                                      select changes).First<SchemaChange>();

            string filePath = args[2];

            if (filePath.Substring(filePath.Length-1) != @"\")
            {
                filePath += @"\";
            }
            string connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;MultipleActiveResultSets=True", args[0], args[1]);
            


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

                    RemoveProcs(cmd);
                    AddProcs(cmd, filePath);
                    RemoveViews(cmd);
                    AddViews(cmd, filePath);
                    RemoveFuncs(cmd);
                    AddFuncs(cmd, filePath);
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
                cmd.CommandText = script.OpenText().ReadToEnd();
                cmd.CommandText = Regex.Replace(cmd.CommandText, "(?I)\\bGO\\b", String.Empty);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
