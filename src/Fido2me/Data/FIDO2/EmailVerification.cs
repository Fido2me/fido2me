using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.FIDO2
{
    [Table("EmailVerifications")]
    public class EmailVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public int Code { get; set; }

        public int FailedAttempts { get; set; } = 0;
    }
}
