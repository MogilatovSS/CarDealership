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
    
    public partial class UserPark
    {
        public int User_id { get; set; }
        public int UserPark_id { get; set; }
        public int Car_id { get; set; }
    
        public virtual Car Car { get; set; }
        public virtual User User { get; set; }
    }
}