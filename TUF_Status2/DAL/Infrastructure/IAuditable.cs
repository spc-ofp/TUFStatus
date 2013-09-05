using System;

namespace TUF_Status.DAL.Infrastructure
{

    /// <summary>
    /// Interface for marking an entity as auditable.
    /// </summary>
    public interface IAuditable
    {
        string EnteredBy { get; set; }

        DateTime? EnteredDate { get; set; }

        string UpdatedBy { get; set; }

        DateTime? UpdatedDate { get; set; }

        void SetAuditTrail(string userName, DateTime timestamp);
    }
}
