﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TRAADJEntities : DbContext
    {
        public TRAADJEntities()
            : base("name=TRAADJEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BM_TRANS_TYPE> BM_TRANS_TYPE { get; set; }
        public virtual DbSet<TM_ADJ_APPLICATION> TM_ADJ_APPLICATION { get; set; }
        public virtual DbSet<TM_ADJ_APPLICATION_ITEM> TM_ADJ_APPLICATION_ITEM { get; set; }
    }
}
