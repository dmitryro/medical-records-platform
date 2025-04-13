namespace MedicalAPI.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("roles", Schema = "public")]
public class Role
{
    [Key]
    [Column("id")]
    public uint Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    // CRUD rights (e.g., "create", "read", "update", "delete")
    [Column("permissions")] 
    public string[] Permissions { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
