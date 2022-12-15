using Microsoft.VisualStudio.TestTools.UnitTesting;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/getOrderbook.ts

namespace Xrpl.Tests.ClientLib
{
    //public class TestUGetOrderBook
    //{

    //    public void CheckSortingOfOrders(List<Order> orders)
    //    {
    //        var previousRate = "0";
    //        foreach (var order in orders)
    //        {
    //            Assert.True(new BigNumber(order.Quality).IsGreaterThanOrEqualTo(previousRate), $"Rates must be sorted from least to greatest: {order.Quality as number} should be >= {previousRate}");
    //            previousRate = order.Quality;
    //        }
    //    }

    //    public static bool IsUSD(string currency)
    //    {
    //        return currency == "USD" || currency == "0000000000000000000000005553440000000000";
    //    }

    //    public static JObject NormalRippledResponse(BookOffersRequest request)
    //    {
    //        if (IsBtc(request.TakerGets.Currency) && IsUsd(request.TakerPays.Currency))
    //        {
    //            return Rippled.BookOffers.Fabric.RequestBookOffersBidsResponse(request);
    //        }
    //        if (IsUsd(request.TakerGets.Currency) && IsBtc(request.TakerPays.Currency))
    //        {
    //            return Rippled.BookOffers.Fabric.RequestBookOffersAsksResponse(request);
    //        }
    //        throw new XrplException("unexpected end");
    //    }
    //}
}

