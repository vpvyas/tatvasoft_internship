﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Mission.Entities.Entities
{
    [Table("Country")] // Explicitly specify the table name
    public class Country
    {
        [Key]
        [Column("id")] // Define the column name and make it lowercase
        public int Id { get; set; }

        [Column("country_name")] // Define the column name and make it lowercase
        public string CountryName { get; set; }

        //public virtual ICollection<Missions> Missions { get; set; } = [];
    }
}
