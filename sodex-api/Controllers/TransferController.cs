using CSharp2nem.Connectivity;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.DataModels;
using CSharp2nem.RequestClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static CSharp2nem.ResponseObjects.Mosaic.MosaicDefinition;

namespace sodex_api.Controllers
{
    public class TransferController : ApiController
    {
        [HttpPost, Route("Send")]
        public String Transaction(Models.TransferData data)
        {
            String message = " ";
            List<CSharp2nem.Model.Transfer.Mosaics.Mosaic> MosaicList = new List<CSharp2nem.Model.Transfer.Mosaics.Mosaic>();
            var connection = new Connection();
            connection.SetTestnet();

            var mosaicClient = new NamespaceMosaicClient(connection);
            var mosaicResult = mosaicClient.BeginGetMosaicsOwned(data.Sender);
            var mosaicResponse = mosaicClient.EndGetMosaicsOwned(mosaicResult);

            foreach (var mosaic in mosaicResponse.Data)
            {
                if (mosaic.MosaicId.Name != "xem")
                {
                    MosaicList.Add(new CSharp2nem.Model.Transfer.Mosaics.Mosaic(mosaic.MosaicId.NamespaceId, mosaic.MosaicId.Name, (data.Amount * 10000)));
                }

            }

            var accountFactory = new PrivateKeyAccountClientFactory(connection);
            var accClient = accountFactory.FromPrivateKey(data.SenderPrivateKey);

            var transData = new TransferTransactionData()
            {
                Amount = 1000000, // amount should always be 1000000 micro xem when attaching mosaics as it acts as a multiplier.
                RecipientAddress = data.Receiver,
                ListOfMosaics = MosaicList
            };
            try
            {
                accClient.BeginSendTransaction(body =>
                {
                    try
                    {
                        if (body.Ex != null) throw body.Ex;
                        message = body.Content.Message;
                    }
                    catch (Exception e)
                    {
                        message = e.Message;
                    }
                }, transData);
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return message;
        }
    }
}