namespace IceTrackPlatform.API.Technicians.Domain.Model.Aggregates;

using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

public partial class Technician : IEntityWithCreatedUpdatedDate
{
    [Column("CreatedAt")] public DateTimeOffset? CreatedDate { get; set; }
    [Column("UpdatedAt")] public DateTimeOffset? UpdatedDate { get; set; }
    [Column("deleted_at")] public DateTimeOffset? DeletedAt { get; set; }
}
