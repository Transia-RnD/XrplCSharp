using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Models.Ledger;

namespace Xrpl.Models.Methods;

public class LONFTokenPage : BaseLedgerEntry
{

    public LONFTokenPage()
    {
        //The type of ledger object (0x0074).
        LedgerEntryType = LedgerEntryType.NFTokenPage;
    }
    public string Flags { get; set; }
    /// <summary>
    /// The locator of the next page, if any. Details about this field and how it should be used are outlined below.
    /// </summary>
    public string NFTokenPage { get; set; }

    /// <summary>
    /// The collection of NFToken objects contained in this NFTokenPage object.
    /// This specification places an upper bound of 32 NFToken objects per page.
    /// Objects are sorted from low to high with the NFTokenID used as the sorting parameter..
    /// </summary>
    public List<NFToken> NFTokens { get; set; }

    /// <summary>
    /// The locator of the previous page, if any. Details about this field and how it should be used are outlined below.
    /// </summary>
    public string PreviousPageMin { get; set; }
    /// <summary>
    /// Identifies the transaction ID of the transaction that most recently modified this NFTokenPage object.
    /// </summary>
    public String PreviousTxnID { get; set; }
    /// <summary>
    /// The sequence of the ledger that contains the transaction that most recently modified this NFTokenPage object.
    /// </summary>
    public long PreviousTxnLgrSeq { get; set; }
}

[JsonConverter(typeof(LONFTokenConverter))]
public class NFToken
{
    public string NFTokenID { get; set; }
    public string URI { get; set; }
}