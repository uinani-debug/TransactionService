using AutoMapper;
using TransactionLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Confluent.Kafka;
using TransactionService.API.Models;
using Newtonsoft.Json;
using System.Threading;
using TransactionLibrary.API.Entities;

namespace TransactionLibrary.API.Controllers
{
    [ApiController]

    public class TransactionController : ControllerBase
    {
        private readonly ITransactionLibraryRepository _TransactionLibraryRepository;
        private readonly IMapper _mapper;
        private readonly ConsumerConfig _config;
        public TransactionController(ITransactionLibraryRepository DebitLibraryRepository,
            IMapper mapper)
        {



            _TransactionLibraryRepository = DebitLibraryRepository ??
                throw new ArgumentNullException(nameof(DebitLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Route("CreditTransaction")]
        [HttpPost]
        public ActionResult<string> CreditTransaction()
        {
            subscribeCreditTransactionEvent();            
            return Accepted();
        }

        [Route("DebitTransaction")]
        [HttpPost]
        public ActionResult<string> DebitTransaction()
        {
            subscribeDebitTransactionEvent();
            return Accepted();
        }


        private void subscribeDebitTransactionEvent()
        {
            var _config = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "3.129.43.25:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                //TODO
                c.Subscribe("AccountDebited");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                        cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            var Request = JsonConvert.DeserializeObject<Request>(cr.Value);
                           
                            if (Request != null)
                            {
                                var TransactionRequest = new Transaction
                                {
                                    AccountIdentifier = Request.AccountIdentifier,
                                    //AvailableBalance = Request.Creditor.,
                                    TransactionAmount = Request.TransferAmount,
                                    TransactionDate = DateTime.Now,
                                    TransactionTowards = Request.PaymentReference,
                                    TransactionType = "Debit"
                                };

                                _TransactionLibraryRepository.TransactionDetails(TransactionRequest);
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }



        private  void subscribeCreditTransactionEvent()
        {
            var _config = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "3.129.43.25:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                //TODO
                c.Subscribe("AccountCredited");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            var Request = JsonConvert.DeserializeObject<Request>(cr.Value);
                           
                            if (Request != null)
                            {
                                var TransactionRequest = new Transaction
                                {
                                    AccountIdentifier = Request.AccountIdentifier,
                                    //AvailableBalance = Request.Creditor.,
                                    TransactionAmount = Request.TransferAmount,
                                    TransactionDate = DateTime.Now,
                                    TransactionTowards = Request.PaymentReference,
                                    TransactionType = "Credit"
                                };

                                _TransactionLibraryRepository.TransactionDetails(TransactionRequest);
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }

        }
    }



}
