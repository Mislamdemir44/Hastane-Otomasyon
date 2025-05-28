using System;
using System.Collections.Generic;
using System.Text.Json;

namespace HospitalManagement.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public Guid? UserId { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string IpAddress { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        
        // Helper methods for JSON serialization/deserialization
        public void SetOldValues<T>(T oldValues)
        {
            if (oldValues != null)
            {
                OldValues = JsonSerializer.Serialize(oldValues);
            }
        }
        
        public void SetNewValues<T>(T newValues)
        {
            if (newValues != null)
            {
                NewValues = JsonSerializer.Serialize(newValues);
            }
        }
        
        public T GetOldValues<T>()
        {
            if (string.IsNullOrEmpty(OldValues))
            {
                return default;
            }
            
            return JsonSerializer.Deserialize<T>(OldValues);
        }
        
        public T GetNewValues<T>()
        {
            if (string.IsNullOrEmpty(NewValues))
            {
                return default;
            }
            
            return JsonSerializer.Deserialize<T>(NewValues);
        }
    }
}
