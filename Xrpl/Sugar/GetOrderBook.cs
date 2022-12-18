
//// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/utils.ts

//namespace Xrpl.Sugar
//{
//    public class UtilsSugar
//    {
//        private const int DEFAULT_LIMIT = 20;
    
//        private static BookOffer[] SortOffers(BookOffer[] offers)
//        {
//        return offers.OrderBy(offer => offer.Quality ?? 0).ToArray();
//        }
        
//        public async Task<(List<BookOffer> buy, List<BookOffer> sell)> GetOrderbook(TakerAmount takerPays, TakerAmount takerGets, int limit = DEFAULT_LIMIT, int ledgerIndex = -1, string ledgerHash = null, string taker = null)
//        {
//            var request = new BookOffersRequest
//            {
//                Command = "book_offers",
//                TakerPays = takerPays,
//                TakerGets = takerGets,
//                LedgerIndex = ledgerIndex == -1 ? "validated" : ledgerIndex.ToString(),
//                LedgerHash = ledgerHash,
//                Limit = limit,
//                Taker = taker
//            };
//            var directOfferResults = await RequestAll(request);
//            request.TakerGets = takerPays;
//            request.TakerPays = takerGets;
//            var reverseOfferResults = await RequestAll(request);
//            var directOffers = directOfferResults.SelectMany(directOfferResult => directOfferResult.Result.Offers).ToList();
//            var reverseOffers = reverseOfferResults.SelectMany(reverseOfferResult => reverseOfferResult.Result.Offers).ToList();
//            var orders = directOffers.Concat(reverseOffers).ToList();
//            var buy = new List<BookOffer>();
//            var sell = new List<BookOffer>();
//            orders.ForEach(order =>
//            {
//                if ((order.Flags & OfferFlags.lsfSell) == 0)
//                {
//                    buy.Add(order);
//                }
//                else
//                {
//                    sell.Add(order);
//                }
//            });
//            return (SortOffers(buy).Take(limit).ToList(), SortOffers(sell).Take(limit).ToList());
//        }
//    }
//}

