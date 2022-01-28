using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Common.Models
{
    public class CirrusCoreErrorResponse
    {
        [JsonProperty("errors")]
        public List<CirrusCoreError> Errors { get; set; }
    }

    public class CirrusCoreError
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
