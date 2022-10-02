using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/serverInfo.ts
namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The `server_info` command asks the server for a human-readable version of  various information about the rippled server being queried.<br/>
    /// Expects a  response in the form of a {@link ServerInfoResponse}.
    /// </summary>
    public class ServerInfoRequest : RippleRequest
    {
        public ServerInfoRequest()
        {
            Command = "server_info";
        }
    }

    /// <summary>
    /// Depending on how the rippled server is configured, how long it has been running, and other factors,
    /// a server may be participating in the global XRP Ledger peer-to-peer network to different degrees.<br/>
    /// This is represented as the server_state field in the responses to the server_info method and server_state method.<br/>
    /// The possible responses follow a range of ascending interaction, with each later value superseding the previous one.<br/>
    /// <a>https://xrpl.org/rippled-server-states.html</a>
    /// </summary>
    public enum ServerState
    {
        /// <summary>
        /// The server is not connected to the XRP Ledger peer-to-peer network whatsoever.<br/>
        /// It may be running in offline mode, or it may not be able to access the network for whatever reason.
        /// </summary>
        [EnumMember(Value = "disconnected")]
        Disconnected,
        /// <summary>
        /// The server believes it is connected to the network.
        /// </summary>
        [EnumMember(Value = "connected")]
        Connected,
        /// <summary>
        /// The server is currently behind on ledger versions.<br/>
        /// (It is normal for a server to spend a few minutes catching up after you start it).
        /// </summary>
        [EnumMember(Value = "syncing")]
        Syncing,
        /// <summary>
        /// The server is in agreement with the network.
        /// </summary>
        [EnumMember(Value = "tracking")]
        Tracking,
        /// <summary>
        /// The server is fully caught-up with the network and could participate in validation,
        /// but is not doing so (possibly because it has not been configured as a validator).
        /// </summary>
        [EnumMember(Value = "full")]
        Full,
        /// <summary>
        /// The server is currently participating in validation of the ledger.
        /// </summary>
        [EnumMember(Value = "validating")]
        Validating,
        /// <summary>
        /// The server is participating in validation of the ledger and currently proposing its own version.
        /// </summary>
        [EnumMember(Value = "proposing")]
        Proposing
    }
    
    public class ServerInfo //todo rename to ServerInfoResponse extends BaseResponse 
    {
        [JsonProperty("info")]
        public Info Info { get; set; }
    }

    public class Info
    {
        /// <summary>
        /// The version number of the running rippled version.
        /// </summary>
        [JsonProperty("build_version")]
        public string BuildVersion { get; set; }

        /// <summary>
        /// Range expression indicating the sequence numbers of the ledger versions the local rippled has in its database.
        /// </summary>
        [JsonProperty("complete_ledgers")]
        public string CompleteLedgers { get; set; }

        /// <summary>
        /// On an admin request, returns the hostname of the server running the rippled instance;<br/>
        /// otherwise, returns a single RFC-1751 word based on the node public key.
        /// </summary>
        [JsonProperty("hostid")]
        public string HostId { get; set; }

        /// <summary>
        /// Amount of time spent waiting for I/O operations, in milliseconds.<br/>
        /// If this number is not very, very low, then the rippled server is probably having serious load issues.
        /// </summary>
        [JsonProperty("io_latency_ms")]
        public int IoLatencyMs { get; set; }

        /// <summary>
        /// Information about the last time the server closed a ledger,
        /// including the amount of time it took to reach a consensus and the number of trusted validators participating.
        /// </summary>
        [JsonProperty("last_close")]
        public LastClose LastClose { get; set; }

        /// <summary>
        /// (Admin only) Detailed information about the current load state of the server.
        /// </summary>
        [JsonProperty("load")]
        public Load Load { get; set; }

        /// <summary>
        /// The load-scaled open ledger transaction cost the server is currently enforcing, as a multiplier on the base transaction cost.<br/>
        /// For example, at 1000 load factor and a reference transaction cost of 10 drops of XRP, the load-scaled transaction cost is 10,000 drops (0.01 XRP).<br/>
        /// The load factor is determined by the highest of the individual server's load factor,
        /// the cluster's load factor, the open ledger cost and the overall network's load factor.
        /// </summary>
        [JsonProperty("load_factor")]
        public double? LoadFactor { get; set; }

        /// <summary>
        /// Current multiplier to the transaction cost being used by the rest of the network.
        /// </summary>
        [JsonProperty("load_factor_net")]
        public double? LoadFactorNet { get; set; }

        /// <summary>
        /// How many other rippled servers this one is currently connected to.
        /// </summary>
        [JsonProperty("peers")]
        public int Peers { get; set; }

        /// <summary>
        /// Public key used to verify this server for peer-to-peer communications.<br/>
        /// This node key pair is automatically generated by the server the first   time it starts up.<br/>
        /// (If deleted, the server can create a new pair of Keys).
        /// </summary>
        [JsonProperty("pubkey_node")]
        public string PubkeyNode { get; set; }

        /// <summary>
        /// Public key used by this node to sign ledger validations.
        /// </summary>
        [JsonProperty("pubkey_validator")]
        public string PubkeyValidator { get; set; }

        /// <summary>
        /// A string indicating to what extent the server is participating in the   network.
        /// </summary>
        [JsonProperty("server_state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ServerState ServerState { get; set; }

        /// <summary>
        /// A map of various server states with information about the time the   server spends in each.<br/>
        /// This can be useful for tracking the long-term   health of your server's connectivity to the network.
        /// </summary>
        [JsonProperty("state_accounting")]
        public AccountingStateSummary AccountingStateSummary { get; set; }

        /// <summary>
        /// Number of consecutive seconds that the server has been operational.
        /// </summary>
        [JsonProperty("uptime")]
        public int Uptime { get; set; }

        [JsonIgnore]
        public TimeSpan UptimeTimeSpan => TimeSpan.FromSeconds(Uptime);

        /// <summary>
        /// Information about the most recent fully-validated ledger.
        /// </summary>
        [JsonProperty("validated_ledger")]
        public ValidatedLedger ValidatedLedger { get; set; }

        /// <summary>
        /// Minimum number of trusted validations required to validate a ledger   version.<br/>
        /// Some circumstances may cause the server to require more   validations.
        /// </summary>
        [JsonProperty("validation_quorum")]
        public int ValidationQuorum { get; set; }

        /// <summary>
        /// Either the human readable time, in UTC, when the current validator list will expire,
        /// the string unknown if the server has yet to load a published validator list or the string never if the server uses a static validator list.
        /// </summary>
        [JsonProperty("validator_list_expires")]
        public string ValidatorListExpires { get; set; }

        //todo not found fields -  amendment_blocked?: boolean,  closed_ledger?:, jq_trans_overflow: string, load_factor_local?: number,   load_factor_cluster?: number
        //load_factor_fee_escalation?: number, load_factor_fee_queue?: number, load_factor_server?: number, network_ledger?: 'waiting'
        //   server_state_duration_us: number,  time: string, 
    }

    /// <summary>
    /// Information about the last time the server closed a ledger,
    /// including the amount of time it took to reach a consensus and the number of trusted validators participating.
    /// </summary>
    public class LastClose
    {
        /// <summary>
        /// The amount of time it took to reach a consensus on the most recently  validated ledger version, in seconds.
        /// </summary>
        [JsonProperty("converge_time_s")]
        public double ConvergeTimeS { get; set; }

        /// <summary>
        /// How many trusted validators the server considered (including itself,if configured as a validator)
        /// in the consensus process for the most recently validated ledger version.
        /// </summary>
        [JsonProperty("proposers")]
        public int Proposers { get; set; }
    }

    /// <summary>
    /// (Admin only) Detailed information about the current load state of the server.
    /// </summary>
    public class JobType
    {
        [JsonProperty("job_type")]
        public string JobTypeDescription { get; set; }

        [JsonProperty("per_second")]
        public int PerSecond { get; set; }

        [JsonProperty("in_progress")]
        public int? InProgress { get; set; }

        //todo not found fields peak_time?: number, avg_time?: number,  threads: number
        //https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/serverInfo.ts#L28
    }

    /// <summary>
    /// (Admin only) Detailed information about the current load state of the   server.
    /// </summary>
    public class Load
    {
        /// <summary>
        /// (Admin only) Information about the rate of different types of jobs  the server is doing and how much time it spends on each.
        /// </summary>
        [JsonProperty("job_types")]
        public List<JobType> JobTypes { get; set; }

        /// <summary>
        /// (Admin only) The number of threads in the server's main job pool.
        /// </summary>
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


    /// <summary>
    /// A map of various server states with information about the time the   server spends in each.<br/>
    /// This can be useful for tracking the long-term   health of your server's connectivity to the network.
    /// </summary>
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

    /// <summary>
    /// Information about the most recent fully-validated ledger.
    /// </summary>
    public class ValidatedLedger
    {
        /// <summary>
        /// The time since the ledger was closed, in seconds.
        /// </summary>
        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonIgnore]
        public TimeSpan AgeTimeSpan => TimeSpan.FromSeconds(Age);

        /// <summary>
        /// Base fee, in XRP.<br/>
        /// This may be represented in scientific notation.<br/>
        /// Such as 1e-05 for 0.00005.
        /// </summary>
        [JsonProperty("base_fee_xrp")]
        public double BaseFeeXrp { get; set; }

        /// <summary>
        /// Unique hash for the ledger, as hexadecimal.
        /// </summary>
        [JsonProperty("hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Minimum amount of XRP (not drops) necessary for every account to.<br/>
        /// Keep in reserve.
        /// </summary>
        [JsonProperty("reserve_base_xrp")]
        public uint ReserveBaseXrp { get; set; }

        /// <summary>
        /// Amount of XRP (not drops) added to the account reserve for each  object an account owns in the ledger.
        /// </summary>
        [JsonProperty("reserve_inc_xrp")]
        public uint ReserveIncXrp { get; set; }

        /// <summary>
        /// The ledger index of the latest validated ledger.
        /// </summary>
        [JsonProperty("seq")]
        public int Sequence { get; set; }
    }
}
