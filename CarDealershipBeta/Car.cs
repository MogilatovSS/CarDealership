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
    
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            this.Liked = new HashSet<Liked>();
            this.CategoryService = new HashSet<CategoryService>();
            this.UserPark = new HashSet<UserPark>();
        }
    
        public string Model { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
        public string Supercharger { get; set; }
        public string Type_Engine { get; set; }
        public string Model_Engine { get; set; }
        public string Volume { get; set; }
        public string Power { get; set; }
        public string Transmission { get; set; }
        public string Drive_unit { get; set; }
        public string Carcase { get; set; }
        public string Fuel_consumption { get; set; }
        public string Suspension { get; set; }
        public string Hydraulic_booster { get; set; }
        public string Brakes { get; set; }
        public string Hatch { get; set; }
        public int Car_id { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public Nullable<int> Amount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Liked> Liked { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoryService> CategoryService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserPark> UserPark { get; set; }
    }
}