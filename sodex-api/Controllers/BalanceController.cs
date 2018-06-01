using CSharp2nem.Connectivity;
using CSharp2nem.RequestClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sodex_api.Controllers
{
    public class BalanceController : ApiController
    {
        [HttpGet, Route("Check")]
        public IHttpActionResult GetBalance([FromUri] Models.Address address) {
            List<Models.MosaicBalanceModel> balance = new List<Models.MosaicBalanceModel>();
            var connection = new Connection();
            connection.SetTestnet();
            try
            {
                // To get mosaic information of the account from the address.
                var mosaicClient = new NamespaceMosaicClient(connection);
                var mosaicResult = mosaicClient.BeginGetMosaicsOwned(address.MosaicAddress);
                var mosaicResponse = mosaicClient.EndGetMosaicsOwned(mosaicResult);

                foreach (var data in mosaicResponse.Data)
                {
                    if (data.MosaicId.Name == "tfc")
                    {
                        balance.Add(new Models.MosaicBalanceModel
                        {
                            Name = data.MosaicId.Name,
                            Amount = data.Quantity / 10000

                        });
                    }
                }
            }
            catch (Exception e) { }

            if (balance.Count == 0) {
                return NotFound();
            }

            return Ok(balance);
        }
    }
}
