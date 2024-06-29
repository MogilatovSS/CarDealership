//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class OrderService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderService()
        {
            this.ApplicationOrderService = new HashSet<ApplicationOrderService>();
            this.WarehouseAutopart = new HashSet<WarehouseAutopart>();
            this.Service = new HashSet<Service>();
        }
    
        public int OrderService_id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int Car_id { get; set; }
        public Nullable<int> User_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationOrderService> ApplicationOrderService { get; set; }
        public virtual Car Car { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WarehouseAutopart> WarehouseAutopart { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Service { get; set; }
    }
}