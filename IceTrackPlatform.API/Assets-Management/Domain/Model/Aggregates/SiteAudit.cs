using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;

public partial class SiteAudit : IEntityWithCreatedUpdatedDate
{
    [Column("Created")] public DateTimeOffset? CreatedDate { get; set; }
    [Column("Updated")] public DateTimeOffset? UpdatedDate { get; set; }
    [Column("DeletedAt")] public DateTimeOffset? DeletedAt { get; set; }
}