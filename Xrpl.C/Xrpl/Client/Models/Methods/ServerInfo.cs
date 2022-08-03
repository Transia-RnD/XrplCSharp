using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xrpl.Client.Models.Methods
{

    public enum ServerState
    {
        [EnumMember(Value = "disconnected")]
        Disconnected,
        [EnumMember(Value = "connected")]
        Connected,
        [EnumMember(Value = "syncing")]
        Syncing,
        [EnumMember(Value = "tracking")]
        Tracking,
        [EnumMember(Value = "full")]
        Full,
        [EnumMember(Value = "validating")]
        Validating,
        [EnumMember(Value = "proposing")]
        Proposing
    }
    
    public class ServerInfo
    {
        [JsonProperty("info")]
        public Info Info { get; set; }
    }

    public class Info
    {
        [JsonProperty("build_version")]
        public string BuildVersion { get; set; }

        [JsonProperty("complete_ledgers")]
        public string CompleteLedgers { get; set; }

        [JsonProperty("hostid")]
        public string HostId { get; set; }

        [JsonProperty("io_latency_ms")]
        public int IoLatencyMs { get; set; }

        [JsonProperty("last_close")]
        public LastClose LastClose { get; set; }

        [JsonProperty("load")]
        public Load Load { get; set; }

        [JsonProperty("load_factor")]
        public double LoadFactor { get; set; }

        [JsonProperty("load_factor_net")]
        public double LoadFactorNet { get; set; }

        [JsonProperty("peers")]
        public int Peers { get; set; }

        [JsonProperty("pubkey_node")]
        public string PubkeyNode { get; set; }

        [JsonProperty("pubkey_validator")]
        public string PubkeyValidator { get; set; }

        [JsonProperty("server_state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ServerState ServerState { get; set; }

        [JsonProperty("state_accounting")]
        public AccountingStateSummary AccountingStateSummary { get; set; }

        [JsonProperty("uptime")]
        public int Uptime { get; set; }

        [JsonIgnore]
        public TimeSpan UptimeTimeSpan => TimeSpan.FromSeconds(Uptime);

        [JsonProperty("validated_ledger")]
        public ValidatedLedger ValidatedLedger { get; set; }

        [JsonProperty("validation_quorum")]
        public int ValidationQuorum { get; set; }

        [JsonProperty("validator_list_expires")]
        public string ValidatorListExpires { get; set; }
    }

    public class LastClose
    {
        [JsonProperty("converge_time_s")]
        public double ConvergeTimeS { get; set; }

        [JsonProperty("proposers")]
        public int Proposers { get; set; }
    }

    public class JobType
    {
        [JsonProperty("job_type")]
        public string JobTypeDescription { get; set; }

        [JsonProperty("per_second")]
        public int PerSecond { get; set; }

        [JsonProperty("in_progress")]
        public int? InProgress { get; set; }
    }

    public class Load
    {
        [JsonProperty("job_types")]
        public List<JobType> JobTypes { get; set; }

        [JsonProperty("threads")]
        public int Threads { get; set; }
    }

    public class AccountingStateInfo
    {
        [JsonProperty("duration_us")]
        public string DurationMs { get; set; }

        [JsonIgnore]
        public TimeSpan Duration
        {
            get
            {
                if (string.IsNullOrEmpty(DurationMs))
                    return TimeSpan.Zero;
                long ms = long.Parse(DurationMs);
                return TimeSpan.FromMilliseconds(ms);
            }
        }
        

        [JsonProperty("transitions")]
        public int Transitions { get; set; }
    }
    

    public class AccountingStateSummary
    {
        [JsonProperty("connected")]
        public AccountingStateInfo Connected { get; set; }

        [JsonProperty("disconnected")]
        public AccountingStateInfo Disconnected { get; set; }

        [JsonProperty("full")]
        public AccountingStateInfo Full { get; set; }

        [JsonProperty("syncing")]
        public AccountingStateInfo Syncing { get; set; }

        [JsonProperty("tracking")]
        public AccountingStateInfo Tracking { get; set; }

        [JsonProperty("validating")]
        public AccountingStateInfo Validating { get; set; }

        [JsonProperty("proposing")]
        public AccountingStateInfo Proposing { get; set; }
    }

    public class ValidatedLedger
    {
        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonIgnore]
        public TimeSpan AgeTimeSpan => TimeSpan.FromSeconds(Age);

        [JsonProperty("base_fee_xrp")]
        public double BaseFeeXrp { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("reserve_base_xrp")]
        public uint ReserveBaseXrp { get; set; }

        [JsonProperty("reserve_inc_xrp")]
        public uint ReserveIncXrp { get; set; }

        [JsonProperty("seq")]
        public int Sequence { get; set; }
    }
}
