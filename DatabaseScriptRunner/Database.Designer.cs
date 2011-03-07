﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace DatabaseScriptRunner
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class DatabaseEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new DatabaseEntities object using the connection string found in the 'DatabaseEntities' section of the application configuration file.
        /// </summary>
        public DatabaseEntities() : base("name=DatabaseEntities", "DatabaseEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new DatabaseEntities object.
        /// </summary>
        public DatabaseEntities(string connectionString) : base(connectionString, "DatabaseEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new DatabaseEntities object.
        /// </summary>
        public DatabaseEntities(EntityConnection connection) : base(connection, "DatabaseEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<SchemaChange> SchemaChanges
        {
            get
            {
                if ((_SchemaChanges == null))
                {
                    _SchemaChanges = base.CreateObjectSet<SchemaChange>("SchemaChanges");
                }
                return _SchemaChanges;
            }
        }
        private ObjectSet<SchemaChange> _SchemaChanges;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the SchemaChanges EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToSchemaChanges(SchemaChange schemaChange)
        {
            base.AddObject("SchemaChanges", schemaChange);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="databaseModel", Name="SchemaChange")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class SchemaChange : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new SchemaChange object.
        /// </summary>
        /// <param name="id">Initial value of the id property.</param>
        /// <param name="majorVersion">Initial value of the MajorVersion property.</param>
        /// <param name="minorVersion">Initial value of the MinorVersion property.</param>
        /// <param name="scriptName">Initial value of the ScriptName property.</param>
        /// <param name="dateApplied">Initial value of the DateApplied property.</param>
        public static SchemaChange CreateSchemaChange(global::System.Int32 id, global::System.Int32 majorVersion, global::System.Int32 minorVersion, global::System.String scriptName, global::System.DateTime dateApplied)
        {
            SchemaChange schemaChange = new SchemaChange();
            schemaChange.id = id;
            schemaChange.MajorVersion = majorVersion;
            schemaChange.MinorVersion = minorVersion;
            schemaChange.ScriptName = scriptName;
            schemaChange.DateApplied = dateApplied;
            return schemaChange;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    OnidChanging(value);
                    ReportPropertyChanging("id");
                    _id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("id");
                    OnidChanged();
                }
            }
        }
        private global::System.Int32 _id;
        partial void OnidChanging(global::System.Int32 value);
        partial void OnidChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 MajorVersion
        {
            get
            {
                return _MajorVersion;
            }
            set
            {
                OnMajorVersionChanging(value);
                ReportPropertyChanging("MajorVersion");
                _MajorVersion = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("MajorVersion");
                OnMajorVersionChanged();
            }
        }
        private global::System.Int32 _MajorVersion;
        partial void OnMajorVersionChanging(global::System.Int32 value);
        partial void OnMajorVersionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 MinorVersion
        {
            get
            {
                return _MinorVersion;
            }
            set
            {
                OnMinorVersionChanging(value);
                ReportPropertyChanging("MinorVersion");
                _MinorVersion = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("MinorVersion");
                OnMinorVersionChanged();
            }
        }
        private global::System.Int32 _MinorVersion;
        partial void OnMinorVersionChanging(global::System.Int32 value);
        partial void OnMinorVersionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String ScriptName
        {
            get
            {
                return _ScriptName;
            }
            set
            {
                OnScriptNameChanging(value);
                ReportPropertyChanging("ScriptName");
                _ScriptName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("ScriptName");
                OnScriptNameChanged();
            }
        }
        private global::System.String _ScriptName;
        partial void OnScriptNameChanging(global::System.String value);
        partial void OnScriptNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime DateApplied
        {
            get
            {
                return _DateApplied;
            }
            set
            {
                OnDateAppliedChanging(value);
                ReportPropertyChanging("DateApplied");
                _DateApplied = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DateApplied");
                OnDateAppliedChanged();
            }
        }
        private global::System.DateTime _DateApplied;
        partial void OnDateAppliedChanging(global::System.DateTime value);
        partial void OnDateAppliedChanged();

        #endregion
    
    }

    #endregion
    
}
