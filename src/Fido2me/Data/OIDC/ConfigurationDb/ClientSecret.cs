﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ClientSecrets")]
    [Index(nameof(ClientId))]
    public class ClientSecret
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long ClientId { get; set; }
        public Client Client { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(4000)]
        public string Value { get; set; }

        public DateTime? Expiration { get; set; } = null;

        [Required]
        [StringLength(250)]
        public string Type { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}