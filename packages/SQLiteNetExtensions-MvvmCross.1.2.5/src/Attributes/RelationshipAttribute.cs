﻿using System;
#if USING_MVVMCROSS
using IgnoreAttribute = Cirrious.MvvmCross.Community.Plugins.Sqlite.IgnoreAttribute;
#elif PCL
using SQLite.Net.Attributes;
#else
using SQLite;
#endif

namespace SQLiteNetExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class RelationshipAttribute : IgnoreAttribute
    {
        protected RelationshipAttribute(string foreignKey, string inverseForeignKey, string inverseProperty)
        {
            InverseForeignKey = inverseForeignKey;
            InverseProperty = inverseProperty;
            ForeignKey = foreignKey;
        }

        public string ForeignKey { get; private set; }
        public string InverseProperty { get; private set; }
        public string InverseForeignKey { get; private set; }
        public virtual CascadeOperation CascadeOperations { get; set; }
        public bool ReadOnly { get; set; }

        public bool IsCascadeRead { get { return CascadeOperations.HasFlag(CascadeOperation.CascadeRead); } }
        public bool IsCascadeInsert { get { return CascadeOperations.HasFlag(CascadeOperation.CascadeInsert); } }
        public bool IsCascadeDelete { get { return CascadeOperations.HasFlag(CascadeOperation.CascadeDelete); } }
    }
}
