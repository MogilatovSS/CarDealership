﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarDealershipBeta
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataBaseEntities : DbContext
    {
        private static DataBaseEntities _entities;
        public DataBaseEntities()
            : base("name=DataBaseEntities")
        {
        }
        public static DataBaseEntities GetContext()
        {
            if( _entities == null )
                    _entities = new DataBaseEntities();
            return _entities;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BasketAutopart> BasketAutopart { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<CategoryService> CategoryService { get; set; }
        public virtual DbSet<CategoryService_Warehouse> CategoryService_Warehouse { get; set; }
        public virtual DbSet<Liked> Liked { get; set; }
        public virtual DbSet<PhoneService> PhoneService { get; set; }
        public virtual DbSet<RegistrationService> RegistrationService { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserPark> UserPark { get; set; }
        public virtual DbSet<WarehouseAutopart> WarehouseAutopart { get; set; }
    }
}
