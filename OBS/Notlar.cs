//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OBS
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notlar
    {
        public int Id { get; set; }
        public int OgrenciNo { get; set; }
        public string DersKodu { get; set; }
        public Nullable<int> Vize { get; set; }
        public Nullable<int> Final { get; set; }
        public Nullable<int> But { get; set; }
    
        public virtual Dersler Dersler { get; set; }
        public virtual Ogrenciler Ogrenciler { get; set; }
    }
}
