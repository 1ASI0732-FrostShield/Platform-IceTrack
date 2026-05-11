using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;

public partial class EquipmentAudit : IEntityWithCreatedUpdatedDate
{
    [Column("Created")] public DateTimeOffset? CreatedDate { get; set; }
    [Column("Updated")] public DateTimeOffset? UpdatedDate { get; set; }
    [Column("deleted_at")] public DateTimeOffset? DeletedAt { get; set; }
}