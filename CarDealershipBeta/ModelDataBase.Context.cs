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
    
    public partial class YourRoadDataBaseEntities : DbContext
    {
        public static YourRoadDataBaseEntities entities;
        public YourRoadDataBaseEntities()
            : base("name=YourRoadDataBaseEntities")
        {
        }
        public static YourRoadDataBaseEntities GetContext()
        {
            if (entities == null)
            {
                entities = new YourRoadDataBaseEntities();
            }

            return entities;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ApplicationOrderService> ApplicationOrderService { get; set; }
        public virtual DbSet<BasketAutopart> BasketAutopart { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<CategoryService> CategoryService { get; set; }
        public virtual DbSet<OrderService> OrderService { get; set; }
        public virtual DbSet<PhoneService> PhoneService { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WarehouseAutopart> WarehouseAutopart { get; set; }
    }
}
