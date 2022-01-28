﻿using Application.Common.Dtos.ApiRequest;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Token.Commands.Create
{
    public class CreateStandardTokenCommand : IRequest<IResult>
    {
        public string Sender { get; set; }
        public long TotalSupply { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Decimal { get; set; }

        public class CreateStandardTokenCommandHandler : IRequestHandler<CreateStandardTokenCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly ISmartContractsService _smartContractsService;
            private readonly IDomainEventService _domainEventService;
            private readonly IDateTime _dateTime;
            public CreateStandardTokenCommandHandler(ICallContext context, IApplicationDbContext dbContext, ISmartContractsService smartContractsService, IDomainEventService domainEventService, IDateTime dateTime)
            {
                _context = context;
                _dbContext = dbContext;
                _smartContractsService = smartContractsService;
                _domainEventService = domainEventService;
                _dateTime = dateTime;
            }

            public async Task<IResult> Handle(CreateStandardTokenCommand request, CancellationToken cancellationToken)
            {
                var totalSupply = request.TotalSupply * Math.Pow(10, request.Decimal);

                var args = new SmartContractWalletCreateRequestDto()
                {
                    Amount = 0,
                    ContractCode = "4D5A90000300000004000000FFFF0000B800000000000000400000000000000000000000000000000000000000000000000000000000000000000000800000000E1FBA0E00B409CD21B8014CCD21546869732070726F6772616D2063616E6E6F742062652072756E20696E20444F53206D6F64652E0D0D0A2400000000000000504500004C0102004F7450990000000000000000E00022200B0130000010000000020000000000002A2E0000002000000040000000000010002000000002000004000000000000000400000000000000006000000002000000000000030040850000100000100000000010000010000000000000100000000000000000000000D82D00004F000000000000000000000000000000000000000000000000000000004000000C000000BC2D00001C0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200000080000000000000000000000082000004800000000000000000000002E74657874000000300E0000002000000010000000020000000000000000000000000000200000602E72656C6F6300000C0000000040000000020000001200000000000000000000000000004000004200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000C2E0000000000004800000002000500D0230000EC0900000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000E20203280500000A0204280900000602052805000006020E042803000006020E0528070000060202280600000A6F0700000A04280B0000062A4602280800000A72010000706F0900000A2A4A02280800000A7201000070036F0A00000A2A4602280800000A720F0000706F0900000A2A4A02280800000A720F000070036F0A00000A2A4E02280800000A72190000706F0B00000A16912A6E02280800000A7219000070178D0E0000012516039C6F0C00000A2A4602280800000A722B0000706F0D00000A2A4A02280800000A722B000070036F0E00000A2A7202280800000A7243000070038C09000001280F00000A6F0D00000A2A7602280800000A7243000070038C09000001280F00000A046F0E00000A2A0013300400C2000000010000110416281000000A281100000A2C38021201FE1503000002120102280600000A6F0700000A7D010000041201037D02000004120116281000000A7D0300000407280100002B172A0202280600000A6F0700000A280A0000060A0604281300000A2C02162A0202280600000A6F0700000A0604281400000A280B00000602030203280A00000604281500000A280B000006021201FE1503000002120102280600000A6F0700000A7D010000041201037D020000041201047D0300000407280100002B172A000013300500CF000000020000110516281000000A281100000A2C2E021202FE15030000021202037D010000041202047D02000004120216281000000A7D0300000408280100002B172A020302280600000A6F0700000A28100000060A0203280A0000060B0605281300000A2D090705281300000A2C02162A020302280600000A6F0700000A0605281400000A280F00000602030705281400000A280B00000602040204280A00000605281500000A280B000006021202FE15030000021202037D010000041202047D020000041202057D0300000408280100002B172A00133004006A000000030000110202280600000A6F0700000A03281000000604281600000A2C02162A0202280600000A6F0700000A0305280F000006021200FE1504000002120002280600000A6F0700000A7D040000041200037D050000041200057D070000041200047D0600000406280200002B172A8E02280800000A725B000070038C09000001048C09000001281700000A056F0E00000A2A8A02280800000A725B000070038C09000001048C09000001281700000A6F0D00000A2A00000042534A4201000100000000000C00000076342E302E33303331390000000005006C0000000C040000237E0000780400009403000023537472696E6773000000000C08000080000000235553008C0800001000000023475549440000009C0800005001000023426C6F6200000000000000020000015717A201090A000000FA013300160000010000001000000004000000070000001000000019000000010000001700000007000000030000000100000004000000080000000100000003000000020000000200000000008D010100000000000600F50079020600240179020600E10045020F00990200000A000B03ED020E00010058020A00A900ED020A002100ED020A00DD02ED0206009500BC010A001501ED020A006900ED020A00D000ED0206004201BC0106006201BC0106001903BC01000000002900000000000100010001001000E10100001500010001000A011000750100002900010011000A011000690100002900040011000600CB019D00060012029D0006003D03A100060033029D00060023029D0006002C03A10006003D03A10050200000000086183F02A500010089200000000086089F01B00006009B20000000008108AA01B4000600AE200000000086087E00B0000700C0200000000081088700B4000700D32000000000E609BA02B9000800E720000000008108C702BD000800032100000000E6094B03C200090015210000000081085B03C7000900282100000000E6014900CD000A0045210000000081005400D4000B00642100000000E6010A02DC000D00342200000000E601C301E4000F00102300000000E6014D01EE00120086230000000081008101F8001500AA2300000000E6015F000201180000000100BD00000002006B0300000300900000000400B50100000500D40200000100470100000100470100000100470100000100470100000100E50200000100E50200000200470100000100150200000200440300000100D001000002001502000003004403000001002B02000002003603000003004403000001003902000002002B02000003004701000001003902000002002B020200190009003F02010011003F02060019003F020A0059003F02060029003F021000290072001600610018021B0029009F00200069005501250069005F012A006900A80230006900B1023600690013003D0069001E004300790004034A00410020035700410077035D0029007D0165004100D5015D004100EF0171004100FE017100410083035D00790004038D002100230049012E000B0018012E00130021012E001B004001410023004901810023004901A1002300490150007A008300020001000000AE010B0100008B000B010000CB020F0100005F0313010200020003000100030003000200040005000100050005000200060007000100070007000200080009000100090009000480000000000000000000000000000000000B03000004000000000000000000000094003200000000000200000000000000000000000000ED02000000000200000000000000000000000000580200000000030002000400020025006C0025008800000000495374616E64617264546F6B656E3235360047657455496E743235360053657455496E74323536003C4D6F64756C653E0053797374656D2E507269766174652E436F72654C69620047657442616C616E63650053657442616C616E636500416C6C6F77616E636500494D657373616765006765745F4D657373616765006765745F4E616D65007365745F4E616D65006E616D650056616C756554797065006765745F53746174650049536D617274436F6E7472616374537461746500736D617274436F6E74726163745374617465004950657273697374656E7453746174650044656275676761626C6541747472696275746500436F6D70696C6174696F6E52656C61786174696F6E7341747472696275746500496E6465784174747269627574650052756E74696D65436F6D7061746962696C69747941747472696275746500427974650076616C756500417070726F766500476574537472696E6700536574537472696E6700417070726F76616C4C6F67005472616E736665724C6F6700536574417070726F76616C00536D617274436F6E74726163742E646C6C006765745F53796D626F6C007365745F53796D626F6C0073796D626F6C0053797374656D005472616E7366657246726F6D0066726F6D006F705F4C6573735468616E005374616E64617264546F6B656E006F705F5375627472616374696F6E006F705F4164646974696F6E005472616E73666572546F00746F006765745F53656E646572005370656E646572007370656E646572004F776E6572006F776E6572002E63746F720053797374656D2E446961676E6F737469637300537472617469732E536D617274436F6E7472616374732E5374616E64617264730053797374656D2E52756E74696D652E436F6D70696C6572536572766963657300446562756767696E674D6F646573004765744279746573005365744279746573006765745F446563696D616C73007365745F446563696D616C7300646563696D616C730041646472657373006164647265737300537472617469732E536D617274436F6E74726163747300466F726D617400536D617274436F6E7472616374004F626A656374006F705F496D706C69636974004F6C64416D6F756E740063757272656E74416D6F756E7400616D6F756E74006765745F546F74616C537570706C79007365745F546F74616C537570706C7900746F74616C537570706C79006F705F457175616C697479006F705F496E657175616C69747900000000000D530079006D0062006F006C0000094E0061006D006500001144006500630069006D0061006C007300001754006F00740061006C0053007500700070006C0079000017420061006C0061006E00630065003A007B0030007D00002341006C006C006F00770061006E00630065003A007B0030007D003A007B0031007D000000DDF72CC17C666E4FA9162D348C993F730004200101080320000105200101111105200101121D0420001231042000112504200012350420010E0E052002010E0E0520011D050E062002010E1D0505200111210E062002010E11210500020E0E1C0607021121110C050001112108070002021121112106300101011E00040A01110C08000211211121112108070311211121110C0407011110040A0111100600030E0E1C1C087CEC85D7BEA7798E03061125030611210A200501121D11210E0E050320000E042001010E032000050420010105042000112105200101112106200111211125072002011125112107200202112511210920030211251125112109200302112511211121092003011125112511210820021121112511250328000E0328000504280011210801000800000000001E01000100540216577261704E6F6E457863657074696F6E5468726F7773010801000200000000000401000000000000000000000000000000000010000000000000000000000000000000002E000000000000000000001A2E00000020000000000000000000000000000000000000000000000C2E0000000000000000000000005F436F72446C6C4D61696E006D73636F7265652E646C6C0000000000FF25002000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002000000C0000002C3E00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
                    FeeAmount = 0.001,
                    GasLimit = 150000,
                    GasPrice = 100,
                    Parameters = new() { $"12#{totalSupply}", $"4#{request.Name}", $"4#{request.Symbol}", $"2#{request.Decimal}" },
                    Sender = request.Sender,
                    Password = _context.WalletPassword,
                    WalletName = _context.WalletName
                };

                var transactionHash = _smartContractsService.Create(args);

                while (true)
                {
                    try
                    {
                        var receipt = _smartContractsService.Receipt(transactionHash);

                        if (receipt.Success)
                        { 

                            var token = new StandardToken()
                            {
                                ContractAddress = receipt.NewContractAddress,
                                Symbol = request.Symbol,
                                Decimal = request.Decimal,
                                Name = request.Name
                            };

                            _dbContext.StandardTokens.Add(token);

                            var senderToken = new HolderStandardToken()
                            {
                                HolderAddress = request.Sender,
                                ContractAddress = receipt.NewContractAddress,
                                Symbol = request.Symbol,
                                Decimal = request.Decimal,
                                Name = request.Name
                            };

                            _dbContext.HolderStandardTokens.Add(senderToken);

                            await _domainEventService.Publish(new OnTokenCreateEvent()
                            {
                                ContractAddress = receipt.NewContractAddress,
                                Decimal = request.Decimal,
                                Sender = request.Sender,
                                Name = request.Name,
                                Symbol = request.Symbol,
                                TotalSupply = Convert.ToInt64(totalSupply),
                                TransactionHash = transactionHash
                            });

                            _dbContext.SaveChangesAsync().Wait();
                            return await Result.SuccessAsync();
                        }

                        return await Result.FailAsync(receipt.Error);
                    }
                    catch (Exception ex)
                    {
                        Task.Delay(1000).Wait();
                    }
                }
            }
        }
    }
}
